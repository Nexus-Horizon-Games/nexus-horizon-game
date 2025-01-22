using Nexus_Horizon_Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexus_Horizon_Game
{
    internal class World
    {
        private Dictionary<Type, List<IComponent>> componentLists = new();
        private Queue<int> destroyedEntities = new();
        private int nextId = 0;

        /// <summary>
        /// creates a new entity with no components.
        /// </summary>
        /// <returns> new Entities ID. </returns>
        public int CreateEntity()
        {
            int newEntity;

            // use a destroyed entity if one is available
            // otherwise create a new entity and the components it may have
            if (destroyedEntities.Count != 0) { newEntity = destroyedEntities.Dequeue(); }
            else
            {
                newEntity = nextId;

                foreach (Type type in this.componentLists.Keys)
                {
                    List<IComponent> componentList;
                    this.componentLists.TryGetValue(type, out componentList);
                    componentList.Add(default);
                }

                nextId++;
            }


            return newEntity;
        }

        /// <summary>
        /// creates an entity with the components specified.
        /// </summary>
        /// <param name="components"> list of components wanted attached to entity. </param>
        /// <returns> entity ID. </returns>
        public int CreateEntity(List<IComponent> components)
        {
            int newEntity = this.CreateEntity();

            foreach (IComponent component in components)
            {
                if (!this.componentLists.TryGetValue(component.GetType(), out List<IComponent> componentList))
                {
                    List<IComponent> newComponentList = new List<IComponent>();
                    for (int i = 0; i < nextId; i++)
                    {
                        if (i == newEntity) { newComponentList.Add(component); }
                        else { newComponentList.Add(default); }
                    }

                    this.componentLists.Add(component.GetType(), newComponentList);

                    return newEntity;
                }

                componentList[newEntity] = component;
            }


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

            return newEntity;
        }

        /// <summary>
        /// removes all components from the entity and adds it to the destroyed entities queue.
        /// </summary>
        /// <param name="entity"> ID of entity wanting to be destroyed. </param>
        public void DestroyEntity(int entity)
        {
            // may need to allow each component to do some cleanup before enitity being destroyed.

            foreach (List<IComponent> component in componentLists.Values)
            {
                component[entity] = default;
            }
            destroyedEntities.Enqueue(entity);
        }

        public void AddComponent<T>(int entity, T component) where T : IComponent
        {
            if (!componentLists.ContainsKey(typeof(T)))
            {
                componentLists.Add(component.GetType(), new List<IComponent>());
            }

            List<IComponent> componentList = componentLists[typeof(T)];
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

        /// <summary>
        /// removes the component only if it exists otherwise does nothing
        /// </summary>
        /// <typeparam name="T"> the inherited version of the IComponent. </typeparam>
        /// <param name="entity"> the Id of the entity. </param>
        public void RemoveComponent<T>(int entity) where T : IComponent
        {
            if (!componentLists.TryGetValue(typeof(T), out List<IComponent> componentList)) { return; }
            componentList[entity] = default;
        }

        /// <summary>
        /// creates a enumeration query of the type of components specified.
        /// </summary>
        /// <typeparam name="T"> type of component. </typeparam>
        /// <returns> query of components of the type. </returns>
        public IEnumerable<T> GetComponents<T>() where T : IComponent
        {
            componentLists.TryGetValue(typeof(T), out var componentList);
            return componentList.Cast<T>();
        }

        public List<int> GetEntitiesWithComponent<T>() where T : IComponent
        {
            componentLists.TryGetValue(typeof(T), out List<IComponent> componentList);
            var entities = new List<int>();
            for (int i = 0; i < componentList.Count; i++)
            {
                if (componentList[i] != default)
                {
                    if (!componentList[i].Equals(default(T)))
                    {
                        entities.Add(i);
                    }
                }
            }

            return entities;
        }

        /// <summary>
        /// check whether the entity has the component specified.
        /// </summary>
        /// <typeparam name="T"> component being checked on. </typeparam>
        /// <param name="entity"> the id of the entity wanting to check. </param>
        /// <returns> true when entity has component otherwise false. </returns>
        public bool HasComponent<T>(int entity) where T : IComponent
        {
            if (!componentLists.TryGetValue(typeof(T), out List<IComponent> componentList)) { return false; } // does not have component since it does not exist in dictionary
            return componentList[entity] != default;
        }
    }
}
