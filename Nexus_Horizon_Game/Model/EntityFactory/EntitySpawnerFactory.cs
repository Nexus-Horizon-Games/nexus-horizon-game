using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.Prefab;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Nexus_Horizon_Game.Model.EntityFactory
{
    internal static class EntitySpawnerFactory
    {
        public static int CreateEntitySpawner(PrefabEntity prefab)
        {
            int spawnerID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(spawnerID, new BehaviourComponent(new EntitySpawnerBehaviour(spawnerID, prefab)));
            return spawnerID;
        }

        public static int CreateBulletSpawner(string textureName, float scale = 0.25f, uint spriteLayer = 0, Tag projectileTag = 0)
        {
            List<IComponent> components = new List<IComponent>
            { new TransformComponent(Vector2.Zero),
              new SpriteComponent(textureName, color: Color.White, scale: scale, spriteLayer: spriteLayer, centered: true),
              new TagComponent(projectileTag),
                          
            };

            int spawnerID = Scene.Loaded.ECS.CreateEntity();
            Scene.Loaded.ECS.AddComponent(spawnerID, new BehaviourComponent(new EntitySpawnerBehaviour(spawnerID, new PrefabEntity(components))));
            return spawnerID;
        }
    }
}
