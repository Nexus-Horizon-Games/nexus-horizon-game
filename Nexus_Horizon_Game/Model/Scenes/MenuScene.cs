using System.Collections.Generic;
using Nexus_Horizon_Game.Model.EntityFactory;

namespace Nexus_Horizon_Game.Model.Scenes
{
    /// <summary>
    /// Menu Screen Scene
    /// </summary>
    internal class MenuScene : Scene
    {
        private static int menuID;

        public MenuScene() : base() { }

        public static int MenuID
        {
            get => menuID;
        }

        protected override void Initialize()
        {
            return;
        }

        protected override void LoadContent()
        {
            Renderer.LoadContent(new List<string> { });
        }

        protected override void LoadScene()
        {
            menuID = MenuPrefab.CreateMainMenu();
        }
    }
}
