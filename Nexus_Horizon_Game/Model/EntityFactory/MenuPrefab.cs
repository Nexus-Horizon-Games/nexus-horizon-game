using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.Components;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours.MenuBehaviour.States;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours.MenuBehaviour;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Model.EntityFactory
{
    internal static class MenuPrefab
    {

        ///
        /// returns the entityID of the main menu.
        ///
        public static int CreateMainMenu()
        {
            int backgroundID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(backgroundID, new TransformComponent(new Vector2(0, 0)));
            Scene.Loaded.ECS.AddComponent(backgroundID, new SpriteComponent("MenuScreen", spriteLayer: int.MaxValue - 1, isUI: true));
            Scene.Loaded.ECS.AddComponent(backgroundID, new ChildrenComponent());

            List<SpriteFontSelectionState> spriteFontStates = new()
            {
                new PlayGameState("PlayGame"),
                new ExitGameState("Exit")
            };

            Scene.Loaded.ECS.AddComponent<BehaviourComponent>(backgroundID, new BehaviourComponent(new MenuBehavior(backgroundID, new Vector2(500, 300), new Vector2(20, 50), spriteFontStates)));

            return backgroundID;
        }

        ///
        /// returns the entityID of the main menu.
        ///
        public static int CreatePauseMenu(uint spriteLayer)
        {
            int backgroundID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(backgroundID, new TransformComponent(new Vector2(0, 0)));
            Scene.Loaded.ECS.AddComponent(backgroundID, new SpriteComponent("InGameMenu", spriteLayer: spriteLayer, isUI: true));
            Scene.Loaded.ECS.AddComponent(backgroundID, new ChildrenComponent());

            int fontTitleID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(fontTitleID, new TransformComponent(new Vector2(Renderer.ScreenWidth / 2, Renderer.ScreenHeight / 2 - 100)));
            Scene.Loaded.ECS.EntityHasComponent<SpriteComponent>(backgroundID, out SpriteComponent component);
            Scene.Loaded.ECS.AddComponent(fontTitleID, new SpriteFontComponent("NineteenNinetySeven", "Paused", spriteLayer: component.spriteLayer + 1, scale: 2f, centered: true));
            Scene.Loaded.ECS.GetComponentFromEntity<ChildrenComponent>(backgroundID).AddChild("Paused", fontTitleID);

            List<SpriteFontSelectionState> spriteFontStates = new()
            {
                new ResumeGameState("Resume"),
                new MainMenuState("Main Menu")
            };

            Scene.Loaded.ECS.AddComponent<BehaviourComponent>(backgroundID, new BehaviourComponent(new MenuBehavior(backgroundID, new Vector2(Renderer.ScreenWidth / 2, Renderer.ScreenHeight / 2), new Vector2(0, 50), spriteFontStates)));
            (Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(backgroundID).Behaviour as MenuBehavior)?.HideMenu();

            return backgroundID;
        }

        ///
        /// returns the entityID of the main menu.
        ///
        public static int CreateDeathMenu(uint spriteLayer)
        {
            int backgroundID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(backgroundID, new TransformComponent(new Vector2(0, 0)));
            Scene.Loaded.ECS.AddComponent(backgroundID, new SpriteComponent("InGameMenu", spriteLayer: spriteLayer, isUI: true));
            Scene.Loaded.ECS.AddComponent(backgroundID, new ChildrenComponent());

            int fontTitleID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(fontTitleID, new TransformComponent(new Vector2(Renderer.ScreenWidth / 2, Renderer.ScreenHeight / 2 - 100)));
            Scene.Loaded.ECS.EntityHasComponent<SpriteComponent>(backgroundID, out SpriteComponent component);
            Scene.Loaded.ECS.AddComponent(fontTitleID, new SpriteFontComponent("NineteenNinetySeven", "YOU DIED", spriteLayer: component.spriteLayer + 1, scale: 2f, centered: true));
            Scene.Loaded.ECS.GetComponentFromEntity<ChildrenComponent>(backgroundID).AddChild("YOU DIED", fontTitleID);

            List<SpriteFontSelectionState> spriteFontStates = new()
            {
                new PlayGameState("Restart"),
                new MainMenuState("Main Menu")
            };

            Scene.Loaded.ECS.AddComponent<BehaviourComponent>(backgroundID, new BehaviourComponent(new MenuBehavior(backgroundID, new Vector2(Renderer.ScreenWidth / 2, Renderer.ScreenHeight / 2), new Vector2(0, 50), spriteFontStates)));
            (Scene.Loaded.ECS.GetComponentFromEntity<BehaviourComponent>(backgroundID).Behaviour as MenuBehavior)?.HideMenu();

            return backgroundID;
        }
    }
}
