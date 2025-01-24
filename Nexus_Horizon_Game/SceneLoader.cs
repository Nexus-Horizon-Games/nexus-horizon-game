using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game
{
    internal static class SceneLoader
    {
        public static Scene LoadScene()
        {
            var scene = new Scene();

            // TODO: parse the scene from JSON here

            int player = scene.World.CreateEntity();
            scene.World.AddComponent(player, new TransformComponent(new Vector2(0.0f, 0.0f)));
            scene.World.AddComponent(player, new SpriteComponent("guinea_pig", Color.White, scale: 1.0f));

            int player2 = scene.World.CreateEntity();
            var sprite = new SpriteComponent("guinea_pig");
            sprite.scale = 1.0f;
            scene.World.AddComponent(player2, new TransformComponent(new Vector2(5.0f, 10.0f)));
            scene.World.AddComponent(player2, sprite);

            return scene;
        }
    }
}
