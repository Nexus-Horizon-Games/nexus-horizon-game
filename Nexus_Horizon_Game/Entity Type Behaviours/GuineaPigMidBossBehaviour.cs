using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Timers;
using System;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
	/// <summary>
	/// Defines the behavior of the evil guinea pig mid-boss.
	/// </summary>
	internal class GuineaPigMidBossBehaviour : Behaviour
	{
		// Constants 
		private const float EnteringSpeed = 4.0f;
		private const float IdealY = 50.0f;
		private const float MovementVelocity = 4.0f;
		private const float TimeBeforeFirstAttack = 1.5f;
		private const float TimeBetweenAttacks = 4.0f;

		// Projectile Sizes
		private const float BigBulletScale = 0.5f;
		private const float SmallBulletScale = 0.2f;
		private const float MediumBulletScale = 0.35f;

		// Bullets
		private readonly BulletFactory bulletFactoryBig = new BulletFactory("BulletSample");
		private readonly BulletFactory bulletFactorySmall = new BulletFactory("BulletSample");
		private readonly BulletFactory bulletFactoryMedium = new BulletFactory("BulletSample");

		// Movement Boundaries
		private readonly Vector2 MovementAreaPosition;
		private readonly Vector2 MovementAreaSize;
		//private int movementDirection = 1; // 1 = Right, -1 = Left

		//private float movementTimer = 0f; // Tracks movement progression
		//private const float CircleRadius = 80f; // Adjusts the size of the circular motion
		//private const float CircleSpeed = 0.75f; // Controls how fast the boss moves in the circle

		// Ensures the movement is centered in the middle of the screen
		private readonly Vector2 movementCenter = new Vector2(Renderer.DrawAreaWidth / 2, 150);

		// Timers
		private TimerContainer timerContainer = new TimerContainer();

		public GuineaPigMidBossBehaviour(int thisEntity) : base(thisEntity)
		{
			MovementAreaPosition = GameM.CurrentScene.ArenaPosition;
			MovementAreaSize = new Vector2(GameM.CurrentScene.ArenaSize.X, GameM.CurrentScene.ArenaSize.Y / 2.0f);
		}

		public enum GuineaPigMidBossState
		{
			None,
			Start,
			EnteringArena,
			Phase1,
			Phase2
		}

		private void Move(GameTime gameTime)
		{
			var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);

			// Stop movement completely
			body.Velocity = Vector2.Zero;
			GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);
		}


		public override void OnUpdate(GameTime gameTime)
		{
			var state = GameM.CurrentScene.World.GetComponentFromEntity<StateComponent>(this.Entity);

			timerContainer.Update(gameTime);

			if ((GuineaPigMidBossState)state.state == GuineaPigMidBossState.Start)
			{
				StartState();
			}
			else if ((GuineaPigMidBossState)state.state == GuineaPigMidBossState.EnteringArena)
			{
				EnteringArenaState(gameTime);
			}
			else if ((GuineaPigMidBossState)state.state == GuineaPigMidBossState.Phase1)
			{
				Move(gameTime);  //  move before attacking **
				HandlePhase1();
			}
			else if ((GuineaPigMidBossState)state.state == GuineaPigMidBossState.Phase2)
			{
				Move(gameTime);  //  Ensure movement continues in phase 2 **
				HandlePhase2();
			}
		}

		private void StartState()
		{
			timerContainer.AddTimer(new LoopTimer(TimeBetweenAttacks, OnPhase1Attack), "phase1_attack");

			GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new TransformComponent(new Vector2(Renderer.DrawAreaWidth / 2.0f, -30.0f)));

			var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
			body.Velocity = new Vector2(0.0f, EnteringSpeed);
			GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);

			GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(GuineaPigMidBossState.EnteringArena));
		}

		private void EnteringArenaState(GameTime gameTime)
		{
			var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);

			if (transform.position.Y >= IdealY)
			{
				timerContainer.StartTemporaryTimer(new DelayTimer(TimeBeforeFirstAttack, (gameTime, data) => {
					OnPhase1Attack(gameTime, null);
					timerContainer.GetTimer("phase1_attack").Start();
				}));

				GameM.CurrentScene.World.SetComponentInEntity(this.Entity, new StateComponent(GuineaPigMidBossState.Phase1));
			}
		}

		private void HandlePhase1()
		{
			// Drag effect to slow down movement
			var body = GameM.CurrentScene.World.GetComponentFromEntity<PhysicsBody2DComponent>(this.Entity);
			body.Acceleration = body.Velocity * -2.5f;
			GameM.CurrentScene.World.SetComponentInEntity(this.Entity, body);
		}

		private void HandlePhase2()
		{
			// Handles phase 2 movement 
		}

		// Phase 1 Attack Pattern
		private void OnPhase1Attack(GameTime gameTime, object? data)
		{
			FireBigBullets();
			timerContainer.StartTemporaryTimer(new DelayTimer(0.7f, (gameTime, data) => FireSmallBullets()));
		}

		private Vector2 GetPlayerPosition()
		{
			var entitiesWithTag = GameM.CurrentScene.World.GetEntitiesWithComponent<TagComponent>();

			foreach (var entity in entitiesWithTag)
			{
				var tag = GameM.CurrentScene.World.GetComponentFromEntity<TagComponent>(entity);

				if (tag.Tag == Tag.PLAYER) // Check if the entity is the player
				{
					return GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(entity).position;
				}
			}

			return Vector2.Zero; // Default position if player not found
		}

		private void FireBigBullets()
		{
			var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;
			var playerPosition = GetPlayerPosition();

			// Calculate direction from boss to player
			Vector2 baseDirection = playerPosition - bossPosition;
			baseDirection.Normalize();

			// Calculate the base angle towards the player
			float baseAngle = (float)Math.Atan2(baseDirection.Y, baseDirection.X);

			float angleStep = MathHelper.ToRadians(15); // Spread angle per bullet

			for (int burst = 0; burst < 3; burst++)
			{
				timerContainer.StartTemporaryTimer(new DelayTimer(0.3f * burst, (gameTime, data) =>
				{
					for (int i = -3; i <= 3; i++)  // Spread pattern (-3 to 3)
					{
						float angle = baseAngle + (i * angleStep); // Adjust bullet angle based on player
						Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

						bulletFactoryBig.CreateEntity(bossPosition, direction, 7.0f, scale: BigBulletScale, spriteLayer: 99);
					}
				}));
			}
		}

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

		private void FireMediumBullets()
		{
			var bossPosition = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity).position;
			Vector2 forward = new Vector2(0, 1);

			for (int i = 0; i < 4; i++)
			{
				float angleOffset = MathHelper.PiOver4 / (i + 1);
				Vector2 leftBullet = new Vector2((float)Math.Cos(-angleOffset), (float)Math.Sin(-angleOffset));
				Vector2 rightBullet = new Vector2((float)Math.Cos(angleOffset), (float)Math.Sin(angleOffset));

				bulletFactoryMedium.CreateEntity(bossPosition, forward, 8.0f, scale: MediumBulletScale, spriteLayer: 99);
				bulletFactoryMedium.CreateEntity(bossPosition, leftBullet, 8.0f, scale: MediumBulletScale, spriteLayer: 99);
				bulletFactoryMedium.CreateEntity(bossPosition, rightBullet, 8.0f, scale: MediumBulletScale, spriteLayer: 99);
			}
		}
	}
}