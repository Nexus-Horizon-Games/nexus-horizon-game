using Microsoft.Xna.Framework.Input;
using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;

namespace Nexus_Horizon_Game.View.InputSystem
{
    internal class MenuInput : InputSystem
    {
        protected override void LoadInput()
        {
            InputSystem.AddOnKeyDownListener(Keys.Up, MoveSelectionUp);
            InputSystem.AddOnKeyDownListener(Keys.Down, MoveSelectionDown);
            InputSystem.AddOnKeyDownListener(Keys.X, SelectOption);
        }

        private static void MoveSelectionUp()
        {
            Scene.Loaded.ECS.GetEntitiesWithComponent<BehaviourComponent>(out Dictionary<int, BehaviourComponent> entites);
            if (entites != null)
            {
                foreach (BehaviourComponent mainMenuBehaviorComp in entites.Values)
                {
                    if (mainMenuBehaviorComp.Behaviour is MenuBehavior mainMenuBehavior)
                    {
                        mainMenuBehavior.GoUpState();
                    }
                }
            }
        }

        private static void MoveSelectionDown()
        {
            Scene.Loaded.ECS.GetEntitiesWithComponent<BehaviourComponent>(out Dictionary<int, BehaviourComponent> entites);
            if (entites != null)
            {
                foreach (BehaviourComponent mainMenuBehaviorComp in entites.Values)
                {
                    if (mainMenuBehaviorComp.Behaviour is MenuBehavior mainMenuBehavior)
                    {
                        mainMenuBehavior.GoDownState();
                    }
                }
            }
        }

        private static void SelectOption()
        {
            Scene.Loaded.ECS.GetEntitiesWithComponent<BehaviourComponent>(out Dictionary<int, BehaviourComponent> entites);
            if (entites != null)
            {
                foreach (BehaviourComponent mainMenuBehaviorComp in entites.Values)
                {
                    if (mainMenuBehaviorComp.Behaviour is MenuBehavior mainMenuBehavior)
                    {
                        mainMenuBehavior.SelectStateOption();
                    }
                }
            }
        }
    }
}
