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

            int player = scene.ECS.CreateEntity(new List<IComponent>{ 
                new TransformComponent(new Vector2(0.0f, 0.0f))
            });

            return scene;
        }
    }
}
