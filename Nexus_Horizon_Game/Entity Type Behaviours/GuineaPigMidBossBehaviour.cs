using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using Nexus_Horizon_Game.Paths;
using System;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    /// <summary>
    /// Defines the behavior of the evil guinea pig mid-boss.
    /// It enters the arena, moves around with randomized movement (like the chef boss),
    /// attacks on-screen, and then leaves after 15 seconds.
    /// 
    /// The state enum now uses "phase" terminology:
    /// None, Start, EnteringArena, Phase1, Phase2, ExitingArena.
    /// Phase2 is reserved for future implementation.
    /// </summary>
    internal class GuineaPigMidBossBehaviour : Behaviour
    {
        // Constants 
        private const float EnteringSpeed = 4.0f;
        private const float IdealY = 50.0f;  // Adjust as needed to match your chef boss IdealY.
        private const float MovementVelocity = 4.0f;  // Speed while on-screen.
        private const float TimeBeforeFirstAttack = 1.5f;
        private const float TimeBetweenAttacks = 4.0f;
        private const float Phase1Length = 15.0f;  // Duration in Phase1 before exiting.
        private const float MovementInterval = 2.0f; // Update movement every 2 seconds

        // Projectile Sizes
        private const float BigBulletScale = 0.5f;
        private const float SmallBulletScale = 0.2f;
        private const float MediumBulletScale = 0.35f;

        // Bullet Factories
        private readonly BulletFactory bulletFactoryBig = new BulletFactory("BulletSample");
        private readonly BulletFactory bulletFactorySmall = new BulletFactory("BulletSample");
        private readonly BulletFactory bulletFactoryMedium = new BulletFactory("BulletSample");

        // Movement Boundaries
        private readonly Vector2 MovementAreaPosition;
        private readonly Vector2 MovementAreaSize;

        // Exit point (similar to the chef boss exit point)
        private readonly Vector2 ExitPoint;

        // For exiting movement path
        private IPath? currentPath = null;
        private float currentPathTime = 0.0f;

        // Timers
        private TimerContainer timerContainer = new TimerContainer();

        public GuineaPigMidBossBehaviour(int thisEntity) : base(thisEntity)
        {
            MovementAreaPosition = GameM.CurrentScene.ArenaPosition;
            MovementAreaSize = new Vector2(GameM.CurrentScene.ArenaSize.X, GameM.CurrentScene.ArenaSize.Y / 2.0f);
            // Exit at the top-center (off-screen)
            ExitPoint = new Vector2(GameM.CurrentScene.ArenaSize.X / 2.0f, -20.0f);
        }

        // This enum uses "phase" terminology.
        public enum GuineaPigMidBossState : int
        {
            None,
            Start,
            EnteringArena,
            Phase1,         // On-screen fight (first phase)
            Phase2,         // Reserved for later (second phase)
            ExitingArena,   // Boss leaving the arena
        }

        /// <summary>
        /// Called every frame.
        /// </summary>
        public override void OnUpdate(GameTime gameTime)
        {
            var stateComponent = GameM.CurrentScene.World.GetComponentFromEntity<StateComponent>(this.Entity);
            var currentState = (GuineaPigMidBossState)stateComponent.state;

            timerContainer.Update(gameTime);

            switch (currentState)
            {
                case GuineaPigMidBossState.Start:
                    StartState();
                    break;

                case GuineaPigMidBossState.EnteringArena:
                    EnteringArenaState(gameTime);
                    break;

                case GuineaPigMidBossState.Phase1:
                    // In Phase1, the movement is updated by the timer.
                    // Here we only apply the drag effect.
                    HandlePhase1();
                    break;

                case GuineaPigMidBossState.ExitingArena:
                    UpdateExiting(gameTime);
                    break;

                    // Phase2 is reserved for future implementation.
            }
        }

        /// <summary>
        /// Initial state: set starting position, add movement timer, and begin entering.
        /// Also prepares the attack timer.
        /// </summary>
        private void StartState()
        {
            // Prepare the attack loop (to be started once in the arena)
            timerContainer.AddTimer(new LoopTimer(TimeBetweenAttacks, OnPhase1Attack), "phase1_attack");

            // Prepare the movement timer (but do not start it yet)
            timerContainer.AddTimer(new LoopTimer(MovementInterval, (gt, data) => { Move(gt); }), "move_action");

            // Position the mid-boss off-screen (above)
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new TransformComponent(new Vector2(Renderer.DrawAreaWidth / 2.0f, -30.0f)));

            // Begin moving into the arena
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
            body.Velocity = new Vector2(0.0f, EnteringSpeed);
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);

            // Transition to EnteringArena state
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(GuineaPigMidBossState.EnteringArena));
        }

        /// <summary>
        /// While entering, once the boss reaches its ideal Y, transition into Phase1.
        /// Also schedules the exit after Phase1Length seconds.
        /// </summary>
        private void EnteringArenaState(GameTime gameTime)
        {
            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);

            if (transform.position.Y >= IdealY)
            {
                // Delay before first attack and then start the repeating attack timer.
                timerContainer.StartTemporaryTimer(new DelayTimer(TimeBeforeFirstAttack, (gt, data) =>
                {
                    OnPhase1Attack(gt, null);
                    timerContainer.GetTimer("phase1_attack").Start();
                }));

                // Schedule exit after Phase1Length seconds.
                timerContainer.StartTemporaryTimer(new DelayTimer(Phase1Length, (gt, data) =>
                {
                    StartExitingArena();
                }));

                // Start the movement timer now that we're on-screen.
                timerContainer.GetTimer("move_action").Start();

                // Transition to Phase1 (on-screen fight)
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(GuineaPigMidBossState.Phase1));
            }
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
            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);

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
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);
        }

        /// <summary>
        /// Applies a drag effect to slow down movement (similar to the chef boss).
        /// </summary>
        private void HandlePhase1()
        {
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
            body.Acceleration = body.Velocity * -2.5f;
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);
        }

        /// <summary>
        /// Called periodically during Phase1 to perform an attack.
        /// </summary>
        private void OnPhase1Attack(GameTime gameTime, object? data)
        {
            // Example attack: fire big bullets followed by small bullets after a short delay.
            FireBigBullets();
            timerContainer.StartTemporaryTimer(new DelayTimer(0.7f, (gt, d) => FireSmallBullets()));
        }

        /// <summary>
        /// Initiates the exit sequence for the mid boss.
        /// Stops attack timers and movement, then schedules an exit path.
        /// </summary>
        private void StartExitingArena()
        {
            // Transition to ExitingArena state
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(GuineaPigMidBossState.ExitingArena));

            // Stop attack and movement timers
            timerContainer.GetTimer("phase1_attack").Stop();
            timerContainer.GetTimer("move_action").Stop();

            // Stop current movement
            var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
            body.Velocity = Vector2.Zero;
            body.Acceleration = Vector2.Zero;
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);

            // After a short delay, set up the exit path.
            timerContainer.StartTemporaryTimer(new DelayTimer(2.0f, (gameTime, data) =>
            {
                var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
                // Create a straight line path from current position to the exit point.
                currentPath = new LinePath(transform.position, ExitPoint);
                currentPathTime = 0.0f;
            }));
        }

        /// <summary>
        /// Updates the boss position along the exit path until it leaves the arena.
        /// </summary>
        private void UpdateExiting(GameTime gameTime)
        {
            if (currentPath != null)
            {
                // Increase progress along the exit path.
                currentPathTime += currentPath.GetDeltaT(currentPathTime, 15.0f * (float)gameTime.ElapsedGameTime.TotalSeconds);

                var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
                transform.position = currentPath.GetPoint(currentPathTime);
                GameM.CurrentScene.World.SetComponentInEntity(this.Entity, transform);

                // When the exit path is complete, remove the mid boss.
                if (currentPathTime > 1.0f)
                {
                    GameM.CurrentScene.World.DestroyEntity(this.Entity);
                }
            }
        }

        /// <summary>
        /// Helper method to get the player's current position.
        /// </summary>
        private Vector2 GetPlayerPosition()
        {
            var entitiesWithTag = GameM.CurrentScene.World.GetEntitiesWithComponent<TagComponent>();
            foreach (var entity in entitiesWithTag)
            {
                var tag = GameM.CurrentScene.World.GetComponentFromEntity<TagComponent>(entity);
                if (tag.Tag == Tag.PLAYER)
                {
                    return GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(entity).position;
                }
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Fires a burst of big bullets aimed at the player.
        /// </summary>
        private void FireBigBullets()
        {
            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            var playerPosition = GetPlayerPosition();

            // Calculate direction from boss to player.
            Vector2 baseDirection = playerPosition - bossPosition;
            baseDirection.Normalize();

            // Base angle toward player.
            float baseAngle = (float)Math.Atan2(baseDirection.Y, baseDirection.X);
            float angleStep = MathHelper.ToRadians(15); // Angle between bullets.

            for (int burst = 0; burst < 3; burst++)
            {
                timerContainer.StartTemporaryTimer(new DelayTimer(0.3f * burst, (gameTime, data) =>
                {
                    for (int i = -3; i <= 3; i++) // Create a spread pattern.
                    {
                        float angle = baseAngle + (i * angleStep);
                        Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                        bulletFactoryBig.CreateEntity(bossPosition, direction, 7.0f, scale: BigBulletScale, spriteLayer: 99);
                    }
                }));
            }
        }

        /// <summary>
        /// Fires a burst of small bullets in a circular pattern.
        /// </summary>
        private void FireSmallBullets()
        {
            var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            float arcInterval = MathHelper.TwoPi / 27;

            for (int burst = 0; burst < 2; burst++)
            {
                timerContainer.StartTemporaryTimer(new DelayTimer(0.5f * burst, (gameTime, data) =>
                {
                    for (int j = 0; j < 27; j++)
                    {
                        Vector2 direction = new Vector2((float)Math.Cos(j * arcInterval), (float)Math.Sin(j * arcInterval));
                        bulletFactorySmall.CreateEntity(bossPosition, direction, 5.0f, scale: SmallBulletScale, spriteLayer: 99);
                    }
                }));
            }
        }
    }
}