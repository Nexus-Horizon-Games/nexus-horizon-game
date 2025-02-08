using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Paths;
using Nexus_Horizon_Game.Timers;
using System;
using System.Diagnostics;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    /// <summary>
    /// Class that contains the scripts and behavior of the chef boss (the final boss).
    /// </summary>
    internal class ChefBossBehaviour : Behaviour
    {
        private const float EnteringSpeed = 5.0f;
        private const float IdealY = 40.0f;
        private const int CircleBulletsCount = 16;
        private const float MovementVelocity = 10.0f;
        private const float TimeBeforeFirstAttack = 2.0f;

        private const float TimeBetweenAttacksStage1 = 5.0f;

        private readonly Vector2 ExitPoint = new Vector2(GameM.CurrentScene.ArenaSize.X / 2.0f, -20.0f);

        private const float Stage1Length = 35.0f;
        private const float Stage2Length = 30.0f;

        // The area that the boss can move in
        private readonly Vector2 MovementAreaPosition;
        private readonly Vector2 MovementAreaSize;

        private BulletFactory bulletFactory = new BulletFactory("BulletSample");

        private TimerContainer timerContainer = new TimerContainer();

        private IPath? currentPath = null;
        private float currentPathTime = 0.0f;

        public ChefBossBehaviour(int thisEntity) : base(thisEntity)
        {
            MovementAreaPosition = GameM.CurrentScene.ArenaPosition;
            MovementAreaSize = new Vector2(GameM.CurrentScene.ArenaSize.X, GameM.CurrentScene.ArenaSize.Y / 2.0f);
        }

        public enum ChefBossState : int
        {
            None,
            Start,          // The starting state
            EnteringArena,  // When the boss is entering the arena
            Stage1,         // The frist stage of the fight
            Stage2,         // the second stage of the fight
            ExitingArena,
        }

        public override void OnUpdate(GameTime gameTime)
        {
            var state = GameM.CurrentScene.World.GetComponentFromEntity<StateComponent>(this.Entity);

            timerContainer.Update(gameTime);

            if ((ChefBossState)state.state == ChefBossState.Start)
            {
                StartState();
            }
            else if ((ChefBossState)state.state == ChefBossState.EnteringArena)
            {
                EnteringArenaState(gameTime);
            }
            else if ((ChefBossState)state.state == ChefBossState.Stage1 || (ChefBossState)state.state == ChefBossState.Stage2)
            {
                // drag:
                var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
                body.Acceleration = body.Velocity * -3.0f;
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);
            }
            else if ((ChefBossState)state.state == ChefBossState.ExitingArena)
            {
                if (currentPath != null)
                {
                    currentPathTime += currentPath.GetDeltaT(currentPathTime, 15.0f * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
                    transform.position = currentPath.GetPoint(currentPathTime);
                    GameM.CurrentScene.World.SetComponentInEntity(this.Entity, transform);

                    if (currentPathTime > 1.0f)
                    {
                        OnExit();
                    }
                }
            }
        }

        private void StartState()
        {
            timerContainer.AddTimer(new LoopTimer(TimeBetweenAttacksStage1, OnMoveAction), "move_action");

            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new TransformComponent(new Vector2(Renderer.DrawAreaWidth / 2.0f, -20.0f)));

            // Start moving into the arena
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
            body.Velocity = new Vector2(0.0f, EnteringSpeed);
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);

            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(ChefBossState.EnteringArena));
        }

        private void EnteringArenaState(GameTime gameTime)
        {
            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);

            if (transform.position.Y >= IdealY) // If reached the start y position
            {
                StartState1();
            }
        }

        private void StartState1()
        {
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(ChefBossState.Stage1));

            // Start movements:
            timerContainer.StartTemporaryTimer(new DelayTimer(TimeBeforeFirstAttack, (gameTime, data) => {
                OnMoveAction(gameTime, null); // start first move
                timerContainer.GetTimer("move_action").Start();
            }));

            timerContainer.StartTemporaryTimer(new DelayTimer(Stage1Length, (gameTime, data) =>
            {
                StartStage2();
            }));
        }

        private void StartStage2()
        {
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(ChefBossState.Stage2));

            // Stop movements for now:
            timerContainer.GetTimer("move_action").Stop();

            // Start movements again after some time:
            timerContainer.StartTemporaryTimer(new DelayTimer(5.0f, (gameTime, data) => {
                OnMoveAction(gameTime, null);
                timerContainer.GetTimer("move_action").Start();
            }));

            timerContainer.StartTemporaryTimer(new DelayTimer(Stage2Length, (gameTime, data) =>
            {
                StartExitingArena();
            }));
        }

        private void StartExitingArena()
        {
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(ChefBossState.ExitingArena));

            // Stop movements
            timerContainer.GetTimer("move_action").Stop();

            // Stop any velocity or acceleration
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
            body.Velocity = Vector2.Zero;
            body.Acceleration = Vector2.Zero;
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);

            timerContainer.StartTemporaryTimer(new DelayTimer(2.0f, (gameTime, data) =>
            {
                // Set exit path
                var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
                currentPath = new LinePath(transform.position, ExitPoint);
            }));
        }

        private void OnExit()
        {
            GameM.CurrentScene.World.DestroyEntity(this.Entity);
        }

        private void OnMoveAction(GameTime gameTime, object? data)
        {
            // Move:
            Move();

            var state = (ChefBossState)GameM.CurrentScene.World.GetComponentFromEntity<StateComponent>(this.Entity).state;

            // Fire bullets:
            if (state == ChefBossState.Stage1)
            {
                timerContainer.StartTemporaryTimer(new DelayTimer(0.6f, (gameTime, data) => FireBulletRing(true)));
                timerContainer.StartTemporaryTimer(new DelayTimer(1.6f, (gameTime, data) => FireBulletRing(false)));
                timerContainer.StartTemporaryTimer(new DelayTimer(2.2f, (gameTime, data) => FireBulletPattern1(gameTime)));
            }
            else if (state == ChefBossState.Stage2)
            {
                timerContainer.StartTemporaryTimer(new DelayTimer(0.6f, (gameTime, data) => FireBulletRing2(true)));
                timerContainer.StartTemporaryTimer(new DelayTimer(1.6f, (gameTime, data) => FireBulletRing2(false)));
                timerContainer.StartTemporaryTimer(new DelayTimer(2.2f, (gameTime, data) => FireBulletPattern2(gameTime)));
            }
        }

        private void Move()
        {
            var playerPosition = GetPlayerPosition();

            // Move:
            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);

            float tooCloseToBoundsRange = 40.0f; // If the boss is within this amount of a wall, then it should go away from it

            // "Probability" (not exactally) of moving a particular direction
            float baseProb = 0.5f;
            float left = baseProb + (transform.position.X - playerPosition.X) / MovementAreaSize.X;
            float right = baseProb + (playerPosition.X - transform.position.X) / MovementAreaSize.X;
            float down = baseProb + (IdealY - transform.position.Y) / MovementAreaSize.Y;
            float up = baseProb + (transform.position.Y - IdealY) / MovementAreaSize.Y;

            // Make sure the values are greater than or equal to 0
            left = left < 0.0f ? 0.0f : left;
            right = right < 0.0f ? 0.0f : right;
            down = down < 0.0f ? 0.0f : down;
            up = up < 0.0f ? 0.0f : up;

            // Make sure to go away from the boundaries:
            if (transform.position.X - MovementAreaPosition.X < tooCloseToBoundsRange)
            {
                left = 0.0f;
            }
            else if ((MovementAreaPosition.X + MovementAreaSize.X) - transform.position.X < tooCloseToBoundsRange)
            {
                right = 0.0f;
            }

            if (transform.position.Y - MovementAreaPosition.Y < tooCloseToBoundsRange)
            {
                up = 0.0f;
            }
            else if ((MovementAreaPosition.Y + MovementAreaSize.Y) - transform.position.Y < tooCloseToBoundsRange)
            {
                down = 0.0f;
            }

            // Move in a random direction
            var moveDirection = new Vector2(RandomGenerator.GetFloat(-left, right), RandomGenerator.GetFloat(-up, down));
            moveDirection.Normalize();
            body.Velocity = moveDirection * MovementVelocity;
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);
        }

        private void FireBulletRing(bool counterClockwise)
        {
            const float SpawnRadius = 10.0f;
            const float StartSpeed = 6.0f;

            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            float arcInterval = MathHelper.TwoPi / CircleBulletsCount;

            for (int i = 0; i < CircleBulletsCount; i++)
            {
                Vector2 direction = new Vector2((float)Math.Cos(arcInterval * i), (float)Math.Sin(arcInterval * i));
                Vector2 perpendicularDirection;

                if (counterClockwise)
                {
                    perpendicularDirection = new Vector2(direction.Y, -direction.X);
                }
                else
                {
                    perpendicularDirection = new Vector2(-direction.Y, direction.X);
                }

                var bulletEntity = bulletFactory.CreateEntity(bossPosition + direction * SpawnRadius, direction + (perpendicularDirection * 0.25f), StartSpeed);
            }
        }

        private void FireBulletRing2(bool counterClockwise)
        {
            const float SpawnRadius = 10.0f;
            const float StartSpeed = 6.0f;

            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;

            float arcInterval = MathHelper.TwoPi / CircleBulletsCount;

            for (int i = 0; i < CircleBulletsCount; i++)
            {
                Vector2 direction = new Vector2((float)Math.Cos(arcInterval * i), (float)Math.Sin(arcInterval * i));
                Vector2 perpendicularDirection;

                if (counterClockwise)
                {
                    perpendicularDirection = new Vector2(direction.Y, -direction.X);
                }
                else
                {
                    perpendicularDirection = new Vector2(-direction.Y, direction.X);
                }

                var bulletEntity = bulletFactory.CreateEntity(bossPosition + direction * SpawnRadius, direction + (perpendicularDirection * 0.25f), StartSpeed, bulletAction: (gametime, bullet, bulletEntity, previousVelocity) =>
                {
                    if (bullet.TimeAlive > 0.3f && bullet.TimeAlive < 1.2f)
                    {
                        Vector2 acceleration = direction * 15.0f;
                        return (previousVelocity - (acceleration * (float)gametime.ElapsedGameTime.TotalSeconds));
                    }
                    else if (bullet.TimeAlive < 2.5f)
                    {
                        Vector2 acceleration = direction * 10.0f;
                        return (previousVelocity + (acceleration * (float)gametime.ElapsedGameTime.TotalSeconds));
                    }

                    return previousVelocity;
                });
            }
        }

        private void FireBulletPattern1(GameTime gameTime)
        {
            const float SpawnTimeInterval = 0.01f;
            const float SpawnTimeLength = 0.8f;

            const float StartSpawnRadius = 1.0f;
            const float SpawnRadiusAcceleration = 50.0f;

            const float StartRotationSpeed = 10.0f;
            const float RotationSpeedAcceleration = 1.0f;

            const float Spawner1Angle = 0.0f;
            const float Spawner2Angle = MathHelper.Pi;

            const float BulletSpeed = 3.0f;


            timerContainer.StartTemporaryTimer(new LoopTimer(SpawnTimeInterval, (gameTime, data) =>
            {
                double startTime = (double)data;
                double time = gameTime.TotalGameTime.TotalSeconds - startTime;

                float spawnRadius = StartSpawnRadius + (float)(time * time) * SpawnRadiusAcceleration;
                float rotationSpeed = StartRotationSpeed + (float)(time * time) * RotationSpeedAcceleration;

                var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;
                var playerPosition = GetPlayerPosition();

                // Spawner positions:
                Vector2 spawner1 = new Vector2((float)Math.Cos(Spawner1Angle + time * rotationSpeed), (float)Math.Sin(Spawner1Angle + time * rotationSpeed)) * spawnRadius;
                Vector2 spawner2 = new Vector2((float)Math.Cos(Spawner2Angle + time * rotationSpeed), (float)Math.Sin(Spawner2Angle + time * rotationSpeed)) * spawnRadius;

                // Spawner shoot directions:
                Vector2 spawner1Direction = playerPosition - (spawner1 + bossPosition);
                spawner1Direction.Normalize();

                Vector2 spawner2Direction = playerPosition - (spawner2 + bossPosition);
                spawner2Direction.Normalize();

                // Spawn the bullets:
                bulletFactory.CreateEntity(bossPosition + spawner1, spawner1Direction, BulletSpeed);
                bulletFactory.CreateEntity(bossPosition + spawner2, spawner2Direction, BulletSpeed);

            }, data: gameTime.TotalGameTime.TotalSeconds, stopAfter: SpawnTimeLength));
        }

        private void FireBulletPattern2(GameTime gameTime)
        {
            const float SpawnTimeInterval = 0.01f;
            const float SpawnTimeLength = 1.5f;

            const float StartSpawnRadius = 5.0f;
            const float SpawnRadiusAcceleration = 15.0f;

            const float StartRotationSpeed = 10.0f;
            const float RotationSpeedAcceleration = 1.0f;

            const float Spawner1Angle = 0.0f;
            const float Spawner2Angle = MathHelper.TwoPi / 3.0f;
            const float Spawner3Angle = MathHelper.TwoPi * 2.0f / 3.0f;

            const float BulletSpeed = 6.0f;


            timerContainer.StartTemporaryTimer(new LoopTimer(SpawnTimeInterval, (gameTime, data) =>
            {
                double startTime = (double)data;
                double time = gameTime.TotalGameTime.TotalSeconds - startTime;

                float spawnRadius = StartSpawnRadius + (float)(time * time) * SpawnRadiusAcceleration;
                float rotationSpeed = StartRotationSpeed + (float)(time * time) * RotationSpeedAcceleration;

                var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;
                var playerPosition = GetPlayerPosition();

                // Spawner positions:
                Vector2 spawner1 = new Vector2((float)Math.Cos(Spawner1Angle + time * rotationSpeed), (float)Math.Sin(Spawner1Angle + time * rotationSpeed)) * spawnRadius;
                Vector2 spawner2 = new Vector2((float)Math.Cos(Spawner2Angle + time * rotationSpeed), (float)Math.Sin(Spawner2Angle + time * rotationSpeed)) * spawnRadius;
                Vector2 spawner3 = new Vector2((float)Math.Cos(Spawner3Angle + time * rotationSpeed), (float)Math.Sin(Spawner3Angle + time * rotationSpeed)) * spawnRadius;

                // Spawner shoot directions:
                Vector2 spawner1Direction = playerPosition - (spawner1 + bossPosition); // Point towards the player
                spawner1Direction.Normalize();

                // Get the angle between spawner1Direction and <1, 0>
                double angle = Math.Acos((double)Vector2.Dot(spawner1Direction, new Vector2(1.0f, 0.0f)));

                Vector2 spawner2Direction = new Vector2((float)Math.Cos(Spawner2Angle + angle), (float)Math.Sin(Spawner2Angle + angle));
                Vector2 spawner3Direction = new Vector2((float)Math.Cos(Spawner3Angle + angle), (float)Math.Sin(Spawner3Angle + angle));

                // Spawn the bullets:
                bulletFactory.CreateEntity(bossPosition + spawner1, spawner1Direction, BulletSpeed);
                bulletFactory.CreateEntity(bossPosition + spawner2, spawner2Direction, BulletSpeed);
                bulletFactory.CreateEntity(bossPosition + spawner3, spawner3Direction, BulletSpeed);

            }, data: gameTime.TotalGameTime.TotalSeconds, stopAfter: SpawnTimeLength));
        }

        private Vector2 GetPlayerPosition()
        {
            var entitesWithTag = GameM.CurrentScene.World.GetEntitiesWithComponent<TagComponent>();
            var playerEntity = -1;
            foreach (var entity in entitesWithTag)
            {
                var tag = GameM.CurrentScene.World.GetComponentFromEntity<TagComponent>(entity);
                if (tag.Tag == Tag.PLAYER)
                {
                    playerEntity = entity;
                    break;
                }
            }

            Vector2 playerPosition = Vector2.Zero;
            if (playerEntity != -1)
            {
                playerPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(playerEntity).position;
            }

            return playerPosition;
        }
    }
}
