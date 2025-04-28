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
    internal class GuineaPigBossState : TimedState
    {
        // Constants 
        private const float IdealY = 50.0f;  // Adjust as needed to match your chef boss IdealY.
        private const float MovementVelocity = 4.0f;  // Speed while on-screen.
        private const float TimeBeforeFirstAttack = 1.5f;
        private const float TimeBetweenAttacks = 4.0f;
        private const float MovementInterval = 2.0f; // Update movement every 2 seconds

        // Projectile Sizes
        private const float BigBulletScale = 0.5f;
        private const float SmallBulletScale = 0.2f;

        // Movement Boundaries
        private Vector2 MovementAreaPosition;
        private Vector2 MovementAreaSize;

        // Timers
        private TimerContainer timerContainer = new TimerContainer();
        private int smallSpawnerEntity;
        private int bigSpawnerEntity;

        private PrefabEntity projectile1;
        private PrefabEntity projectile2;
        private IFiringPattern firingPattern;

        public GuineaPigBossState(float timeLength, IFiringPattern? firingPattern = null, PrefabEntity? projectile1 = null, PrefabEntity? projectile2 = null) : base(timeLength)
        {
            this.firingPattern = firingPattern ?? new DirectFiringPattern(7.0f);
            this.projectile1 = projectile1 ?? new PrefabEntity(new List<IComponent>
            {
                new TransformComponent(Vector2.Zero),
                new SpriteComponent("BulletSample", color: Color.White, scale: 0.25f, spriteLayer: 0, centered: true),
                new TagComponent(Tag.ENEMY_PROJECTILE),
            });

            this.projectile2 = projectile2 ?? projectile1;
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

            // Prepare the attack loop (to be started once in the arena)
            timerContainer.AddTimer(new LoopTimer(TimeBetweenAttacks, OnPhase1Attack), "phase1_attack");

            // Prepare the movement timer (but do not start it yet)
            timerContainer.AddTimer(new LoopTimer(MovementInterval, (gt, data) => { Move(gt); }), "move_action");



            // Delay before first attack and then start the repeating attack timer.
            timerContainer.StartTemporaryTimer(new DelayTimer(TimeBeforeFirstAttack, (gt, data) =>
            {
                OnPhase1Attack(gt, null);
                timerContainer.GetTimer("phase1_attack").Start();
            }));

            // Start the movement timer now that we're on-screen.
            timerContainer.GetTimer("move_action").Start();

            this.smallSpawnerEntity = EntitySpawnerFactory.CreateEntitySpawner(projectile1);
            this.bigSpawnerEntity = EntitySpawnerFactory.CreateEntitySpawner(projectile2);
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
            body.Acceleration = body.Velocity * -2.5f;
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, body);
        }

        /// <summary>
        /// Updates the boss's movement direction using a random algorithm similar to the chef boss.
        /// This is called by the movement timer.
        /// </summary>
        private void Move(GameTime gameTime)
        {
            // Get player position
            Vector2 playerPosition = GetPlayerPosition();

            // Get current transform and physics body
            var transform = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity);
            var body = Scene.Loaded.ECS.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);

            float tooCloseToBoundsRange = 40.0f; // Avoid being too near arena edges

            // Base probability factors for movement
            float baseProb = 0.5f;
            float left = baseProb + (transform.position.X - playerPosition.X) / MovementAreaSize.X;
            float right = baseProb + (playerPosition.X - transform.position.X) / MovementAreaSize.X;
            float down = baseProb + (IdealY - transform.position.Y) / MovementAreaSize.Y;
            float up = baseProb + (transform.position.Y - IdealY) / MovementAreaSize.Y;

            left = left < 0.0f ? 0.0f : left;
            right = right < 0.0f ? 0.0f : right;
            down = down < 0.0f ? 0.0f : down;
            up = up < 0.0f ? 0.0f : up;

            // Adjust probabilities near boundaries so the boss moves inward
            if (transform.position.X - MovementAreaPosition.X < tooCloseToBoundsRange)
                left = 0.0f;
            else if ((MovementAreaPosition.X + MovementAreaSize.X) - transform.position.X < tooCloseToBoundsRange)
                right = 0.0f;

            if (transform.position.Y - MovementAreaPosition.Y < tooCloseToBoundsRange)
                up = 0.0f;
            else if ((MovementAreaPosition.Y + MovementAreaSize.Y) - transform.position.Y < tooCloseToBoundsRange)
                down = 0.0f;

            // Choose a random movement direction based on these factors.
            var moveDirection = new Vector2(
                RandomGenerator.GetFloat(-left, right),
                RandomGenerator.GetFloat(-up, down)
            );
            moveDirection.Normalize();
            body.Velocity = moveDirection * MovementVelocity;
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, body);
        }

        /// <summary>
        /// Called periodically during Phase1 to perform an attack.
        /// </summary>
        private void OnPhase1Attack(GameTime gameTime, object? data)
        {
            // Example attack: fire big bullets followed by small bullets after a short delay.
            
            FireBigBullets(gameTime);
            timerContainer.StartTemporaryTimer(new DelayTimer(0.7f, (gt, d) => FireSmallBullets(gameTime)));
        }

        /// <summary>
        /// Helper method to get the player's current position.
        /// </summary>
        private Vector2 GetPlayerPosition()
        {
            var entitiesWithTag = Scene.Loaded.ECS.GetEntitiesWithComponent<TagComponent>();
            foreach (var entity in entitiesWithTag)
            {
                var tag = Scene.Loaded.ECS.GetComponentFromEntity<TagComponent>(entity);
                if (tag.Tag == Tag.PLAYER)
                {
                    return Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(entity).position;
                }
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Fires a burst of big bullets aimed at the player.
        /// </summary>
        private void FireBigBullets(GameTime gameTime)
        {
            incrementAnimation();
            var bossPosition = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            
            var position = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            Scene.Loaded.ECS.SetComponentInEntity(bigSpawnerEntity, new TransformComponent(Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position));
            EntitySpawnerBehaviour entitySpawner = (EntitySpawnerBehaviour)(Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(bigSpawnerEntity).Behaviour);
            entitySpawner.SpawnEntitiesWithPattern(firingPattern, gameTime, timerContainer);
        }

        /// <summary>
        /// Fires a burst of small bullets in a circular pattern.
        /// </summary>
        private void FireSmallBullets(GameTime gameTime)
        {
            incrementAnimation();
            var bossPosition = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            
            var position = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            Scene.Loaded.ECS.SetComponentInEntity(smallSpawnerEntity, new TransformComponent(Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position));
            EntitySpawnerBehaviour entitySpawner = (EntitySpawnerBehaviour)(Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(smallSpawnerEntity).Behaviour);
            entitySpawner.SpawnEntitiesWithPattern(new CicleFiringPattern1(), gameTime, timerContainer);
        }

        public override State Clone()
        {
            GuineaPigBossState clone;

            if (timer is DelayTimer delayTimer)
            {
                clone = new GuineaPigBossState(delayTimer.Delay, firingPattern, projectile1, projectile2);
            }
            else
            {
                clone = new GuineaPigBossState(0.0f, firingPattern, projectile1, projectile2);
            }

            return clone;
        }
    }
}
