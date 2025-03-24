using Nexus_Horizon_Game.Controller;
using Nexus_Horizon_Game.Model.Scenes;
using Nexus_Horizon_Game.View.InputSystem;

namespace Nexus_Horizon_Game.Model.Entity_Type_Behaviours.MenuBehaviour.States
{
    internal class PlayGameState : SpriteFontSelectionState
    {
        public PlayGameState(string entityNameToParent) : base(entityNameToParent) { }
        public override void SelectAction(MenuBehavior menuBehavior)
        {
            GameM.IsGamePaused = false;
            Scene.Loaded = new GameplayScene();
            InputSystem.SetInputSystem(new GamePlayInput());

            // calls collision system
            CollisionSystem.Init();
        }
    }
}
