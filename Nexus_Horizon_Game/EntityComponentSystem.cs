using Nexus_Horizon_Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game
{
    internal class EntityComponentSystem
    {
        public int CreateEntity()
        {
            return 0;
        }

        public int CreateEntity(List<IComponent> components)
        {
            return 0;
        }

        public void DestroyEntity(int entity)
        {
        }

        public void AddComponent<T>(int entity, T component)
        {
        }

        public void RemoveComponent<T>(int entity)
        {
        }

        public List<T> GetComponents<T>()
        {
            return new List<T> { };
        }

        public List<int> GetEntitiesWithComponent<T>()
        {
            return new List<int> { };
        }

        public bool HasComponent<T>(int entity)
        {
            return false;
        }
    }
}
