using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal class PlayerFactory : EntityFactory
    {
        private Scene scene;

        public PlayerFactory(ref Scene scene)
        {
            this.scene = scene;
        }

        public override int CreateEntity()
        {
            return scene.World.CreateEntity(new List<IComponent>
            { new TransformComponent(new Vector2(100.0f, 100.0f)),
              new SpriteComponent("guinea_pig"),
              new PhysicsBody2DComponent(),
              new PlayerComponent() });
        }
    }
}
