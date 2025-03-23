﻿using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using System;
using Nexus_Horizon_Game.Model;

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
               new SpriteComponent("PlayerDot", centered: true, scale: 0.01f, spriteLayer: 101, isVisible: false)
            });


            int playerEntityID = Scene.Loaded.ECS.CreateEntity(new List<IComponent>
            {
              new TransformComponent(new Vector2(100.0f, 100.0f)),
              new SpriteComponent("guinea_pig", centered: true, scale: 1f, spriteLayer: 100),
              new PhysicsBody2DComponent(),
              new TagComponent(Tag.PLAYER)
            });

            Scene.Loaded.ECS.AddComponent(playerEntityID, new MovementControllerComponent(new PlayerController(new Movement(13f)), playerEntityID));

            Scene.Loaded.ECS.AddComponent(playerEntityID, new BehaviourComponent(new Player(playerEntityID, hitboxEntityID)));

            return playerEntityID;
        }

        public override void DestroyEntity(int entity)
        {
            if (Scene.Loaded.ECS.EntityHasComponent<BehaviourComponent>(entity, out BehaviourComponent behaviourComponent))
            {
                if (behaviourComponent.Behaviour is Player player)
                {
                    Scene.Loaded.ECS.DestroyEntity(player.HitBoxEntityID);
                    Scene.Loaded.ECS.DestroyEntity(entity);

                    return;
                }
            }

            throw new ArgumentException("Cannot use PlayerFactory to destroy non player entity");
        }
    }
}
