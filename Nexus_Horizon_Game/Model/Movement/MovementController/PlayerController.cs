using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.Model
{
    internal class PlayerController : MovementController
    {
        bool isSlowed = true;
        Vector2 direction = new(0f, 0f);

        public PlayerController(Movement movement) : base(movement)
        {
        }

        public override void OnUpdate(GameTime gameTime, int entityID)
        {
            base.OnUpdate(gameTime, entityID);

            if (isSlowed) { this.movement.CurrentSpeed *= 0.4f; }
            PhysicsBody2DComponent physicsBody = Scene.Loaded.ECS.GetComponentFromEntity<PhysicsBody2DComponent>(entityID);


            // Set New Velocity
            physicsBody.Velocity = this.movement.Move(direction);
            Scene.Loaded.ECS.SetComponentInEntity<PhysicsBody2DComponent>(entityID, physicsBody);



            // Reset the direction and speed.
            direction = new Vector2(0, 0);
            isSlowed = false;
        }

        /// <summary>
        /// Constrains player to the arena
        /// </summary>
        /// <param name="physicsBody2DComponent"> physics body of entity. </param>
        /// <returns> possible new physics body set by constriant. </returns>
        private void ConstrainPlayerInArena(ref PhysicsBody2DComponent physics, ref TransformComponent transform)
        {
            Vector2 arenaBoundaryDirection = Arena.CheckEntityInArena(transform, out Vector2 boundaryIn, 0.92f, 0.95f);
            float xNewVelocity = physics.Velocity.X;
            float yNewVelocity = physics.Velocity.Y;

            if (physics.Velocity.X > 0 && arenaBoundaryDirection.X == 1)
            {
                xNewVelocity = 0;
                transform.position.X = boundaryIn.X;
            }
            else if (physics.Velocity.X < 0 && arenaBoundaryDirection.X == -1)
            {
                xNewVelocity = 0;
                transform.position.X = boundaryIn.X;
            }

            if (physics.Velocity.Y > 0 && arenaBoundaryDirection.Y == -1)
            {
                yNewVelocity = 0;
                transform.position.Y = boundaryIn.Y;
            }
            else if (physics.Velocity.Y < 0 && arenaBoundaryDirection.Y == 1)
            {
                yNewVelocity = 0;
                transform.position.Y = boundaryIn.Y;
            }

            physics.Velocity = new Vector2(xNewVelocity, yNewVelocity);
        }

        public void Up()
        {
            direction.Y--;
        }

        public void Down()
        {
            direction.Y++;
        }

        public void Left()
        {
            direction.X--;
        }

        public void Right()
        {
            direction.X++;
        }

        public void Slow()
        {
            this.isSlowed = true;
        }
    }
}
