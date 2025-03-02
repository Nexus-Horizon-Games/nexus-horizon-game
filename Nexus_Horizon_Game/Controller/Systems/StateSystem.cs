using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Diagnostics;
using System.Linq;

namespace Nexus_Horizon_Game.Systems
{
    internal static class StateSystem
    {
        public static void OnNewStateComponent(int entity)
        {
            var stateComponent = Scene.Loaded.ECS.GetComponentFromEntity<StateComponent>(entity);
            stateComponent.states[stateComponent.currentState].OnStopEvent += () => { OnStateStopped(entity); };
            stateComponent.states[stateComponent.currentState].OnStart();
        }

        private static void OnStateStopped(int entity)
        {
            var stateComponent = Scene.Loaded.ECS.GetComponentFromEntity<StateComponent>(entity);
            stateComponent.currentState++;
            if (stateComponent.currentState < 0 || stateComponent.currentState >= stateComponent.states.Count)
            {
                Scene.Loaded.ECS.DestroyEntity(entity);
                return;
            }

            stateComponent.states[stateComponent.currentState].OnStopEvent += () => { OnStateStopped(entity); };
            stateComponent.states[stateComponent.currentState].OnStart();
            Scene.Loaded.ECS.SetComponentInEntity(entity, stateComponent);
        }

        public static void Update(GameTime gameTime)
        {
            var components = Scene.Loaded.ECS.GetComponents<StateComponent>().ToList();

            foreach (var component in components)
            {
                if (component.currentState < 0 || component.currentState >= component.states.Count)
                {
                    continue;
                }

                component.states[component.currentState].OnUpdate(gameTime);
            }
        }
    }
}
