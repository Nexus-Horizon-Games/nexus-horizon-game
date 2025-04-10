using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nexus_Horizon_Game.Entity_Type_Behaviours.Bullet;

namespace Nexus_Horizon_Game.Model.EntityFactory
{
    internal static class DropFactory
    {
        private static readonly Vector2 dropVelocity = new Vector2(0, 4);

        public static Vector2 DropVelocity
        {
            get => dropVelocity;
        }

        public static int CreateDrop(string spriteName, Tag dropTag, uint spriteLayer)
        {
            float scale = 0.40f;

            Scene.Loaded.ECS.CreateEntity();

            float originalWidth = Renderer.GetTextureWidth(spriteName);
            float originalHeight = Renderer.GetTextureHeight(spriteName);

            int scaledWidth = (int)(originalWidth * scale);
            int scaledHeight = (int)(originalHeight * scale);

            int entity = Scene.Loaded.ECS.CreateEntity(new List<IComponent>
            { new TransformComponent(new Vector2(0,0)),
              new SpriteComponent(spriteName, color: Color.White, scale: scale, spriteLayer: spriteLayer, centered: true),
              new PhysicsBody2DComponent()
              {
                  Velocity = Vector2.Zero,
              },

                new TagComponent(dropTag)

            });

            Scene.Loaded.ECS.AddComponent<ColliderComponent>(entity, new ColliderComponent(new Point(scaledWidth, scaledHeight), entityIDFollowing: entity));

            return entity;
        }

        public static void SpawnDrops(int Count, Vector2 SpawnPosition, Tag tagType, string spriteName)
        {
            int entityTimer = Scene.Loaded.ECS.CreateEntity(new List<Nexus_Horizon_Game.Components.IComponent> { new TimersComponent() });
            Scene.Loaded.ECS.EntityHasComponent<TimersComponent>(entityTimer, out TimersComponent timerComp);

            for (int i = 0; i < Count; i++)
            {
                int entityID = DropFactory.CreateDrop(spriteName, tagType, spriteLayer: 101);
                Scene.Loaded.ECS.EntityHasComponent<TransformComponent>(entityID, out TransformComponent transformComp);

                transformComp.position = SpawnPosition;

                Scene.Loaded.ECS.SetComponentInEntity(entityID, transformComp);

                SendDropRandomDirectionThenFallDown(entityID);
                float timeBeforeFalling = RandomGenerator.GetFloat(0, 1.5f);
                timerComp.timers.Add($"entity{i}", new DelayTimer(timeBeforeFalling, (gameTime, data) =>
                {
                    Scene.Loaded.ECS.EntityHasComponent<PhysicsBody2DComponent>(entityID, out PhysicsBody2DComponent physBodyComp);

                    physBodyComp.Velocity = DropFactory.DropVelocity;

                    Scene.Loaded.ECS.SetComponentInEntity(entityID, physBodyComp);
                }));
                timerComp.timers[$"entity{i}"].Start();
                Scene.Loaded.ECS.SetComponentInEntity(entityTimer, timerComp);
            }
        }

        private static void SendDropRandomDirectionThenFallDown(int entity)
        {
            float randomXDirection = RandomGenerator.GetFloat(-1f, 1f); // X Direction
            float randomYDirection = RandomGenerator.GetFloat(-1f, 1f); // Y Direction
            float scaler = RandomGenerator.GetFloat(0f, 5f); // Scaler

            Scene.Loaded.ECS.EntityHasComponent<PhysicsBody2DComponent>(entity, out PhysicsBody2DComponent physBodyComp);

            physBodyComp.Velocity = (new Vector2(randomXDirection, randomYDirection)) * scaler;

            Scene.Loaded.ECS.SetComponentInEntity(entity, physBodyComp);

        }

    }
}
