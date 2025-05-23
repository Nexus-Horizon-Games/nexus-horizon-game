﻿
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.EntityFactory;
using Nexus_Horizon_Game.Model.EntityPatterns;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Timers;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.States
{
    internal class ChefBossStage1State : TimedState
    {
        private const float IdealY = 40.0f;
        private const float MovementVelocity = 10.0f;
        private const float TimeBeforeFirstAttack = 2.0f;

        private const float TimeBetweenAttacksStage1 = 5.0f;

        // The area that the boss can move in
        private Vector2 MovementAreaPosition;
        private Vector2 MovementAreaSize;

        private TimerContainer timerContainer = new TimerContainer();
        private int spawnerEntity;

        private PrefabEntity projectile;
        private IFiringPattern firingPattern;

        public ChefBossStage1State(float timeLength, IFiringPattern? firingPattern = null, PrefabEntity? projectile = null) : base(timeLength)
        {
            this.firingPattern = firingPattern ?? new DirectFiringPattern(7.0f);
            this.projectile = projectile ?? new PrefabEntity(new List<IComponent>
            {
                new TransformComponent(Vector2.Zero),
                new SpriteComponent("BulletSample", color: Color.White, scale: 0.25f, spriteLayer: 0, centered: true),
                new TagComponent(Tag.ENEMY_PROJECTILE),
            });
        }

        public override void Initalize(int entity)
        {
            base.Initalize(entity);

            // Make sure acceleration is enabled
            if (Scene.Loaded.ECS.EntityHasComponent<PhysicsBody2DComponent>(entity, out PhysicsBody2DComponent component))
            {
                component.AccelerationEnabled = true;
            }
            else
            {
                Scene.Loaded.ECS.AddComponent<PhysicsBody2DComponent>(entity, new PhysicsBody2DComponent(accelerationEnabled: true));
            }
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

            this.spawnerEntity = EntitySpawnerFactory.CreateEntitySpawner(projectile);
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
            if (body.AccelerationEnabled == false)
            {
                body.AccelerationEnabled = true;
            }

            body.Acceleration = body.Velocity * -3.0f;
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, body);

        }

        private void OnMoveAction(GameTime gameTime, object? data)
        {
            // Move:
            Move();

            timerContainer.StartTemporaryTimer(new DelayTimer(0.6f, (gameTime, data) => FireBulletRing(gameTime,true)));
            timerContainer.StartTemporaryTimer(new DelayTimer(1.6f, (gameTime, data) => FireBulletRing(gameTime,false)));
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

        private void FireBulletRing(GameTime gameTime, bool counterClockwise)
        {
            var position = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            Scene.Loaded.ECS.SetComponentInEntity(spawnerEntity, new TransformComponent(Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position));
            EntitySpawnerBehaviour entitySpawner = (EntitySpawnerBehaviour)(Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(spawnerEntity).Behaviour);
            if (counterClockwise)
            {
                entitySpawner.SpawnEntitiesWithPattern(new CounterClockwiseRingFiringPattern1(), gameTime, timerContainer);
            }
            else
            {
                entitySpawner.SpawnEntitiesWithPattern(new ClockwiseRingFiringPattern1(), gameTime, timerContainer);
            }
        }

        private void FireBulletPattern1(GameTime gameTime)
        {
            Scene.Loaded.ECS.SetComponentInEntity(spawnerEntity, new TransformComponent(Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position));
            EntitySpawnerBehaviour entitySpawner = (EntitySpawnerBehaviour)(Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(spawnerEntity).Behaviour);
            entitySpawner.SpawnEntitiesWithPattern(firingPattern, gameTime, timerContainer);
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

        public override State Clone()
        {
            ChefBossStage1State clone;

            if (timer is DelayTimer delayTimer)
            {
                clone = new ChefBossStage1State(delayTimer.Delay, firingPattern, projectile);
            }
            else
            {
                clone = new ChefBossStage1State(0.0f, firingPattern, projectile);
            }

            return clone;
        }
    }
}
