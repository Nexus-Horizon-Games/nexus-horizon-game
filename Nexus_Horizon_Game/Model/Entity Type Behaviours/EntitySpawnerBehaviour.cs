﻿using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.EntityPatterns;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Timers;

namespace Nexus_Horizon_Game.Model.Entity_Type_Behaviours
{
    internal class EntitySpawnerBehaviour : Behaviour
    {
        private PrefabEntity prefab;

        public EntitySpawnerBehaviour(PrefabEntity prefab)
        {
            this.prefab = prefab;
        }

        public EntitySpawnerBehaviour(int thisEntity, PrefabEntity prefab) : base(thisEntity)
        {
            this.prefab = prefab;
        }

        public void SpawnEntity()
        {
            var clonedPrefab = prefab.Clone();

            // spawn at spawner position
            var components = clonedPrefab.Components;
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is TransformComponent transform)
                {
                    transform.position = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position; // Set the spawn position
                }
            }

            Scene.Loaded.ECS.CreateEntity(clonedPrefab);
        }

        public void SpawnEntitiesWithPattern(IFiringPattern pattern, GameTime gameTime, TimerContainer timerContainer)
        {
            var clonedPrefab = prefab.Clone();
            var components = clonedPrefab.Components;
            components.RemoveAll(x => x.GetType() == typeof(TransformComponent));
            Vector2 position = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity).position;
            components.Add(new TransformComponent(position, 0));

            pattern.Fire(clonedPrefab.Clone(), gameTime, timerContainer);
        }
    }
}
