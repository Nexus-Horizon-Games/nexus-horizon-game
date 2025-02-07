
using Nexus_Horizon_Game.EntityFactory;

namespace Nexus_Horizon_Game
{
    internal static class SceneLoader
    {
        public static void LoadScene(ref Scene currentScene)
        {
            currentScene = new Scene();
            // TODO: parse the scene from JSON here

            var playerFactory = new PlayerFactory();
            int moveablePlayer2 = playerFactory.CreateEntity();

            EnemyFactory.CreateBoss("evil_guinea_pig_boss");
            EnemyFactory.CreateBoss("chef_boss");
        }
    }
}
