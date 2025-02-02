using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Entity_Type_Behaviours;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal static class EnemyFactory
    {
        public static int CreateEnemy(World world, string type)
        {
            // create enemy
            return -1;
        }

        public static int CreateBoss(World world, string type)
        {
            int bossEntity = world.CreateEntity(new List<IComponent>
            {
                new TransformComponent(new Vector2(0.0f, 0.0f)),
                new PhysicsBody2DComponent(),
            });
            
            if (type == "evil_guinea_pig_boss") // mid boss
            {

            }
            else if (type == "chef_boss") // final boss
            {
                world.AddComponent(bossEntity, new SpriteComponent("chef_boss", centered: true));
                world.AddComponent(bossEntity, new OnUpdateComponent(ChefBossBehaviour.OnUpdate));
                world.AddComponent(bossEntity, new StateComponent(ChefBossBehaviour.ChefBossState.Start));
            }

            return bossEntity;
        }
    }
}
