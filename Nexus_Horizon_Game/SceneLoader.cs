using Nexus_Horizon_Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game
{
    internal static class SceneLoader
    {
        public static Scene LoadScene()
        {
            var scene = new Scene();

            // TODO: parse the scene from JSON here

            /*int player = scene.World.CreateEntity(new List<IComponent>{ 
                new TransformComponent(new Vector2(0.0f, 0.0f)),
                new SpriteComponent(0)
            });*/

            int player = scene.World.CreateEntity();
            scene.World.AddComponent(player, new TransformComponent(new Vector2(1.0f, 0.0f)));
            scene.World.AddComponent(player, new SpriteComponent(4));

            int enemy1 = scene.World.CreateEntity();
            scene.World.AddComponent(enemy1, new TransformComponent(new Vector2(2.0f, 0.0f)));
            scene.World.AddComponent(enemy1, new SpriteComponent(1));

            int enemy2 = scene.World.CreateEntity();
            scene.World.AddComponent(enemy2, new SpriteComponent(56));

            int enemy3 = scene.World.CreateEntity();
            scene.World.AddComponent(enemy3, new TransformComponent(new Vector2(3.0f, 0.0f)));

            var list = scene.World.GetComponents<TransformComponent>();
            var list2 = scene.World.GetComponents<SpriteComponent>();
            var entities = scene.World.GetEntitiesWithComponent<TransformComponent>();
            var entities2 = scene.World.GetEntitiesWithComponent<SpriteComponent>();

            return scene;
        }
    }
}
