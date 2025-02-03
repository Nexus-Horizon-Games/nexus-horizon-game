using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using System;
using System.Diagnostics;

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
            int hitboxVisualEntityID = GameM.CurrentScene.World.CreateEntity(new List<IComponent>
            {
               new TransformComponent(new Vector2(100.0f, 100.0f)),
               new SpriteComponent("PlayerDot", centered: true, scale: 0.01f, spriteLayer: 100, isVisible: false)
            });

            int playerEntityID = GameM.CurrentScene.World.CreateEntity(new List<IComponent>
            { 
              new TransformComponent(new Vector2(100.0f, 100.0f)),
              new SpriteComponent("guinea_pig", centered: true, scale: 1f),
              new PhysicsBody2DComponent(),
              new PlayerComponent(hitboxVisualEntityID),
              new TagComponent(Tag.PLAYER) 
            });

            return playerEntityID;
        }

        public override void DestroyEntity(int entity)
        {
            if (GameM.CurrentScene.World.EntityHasComponent<PlayerComponent>(entity))
            {
                GameM.CurrentScene.World.DestroyEntity(GameM.CurrentScene.World.GetComponentFromEntity<PlayerComponent>(entity).HitboxVisualEntityID);
                GameM.CurrentScene.World.DestroyEntity(entity);
                return;
            }

            throw new ArgumentException("Cannot use PlayerFactory to destroy non player entity");
        }
    }
}
