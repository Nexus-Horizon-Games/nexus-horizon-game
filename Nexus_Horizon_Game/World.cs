using Nexus_Horizon_Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game
{
    internal class World
    {
        private Dictionary<Type, object> componentLists = new();
        private int nextId = 0;

        public int CreateEntity()
        {
            int newEntity = nextId;
            nextId++;
            return newEntity;
        }

        public int CreateEntity(List<IComponent> components)
        {
            // To hard for me to implement
            /*int newEntity = nextId;
            nextId++;

            foreach (IComponent component in components)
            {
                if (!componentLists.ContainsKey(component.GetType()))
                {
                    var type = component.GetType();
                    var listType = typeof(List<>).MakeGenericType(type);
                    componentLists.Add(component.GetType(), Activator.CreateInstance(listType));
                }

                var componentList = componentLists[component.GetType()] as IList;
                if (newEntity < componentList.Count)
                {
                    componentList[newEntity] = component;
                }
                else
                {
                    componentList.Add(component);
                }
            }*/

            return -1;
        }

        public void DestroyEntity(int entity)
        {
        }

        public void AddComponent<T>(int entity, T component) where T : IComponent
        {
            if (!componentLists.ContainsKey(typeof(T)))
            {
                componentLists.Add(component.GetType(), new List<T>());
            }

            var componentList = componentLists[typeof(T)] as List<T>;
            if (entity < componentList.Count)
            {
                componentList[entity] = component;
            }
            else
            {
                // add blank components in any gaps
                while (entity > componentList.Count)
                {
                    componentList.Add(default);
                }

                componentList.Add(component);
            }
        }

        public void RemoveComponent<T>(int entity) where T : IComponent
        {
        }

        public List<T> GetComponents<T>() where T : IComponent
        {
            componentLists.TryGetValue(typeof(T), out var componentList);
            var list = componentList as List<T>;
            return list;
        }

        public List<int> GetEntitiesWithComponent<T>() where T : IComponent
        {
            componentLists.TryGetValue(typeof(T), out var componentList);
            var list = componentList as List<T>;
            var entities = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].Equals(default(T)))
                {
                    entities.Add(i);
                }
            }

            return entities;
        }

        public bool HasComponent<T>(int entity) where T : IComponent
        {
            return false;
        }
    }
}
