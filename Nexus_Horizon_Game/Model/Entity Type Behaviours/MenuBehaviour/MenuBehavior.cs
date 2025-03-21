using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours.MenuBehaviour;
using Nexus_Horizon_Game.Components;

namespace Nexus_Horizon_Game.Model.Entity_Type_Behaviours
{
    internal class MenuBehavior : Behaviour
    {
        enum MainMenuState
        {
            PlayGame = 0,
            Exit = 1
        }

        private MainMenuState state;
        private List<SpriteFontSelectionState> spriteFontStates;
        private int currentSelectedIndex = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"> entity ID of containing Behavior</param>
        /// <param name="spriteFontStates"> list of sprite fonts added in order of creation. </param>
        public MenuBehavior(int entityID, Vector2 positionOfFontsTopLeft, Vector2 TextOffsetPlacement, List<SpriteFontSelectionState> spriteFontStates) : base(entityID)
        {
            this.spriteFontStates = spriteFontStates;

            for (int i = 0; i < this.spriteFontStates.Count; i++)
            {
                int fontEntityID = Scene.Loaded.ECS.CreateEntity();
                Scene.Loaded.ECS.AddComponent(fontEntityID, new TransformComponent(new Vector2(positionOfFontsTopLeft.X + (TextOffsetPlacement.X * i), positionOfFontsTopLeft.Y + (TextOffsetPlacement.Y * i))));
                Scene.Loaded.ECS.EntityHasComponent<SpriteComponent>(entityID, out SpriteComponent component);
                Scene.Loaded.ECS.AddComponent(fontEntityID, new SpriteFontComponent("NineteenNinetySeven", spriteFontStates[i].EntityName, spriteLayer: component.spriteLayer + 1, centered: true));
                Scene.Loaded.ECS.GetComponentFromEntity<ChildrenComponent>(entityID).AddChild(spriteFontStates[i].EntityName, fontEntityID);
            }

            // when creating here create the order of states and position and switch selection based of that keeping order the same everytime menu behavior is worked in.
            currentSelectedIndex = 0;
            spriteFontStates[currentSelectedIndex].HighlightFont(entityID);
        }


        private void UpdateUI(int previousIndexSelection)
        {
            spriteFontStates[previousIndexSelection].UnHighlightFont(this.Entity);
            spriteFontStates[currentSelectedIndex].HighlightFont(this.Entity);
        }

        public void GoUpState()
        {
            // at the playgame state cant go up anymore
            if (currentSelectedIndex == 0)
            {
                return;
            }
            UpdateUI(currentSelectedIndex--);
        }

        public void GoDownState()
        {
            // at the exit state cant go down anymore
            if (currentSelectedIndex == this.spriteFontStates.Count - 1)
            {
                return;
            }
            UpdateUI(currentSelectedIndex++);
        }

        public void SelectStateOption()
        {
            spriteFontStates[currentSelectedIndex].SelectAction(this);
        }

        public void ShowMenu()
        {
            SpriteComponent spriteComponent = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(this.Entity);
            spriteComponent.isVisible = true;
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(this.Entity, spriteComponent);

            foreach (int childID in Scene.Loaded.ECS.GetComponentFromEntity<ChildrenComponent>(this.Entity).GetChildren())
            {
                SpriteFontComponent spriteComponentChild = Scene.Loaded.ECS.GetComponentFromEntity<SpriteFontComponent>(childID);
                spriteComponentChild.isVisible = true;
                Scene.Loaded.ECS.SetComponentInEntity<SpriteFontComponent>(childID, spriteComponentChild);
            }
        }

        public void HideMenu()
        {
            SpriteComponent spriteComponent = Scene.Loaded.ECS.GetComponentFromEntity<SpriteComponent>(this.Entity);
            spriteComponent.isVisible = false;
            Scene.Loaded.ECS.SetComponentInEntity<SpriteComponent>(this.Entity, spriteComponent);

            foreach (int childID in Scene.Loaded.ECS.GetComponentFromEntity<ChildrenComponent>(this.Entity).GetChildren())
            {
                SpriteFontComponent spriteComponentChild = Scene.Loaded.ECS.GetComponentFromEntity<SpriteFontComponent>(childID);
                spriteComponentChild.isVisible = false;
                Scene.Loaded.ECS.SetComponentInEntity<SpriteFontComponent>(childID, spriteComponentChild);
            }
        }
    }
}
