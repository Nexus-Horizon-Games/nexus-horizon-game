using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using System;
using Nexus_Horizon_Game.Model;
using Nexus_Horizon_Game.Model.GameManagers;
using Nexus_Horizon_Game.Timers;
using Nexus_Horizon_Game.Model.EntityFactory;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal class PlayerFactory : EntityFactory
    {
        public PlayerFactory()
        {
        }

        /// <summary>
        /// creates a player.
        /// </summary>
        /// <returns> entity ID. </returns>
        public override int CreateEntity()
        {

            int hitboxEntityID = Scene.Loaded.ECS.CreateEntity(new List<IComponent>
            {
               new TransformComponent(new Vector2(100.0f, 100.0f)),
               new SpriteComponent("PlayerDot", centered: true, scale: 0.01f, spriteLayer: 101, isVisible: false),
               // hitbox for player (decides where to center it, decides size)
               // tags hitbox to player again just incase
               new TagComponent(Tag.PLAYER)
            });

            int playerEntityID = Scene.Loaded.ECS.CreateEntity(new List<IComponent>
            {
              new TransformComponent(new Vector2(Arena.Origin.X, Arena.Origin.Y + 65)),
              new SpriteComponent("guinea_pig", centered: true, scale: 1f, spriteLayer: 100),
              new PhysicsBody2DComponent(),
              new TagComponent(Tag.PLAYER)
            });
            Scene.Loaded.ECS.AddComponent(playerEntityID, new MovementControllerComponent(new PlayerController(new Movement(13f)), playerEntityID));
            Scene.Loaded.ECS.AddComponent(playerEntityID, new BehaviourComponent(new Player(playerEntityID, hitboxEntityID)));

            int timerEntityID = Scene.Loaded.ECS.CreateEntity(new List<IComponent> { new TimersComponent(new Dictionary<string, Timer>()) });
            Scene.Loaded.ECS.EntityHasComponent(timerEntityID, out TimersComponent playerTimerComponent);
            playerTimerComponent.timers.Add("DeathCooldown", new DelayTimer(1.75f, (gameTime, data) =>
            {
                //create collider where its connected to playerID
                Scene.Loaded.ECS.AddComponent<ColliderComponent>(hitboxEntityID, new ColliderComponent(new Point(2, 1), new Point(0, 0)));

                // Death Script Setup
                Scene.Loaded.ECS.EntityHasComponent<ColliderComponent>(hitboxEntityID, out ColliderComponent collComp);
                collComp.OnCollision += (entity) => {
                    if (Scene.Loaded.ECS.EntityHasComponent<TagComponent>(entity, out TagComponent tagComponent))
                    {
                        if ((tagComponent.Tag & Tag.ENEMY_PROJECTILE) == Tag.ENEMY_PROJECTILE)
                        {
                            DropFactory.SpawnDrops(RandomGenerator.GetInteger(0, 10), Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(playerEntityID).position, Tag.POWERDROP, "PowerCarrot");
                            DropFactory.SpawnDrops(RandomGenerator.GetInteger(0, 5), Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(playerEntityID).position, Tag.POINTDROP, "PointCarrot");
                            GameplayManager.Instance.PlayerDied();
                            DestroyEntity(playerEntityID);
                            var playerFactory = new PlayerFactory();
                            int moveablePlayer2 = playerFactory.CreateEntity();
                        }
                        else if ((tagComponent.Tag & Tag.POWERDROP) == Tag.POWERDROP)
                        { 
                            GameplayManager.Instance.PickedUpAPower();
                            Scene.Loaded.ECS.DestroyEntity(entity);

                        }
                        else if ((tagComponent.Tag & Tag.POINTDROP) == Tag.POINTDROP)
                        {
                            GameplayManager.Instance.PickedUpAPoint();
                            Scene.Loaded.ECS.DestroyEntity(entity);
                        }
                    }
                };
                Scene.Loaded.ECS.SetComponentInEntity<ColliderComponent>(hitboxEntityID, collComp);
            }));
            playerTimerComponent.timers["DeathCooldown"].Start();
            Scene.Loaded.ECS.SetComponentInEntity(timerEntityID, playerTimerComponent);
            
            return playerEntityID;
        }

        public override void DestroyEntity(int entity)
        {
            if (Scene.Loaded.ECS.EntityHasComponent<BehaviourComponent>(entity, out BehaviourComponent behaviourComponent))
            {
                if (behaviourComponent.Behaviour is Player player)
                {
                    Scene.Loaded.ECS.DestroyEntity(player.HitboxEntityID);
                    Scene.Loaded.ECS.DestroyEntity(entity);

                    return;
                }
            }

            throw new ArgumentException("Cannot use PlayerFactory to destroy non player entity");
        }
    }
}
