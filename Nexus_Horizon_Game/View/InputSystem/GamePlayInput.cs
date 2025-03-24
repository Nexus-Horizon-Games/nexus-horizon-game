using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model;
using Microsoft.Xna.Framework.Input;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.Scenes;

namespace Nexus_Horizon_Game.View.InputSystem
{
    internal class GamePlayInput : InputSystem
    {
        public enum GamePlayInputState
        {
            UnPaused,
            Paused,
            DiedMenu,
            WinMenu
        }

        public static GamePlayInputState state = GamePlayInputState.UnPaused;
        private static Behaviour currentMenuBehavior = null;
        private static bool isCollisionVisible = true;


        public static void SetToDiedMenu()
        {
            currentMenuBehavior = Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>((Scene.Loaded as GameplayScene).DeathMenuUIID).Behaviour as MenuBehavior;
            (currentMenuBehavior as MenuBehavior).ShowMenu();
            state = GamePlayInputState.DiedMenu;
            GameM.IsGamePaused = true;
        }

        public static void SetToWinMenu()
        {
            currentMenuBehavior = Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>((Scene.Loaded as GameplayScene).WinMenuUIID).Behaviour as MenuBehavior;
            (currentMenuBehavior as MenuBehavior).ShowMenu();
            state = GamePlayInputState.WinMenu;
            GameM.IsGamePaused = true;
        }

        public static void SetToUnPaused()
        {
            currentMenuBehavior = null;
            state = GamePlayInputState.UnPaused;
            GameM.IsGamePaused = false;
        }

        protected override void LoadInput()
        {
            // Initial State
            state = GamePlayInputState.UnPaused;

            // Visibility For Collision Box
            InputSystem.AddOnKeyDownListener(Keys.LeftShift, AddCollisionVisibility);
            InputSystem.AddOnKeyUpListener(Keys.LeftShift, RemoveCollisionVisibility);

            // Pause Menu
            InputSystem.AddOnKeyDownListener(Keys.Escape, Pause_UnPauseGame);
            InputSystem.AddOnKeyDownListener(Keys.Up, MoveSelectionUp);
            InputSystem.AddOnKeyDownListener(Keys.Down, MoveSelectionDown);
            InputSystem.AddOnKeyDownListener(Keys.X, SelectionOption);

            // Movement
            InputSystem.OnUpdate += MovementCheck;
        }

        /* Player Conroller Movement Input */
        private static void MovementCheck()
        {
            if (state == GamePlayInputState.UnPaused)
            {
                foreach (int entityID in Scene.Loaded.ECS.GetEntitiesWithComponent<MovementControllerComponent>())
                {
                    if (Scene.Loaded.ECS.GetComponentFromEntity<MovementControllerComponent>(entityID).Controller is PlayerController playerController)
                    {
                        if (InputSystem.IsKeyDown(Keys.Up))
                        {
                            playerController.Up();
                        }

                        if (InputSystem.IsKeyDown(Keys.Down))
                        {
                            playerController.Down();
                        }

                        if (InputSystem.IsKeyDown(Keys.Left))
                        {
                            playerController.Left();
                        }

                        if (InputSystem.IsKeyDown(Keys.Right))
                        {
                            playerController.Right();
                        }

                        if (InputSystem.IsKeyDown(Keys.LeftShift))
                        {
                            playerController.Slow();
                        }
                    }
                }
            }
        }

        private static void AddCollisionVisibility()
        {
            foreach (int entityID in Scene.Loaded.ECS.GetEntitiesWithComponent<BehaviourComponent>())
            {
                if (Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(entityID).Behaviour is Player player)
                {
                    player.TurnOnSlowAbility();
                    isCollisionVisible = true;
                }
            }
        }

        private static void RemoveCollisionVisibility()
        {
            foreach (int entityID in Scene.Loaded.ECS.GetEntitiesWithComponent<BehaviourComponent>())
            {
                if (Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(entityID).Behaviour is Player player)
                {
                    player.TurnOffSlowAbility();
                    isCollisionVisible = false;
                }
            }
        }
        /* Player Conroller Movement Input End */


        /* PauseMenu Input */
        private static void Pause_UnPauseGame()
        {
            if (state != GamePlayInputState.UnPaused && state != GamePlayInputState.Paused) { return; }

            if (GameM.IsGamePaused == true && state == GamePlayInputState.Paused)
            {
                (currentMenuBehavior as MenuBehavior).HideMenu();
                SetToUnPaused();
            }
            else if (state == GamePlayInputState.UnPaused)
            {
                currentMenuBehavior = Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>((Scene.Loaded as GameplayScene).PauseMenuUIID).Behaviour;
                (currentMenuBehavior as MenuBehavior).ShowMenu();
                GameM.IsGamePaused = true;
                state = GamePlayInputState.Paused;
            }

        }
        /* PauseMenu Input End*/


        /* General Menu Input */
        private static void MoveSelectionUp()
        {
            if (state != GamePlayInputState.UnPaused)
            {
                (currentMenuBehavior as MenuBehavior).GoUpState();
            }
        }

        private static void MoveSelectionDown()
        {
            if (state != GamePlayInputState.UnPaused)
            {
                (currentMenuBehavior as MenuBehavior).GoDownState();
            }
        }

        private static void SelectionOption()
        {
            if (state != GamePlayInputState.UnPaused)
            {
                (currentMenuBehavior as MenuBehavior).SelectStateOption();
            }
        }
        /* General Menu Input End*/
    }
}
