using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.Model
{
    internal abstract class MovementController
    {
        protected Movement movement;

        public MovementController(Movement movement)
        {
            this.movement = movement;
        }

        public virtual void OnUpdate(GameTime gameTime, int entityID)
        {
            Scene.Loaded.ECS.CheckActiveDependency<TransformComponent>(this.GetType(), entityID);
            Scene.Loaded.ECS.CheckActiveDependency<PhysicsBody2DComponent>(this.GetType(), entityID);
            this.movement.OnUpdate(gameTime, entityID);
        }
    }
}
