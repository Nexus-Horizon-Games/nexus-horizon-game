using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

using Nexus_Horizon_Game.EntityFactory;

namespace Nexus_Horizon_Game
{
    internal static class SceneLoader
    {
        public static void LoadScene(ref Scene currentScene)
        {
            currentScene = new Scene();
            // TODO: parse the scene from JSON here

            // For Testing
            int npc1 = currentScene.World.CreateEntity();
            var sprite = new SpriteComponent("guinea_pig", spriteLayer: 100);
            sprite.scale = 1.0f;
            currentScene.World.AddComponent(npc1, new TransformComponent(new Vector2(5.0f, 10.0f)));
            currentScene.World.AddComponent(npc1, sprite);

            var playerFactory = new PlayerFactory();
            int moveablePlayer2 = playerFactory.CreateEntity();

            var bulletFactory = new BulletFactory("BulletSample");

            EnemyFactory.CreateBoss("chef_boss");
        }
    }
}
