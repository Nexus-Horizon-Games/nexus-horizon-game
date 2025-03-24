using Nexus_Horizon_Game.Model.Scenes;
using Nexus_Horizon_Game.View.InputSystem;

namespace Nexus_Horizon_Game.Model.Entity_Type_Behaviours.MenuBehaviour.States
{
    internal class MainMenuState : SpriteFontSelectionState
    {
        public MainMenuState(string entityNameToParent) : base(entityNameToParent) { }
        public override void SelectAction(MenuBehavior menuBehavior)
        {
            GameM.IsGamePaused = false;
            Scene.Loaded = new MenuScene();
            InputSystem.SetInputSystem(new MenuInput());
        }
    }
}
