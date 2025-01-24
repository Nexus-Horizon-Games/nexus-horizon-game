using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using System.Numerics;

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
            scene.World.AddComponent(player, new SpriteComponent(4));

            int player2 = scene.World.CreateEntity();
            var sprite = new SpriteComponent(4);
            sprite.rotation = 2.0f;
            sprite.scale = 6.0f;
            scene.World.AddComponent(player2, new TransformComponent(new Vector2(300.0f, 200.0f)));
            scene.World.AddComponent(player2, sprite);

            return scene;
        }
    }
}
