using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

using Nexus_Horizon_Game.EntityFactory;

namespace Nexus_Horizon_Game
{
    internal static class SceneLoader
    {
        public static Scene LoadScene()
        {
            var scene = new Scene();
            // TODO: parse the scene from JSON here

            int npc0 = scene.World.CreateEntity();
            scene.World.AddComponent(npc0, new TransformComponent(new Vector2(0.0f, 0.0f)));
            scene.World.AddComponent(npc0, new SpriteComponent("guinea_pig", Color.White, scale: 1.0f));

            int npc1 = scene.World.CreateEntity();
            var sprite = new SpriteComponent("guinea_pig");
            sprite.scale = 1.0f;
            scene.World.AddComponent(npc1, new TransformComponent(new Vector2(5.0f, 10.0f)));
            scene.World.AddComponent(npc1, sprite);

            var playerFactory = new PlayerFactory(ref scene);
            int moveablePlayer2 = playerFactory.CreateEntity();

            var bulletFactory = new BulletFactory(ref scene, "BulletSample");
            int bullet3 = bulletFactory.CreateEntity();

            EnemyFactory.CreateBoss(scene.World, "chef_boss");

            return scene;
        }
    }
}
