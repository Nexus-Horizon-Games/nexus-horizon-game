using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Entity_Type_Behaviours;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal static class EnemyFactory
    {
        public static int CreateEnemy(string type)
        {
            // create enemy
            return -1;
        }

        public static int CreateBoss(string type)
        {
            int bossEntity = GameM.CurrentScene.World.CreateEntity(new List<IComponent>
            {
                new TransformComponent(new Vector2(0.0f, 0.0f)),
                new PhysicsBody2DComponent(accelerationEnabled: true),
            });

            if (type == "evil_guinea_pig_boss") // mid boss
            {

            }
            else if (type == "chef_boss") // final boss
            {
                GameM.CurrentScene.World.AddComponent(bossEntity, new SpriteComponent("chef_boss", centered: true));
                GameM.CurrentScene.World.AddComponent(bossEntity, new BehaviourComponent(new ChefBossBehaviour(bossEntity)));
                GameM.CurrentScene.World.AddComponent(bossEntity, new StateComponent(ChefBossBehaviour.ChefBossState.Start));
            }

            return bossEntity;
        }
    }
}
