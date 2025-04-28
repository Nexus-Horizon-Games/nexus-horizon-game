using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Linq;

namespace Nexus_Horizon_Game.Systems
{
    internal static class BehaviourSystem
    {
        public static void OnNewBehaviourComponent(int entity)
        {
            var behaviourComponent = Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(entity);
            behaviourComponent.Behaviour.Initalize(entity);
        }

        public static void Update(GameTime gameTime)
        {
            var components = Scene.Loaded.ECS.GetComponents<BehaviourComponent>().ToList();

            foreach (var component in components)
            {
                component.Behaviour.OnUpdate(gameTime);
            }
        }
    }
}
