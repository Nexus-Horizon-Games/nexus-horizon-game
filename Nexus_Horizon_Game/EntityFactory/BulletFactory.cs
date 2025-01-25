using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal class BulletFactory : EntityFactory
    {
        private Scene scene;

        public BulletFactory(ref Scene scene)
        {
            this.scene = scene;
        }

        public override int CreateEntity()
        {
            return scene.World.CreateEntity(new List<IComponent>
            { new TransformComponent(new Vector2(0.0f, 0.0f)),
              new SpriteComponent(1),
              new PhysicsBody2DComponent(),
              new BulletComponent() });
        }
    }
}
