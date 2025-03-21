namespace Nexus_Horizon_Game.Model.Entity_Type_Behaviours.MenuBehaviour.States
{
    internal class ExitGameState : SpriteFontSelectionState
    {
        public ExitGameState(string entityNameToParent) : base(entityNameToParent) { }

        public override void SelectAction(MenuBehavior menuBehavior)
        {
            GameM.ExitGame = true;
        }
    }
}
