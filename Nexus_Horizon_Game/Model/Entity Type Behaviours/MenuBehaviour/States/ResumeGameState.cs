using Nexus_Horizon_Game.View.InputSystem;

namespace Nexus_Horizon_Game.Model.Entity_Type_Behaviours.MenuBehaviour.States
{
    internal class ResumeGameState : SpriteFontSelectionState
    {
        public ResumeGameState(string entityNameToParent) : base(entityNameToParent) { }
        public override void SelectAction(MenuBehavior menuBehavior)
        {
            menuBehavior.HideMenu();
            GamePlayInput.SetToUnPaused();
        }
    }
}
