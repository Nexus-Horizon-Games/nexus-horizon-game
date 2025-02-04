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
            int hitboxEntityID = GameM.CurrentScene.World.CreateEntity(new List<IComponent>
            {
               new TransformComponent(new Vector2(100.0f, 100.0f)),
               new SpriteComponent("PlayerDot", centered: true, scale: 0.01f, spriteLayer: 101, isVisible: false)
            });

            int playerEntityID = GameM.CurrentScene.World.CreateEntity(new List<IComponent>
            { 
              new TransformComponent(new Vector2(100.0f, 100.0f)),
              new SpriteComponent("guinea_pig", centered: true, scale: 1f, spriteLayer: 100),
              new PhysicsBody2DComponent(),
              new TagComponent(Tag.PLAYER) 
            });

            GameM.CurrentScene.World.AddComponent(playerEntityID, new BehaviourComponent(new Player(playerEntityID, hitboxEntityID)));

            return playerEntityID;
        }

        public override void DestroyEntity(int entity)
        {
            if (GameM.CurrentScene.World.EntityHasComponent<BehaviourComponent>(entity, out BehaviourComponent behaviourComponent)) 
            {
                if (behaviourComponent.Behaviour is Player player)
                {
                    GameM.CurrentScene.World.DestroyEntity(player.HitBoxEntityID);
                    GameM.CurrentScene.World.DestroyEntity(entity);

                    return;
                }   
            }

            throw new ArgumentException("Cannot use PlayerFactory to destroy non player entity");
        }
    }
}
