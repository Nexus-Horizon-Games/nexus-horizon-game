using Nexus_Horizon_Game.Model.Components;
using System;
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Model.Entity_Type_Behaviours.MenuBehaviour
{
    internal abstract class SpriteFontSelectionState
    {
        private string entityName;

        public SpriteFontSelectionState(string entityNameToParent)
        {
            this.entityName = entityNameToParent;
        }

        public string EntityName
        {
            get => entityName;
        }

        public void HighlightFont(int parentEntityID)
        {
            int? fontEntityID = Scene.Loaded.ECS.GetComponentFromEntity<ChildrenComponent>(parentEntityID).GetChildIDByName(entityName);
            if (fontEntityID is not null)
            {
                if (Scene.Loaded.ECS.EntityHasComponent<SpriteFontComponent>((int)fontEntityID, out SpriteFontComponent spriteFont))
                {
                    spriteFont.Color = Color.Red;
                    Scene.Loaded.ECS.SetComponentInEntity<SpriteFontComponent>((int)fontEntityID, spriteFont);
                }
                else
                {
                    throw new Exception("Entity does not have a SpriteFontComponent For Main Menu");
                }
            }
        }

        public void UnHighlightFont(int parentEntityID)
        {
            int? fontEntityID = Scene.Loaded.ECS.GetComponentFromEntity<ChildrenComponent>(parentEntityID).GetChildIDByName(entityName);
            if (fontEntityID is not null)
            {
                if (Scene.Loaded.ECS.EntityHasComponent<SpriteFontComponent>((int)fontEntityID, out SpriteFontComponent spriteFont))
                {
                    spriteFont.Color = Color.White;
                    Scene.Loaded.ECS.SetComponentInEntity<SpriteFontComponent>((int)fontEntityID, spriteFont);
                }
                else
                {
                    throw new Exception("Entity does not have a SpriteFontComponent For Main Menu");
                }
            }
        }

        public abstract void SelectAction(MenuBehavior menuehavior);
    }
}
