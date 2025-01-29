using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal class BulletFactory : EntityFactory
    {
        private Scene scene;
        private string textureName;
        public BulletFactory(ref Scene scene, string textureName)
        {
            this.scene = scene;
            this.textureName = textureName;
        }

        /// <summary>
        /// Creates bullets
        /// </summary>
        /// <returns> entity ID. </returns>
        public override int CreateEntity()
        {
            return scene.World.CreateEntity(new List<IComponent>
            { new TransformComponent(new Vector2(0.0f, 0.0f)),
                new SpriteComponent(textureName, color: Color.White, scale: 0.25f, z: 0.0f),
                new PhysicsBody2DComponent(),
              new BulletComponent() });
        }
    }
}
