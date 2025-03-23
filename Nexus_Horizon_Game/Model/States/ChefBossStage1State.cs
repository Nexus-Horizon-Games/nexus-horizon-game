
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using System;

namespace Nexus_Horizon_Game.States
{
    internal class ChefBossStage1State : TimedState
    {
        private const float IdealY = 40.0f;
        private const int CircleBulletsCount = 16;
        private const float MovementVelocity = 10.0f;
        private const float TimeBeforeFirstAttack = 2.0f;

        private const float TimeBetweenAttacksStage1 = 5.0f;

        // The area that the boss can move in
        private Vector2 MovementAreaPosition;
        private Vector2 MovementAreaSize;

        private BulletFactory bulletFactory = new BulletFactory("BulletSample");

        private TimerContainer timerContainer = new TimerContainer();

        public ChefBossStage1State(int entity, float timeLength) : base(entity, timeLength)
        {
        }

        public override void OnStart()
        {
            base.OnStart();

            MovementAreaPosition = Arena.Position;
            MovementAreaSize = new Vector2(Arena.Size.X, Arena.Size.Y / 2.0f);

            timerContainer.AddTimer(new LoopTimer(TimeBetweenAttacksStage1, OnMoveAction), "move_action");

            // Start movements:
            timerContainer.StartTemporaryTimer(new DelayTimer(TimeBeforeFirstAttack, (gameTime, data) => {
                OnMoveAction(gameTime, null); // start first move
                timerContainer.GetTimer("move_action").Start();
            }));
        }

        public override void OnStop()
        {
            // Stop movements
            timerContainer.GetTimer("move_action").Stop();

            // Stop any velocity or acceleration
            var body = Scene.Loaded.ECS.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
            body.Velocity = Vector2.Zero;
            body.Acceleration = Vector2.Zero;
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, body);

            base.OnStop();
        }

        public override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            timerContainer.Update(gameTime);
            
            // drag:
            var body = Scene.Loaded.ECS.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
            body.Acceleration = body.Velocity * -3.0f;
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, body);
        }

        private void OnMoveAction(GameTime gameTime, object? data)
        {
            // Move:
            Move();

            timerContainer.StartTemporaryTimer(new DelayTimer(0.6f, (gameTime, data) => FireBulletRing(true)));
            timerContainer.StartTemporaryTimer(new DelayTimer(1.6f, (gameTime, data) => FireBulletRing(false)));
            timerContainer.StartTemporaryTimer(new DelayTimer(2.2f, (gameTime, data) => FireBulletPattern1(gameTime)));
        }

        private void Move()
        {
            var playerPosition = GetPlayerPosition();

            // Move:
            var transform = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity);
            var body = Scene.Loaded.ECS.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);

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
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, body);
        }

        private void FireBulletRing(bool counterClockwise)
        {
            const float SpawnRadius = 10.0f;
            const float StartSpeed = 6.0f;

            var bossPosition = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;

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

                var bossPosition = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;
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

        private Vector2 GetPlayerPosition()
        {
            var entitesWithTag = Scene.Loaded.ECS.GetEntitiesWithComponent<TagComponent>();
            var playerEntity = -1;
            foreach (var entity in entitesWithTag)
            {
                var tag = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(entity);
                if (tag.Tag == Tag.PLAYER)
                {
                    playerEntity = entity;
                    break;
                }
            }

            Vector2 playerPosition = Vector2.Zero;
            if (playerEntity != -1)
            {
                playerPosition = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(playerEntity).position;
            }

            return playerPosition;
        }
    }
}
