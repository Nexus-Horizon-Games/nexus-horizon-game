using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Linq;

namespace Nexus_Horizon_Game.Systems
{
    internal static class BehaviourSystem
    {
        public static void Update(GameTime gameTime)
        {
            var components = GameM.CurrentScene.World.GetComponents<BehaviourComponent>().ToList();

            foreach (var component in components)
            {
                component.behaviour.OnUpdate(gameTime);
            }
        }
    }
}
