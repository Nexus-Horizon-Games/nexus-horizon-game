using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model;
using Microsoft.Xna.Framework.Input;
using Nexus_Horizon_Game.Entity_Type_Behaviours;

namespace Nexus_Horizon_Game.View.InputSystem
{
    internal class GamePlayInput : InputSystem
    {
        protected override void LoadInput()
        {
            // Visibility For Collision Box
            InputSystem.AddOnKeyDownListener(Keys.LeftShift, AddCollisionVisibility);
            InputSystem.AddOnKeyUpListener(Keys.LeftShift, RemoveCollisionVisibility);

            InputSystem.AddOnKeyDownListener(Keys.Escape, Pause_UnPauseGame);

            // Movement
            InputSystem.OnUpdate += MovementCheck;
        }

        private static void MovementCheck()
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

        private static void AddCollisionVisibility()
        {
            foreach (int entityID in Scene.Loaded.ECS.GetEntitiesWithComponent<BehaviourComponent>())
            {
                if (Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(entityID).Behaviour is Player player)
                {
                    player.TurnOnSlowAbility();
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
                }
            }
        }

        private static void Pause_UnPauseGame()
        {
            if (GameM.IsGamePaused == true)
            {
                GameM.IsGamePaused = false;
            }
            else
            {
                GameM.IsGamePaused = true;
            }
        }
    }
}
