using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.Prefab;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nexus_Horizon_Game
{
    internal class ECS
    {
        private Dictionary<Type, List<IComponent>> componentLists = new();
        private PriorityQueue<int, int> destroyedEntities = new();
        private int nextId = 0;

        public delegate void OnAddComponent(int entity, Type componentType);

        public event OnAddComponent OnAddComponentEvent;

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
                var makeEmptyComponent = component.GetType().GetMethod("MakeEmptyComponent");

                if (!this.componentLists.TryGetValue(component.GetType(), out List<IComponent> componentList)) // if component list does not exist
                {
                    List<IComponent> newComponentList = new List<IComponent>();

                    // add blank components all the way to entity.
                    while (newEntity >= newComponentList.Count)
                    {
                        newComponentList.Add(makeEmptyComponent?.Invoke(null, null) as IComponent);
                    }

                    newComponentList[newEntity] = component;

                    this.componentLists.Add(component.GetType(), newComponentList);
                }
                else // component list does exist
                {
                    // add blank components in any gaps
                    while (newEntity >= componentList.Count)
                    {
                        componentList.Add(makeEmptyComponent?.Invoke(null, null) as IComponent);
                    }

                    componentList[newEntity] = component;
                }

                OnAddComponentEvent?.Invoke(newEntity, component.GetType());
            }

            return newEntity;
        }

        /// <summary>
        /// Creates a new entity with the given prototype.
        /// </summary>
        /// <returns> The id of the newly created entity. </returns>
        public int CreateEntity(PrefabEntity entityPrefab)
        {
            return CreateEntity(entityPrefab.getComponents());
        }

        /// <summary>
        /// removes all components from the entity and adds it to the destroyed entities queue.
        /// </summary>
        /// <param name="entity"> ID of entity wanting to be destroyed. </param>
        public void DestroyEntity(int entity) //!!!!! GARBAGE COLLECTION (GC) FOR REFERENCED CLASSES IN COMPONENTS IS BAD ||OR|| THIS FUNCTION IS REALLY BAD WHEN DESTROYING ENTITIES !!!!!!\\\\\
        {
            if (!this.IsEntityAlive(entity)) { return; } // entity is either destroyed or has not been created yet

            // may need to allow each component to do some cleanup before enitity being destroyed.

            foreach (List<IComponent> componentList in componentLists.Values)
            {
                if (entity >= componentList.Count || componentList[entity].IsEmptyComponent()) { continue; } // the component list is too small, so no need to remove anything

                var makeEmptyComponent = componentList[entity].GetType().GetMethod("MakeEmptyComponent");
                componentList[entity] = makeEmptyComponent?.Invoke(null, null) as IComponent;
            }

            destroyedEntities.Enqueue(entity, entity);
        }

        /// <summary>
        /// Adds a component to the entity specified.
        /// </summary>
        /// <typeparam name="T"> component adding to entity. </typeparam>
        /// <param name="entity"> entity ID. </param>
        /// <param name="component"> component instance. </param>
        public void AddComponent<T>(int entity, T component) where T : IComponent
        {
            if (!this.IsEntityAlive(entity)) { return; } // entity is either destroyed or has not been created yet

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
                    componentList.Add(T.MakeEmptyComponent());
                }

                componentList.Add(component);
            }

            OnAddComponentEvent?.Invoke(entity, typeof(T));
        }

        /// <summary>
        /// removes the component only if it exists otherwise does nothing
        /// </summary>
        /// <typeparam name="T"> the inherited version of the IComponent. </typeparam>
        /// <param name="entity"> the Id of the entity. </param>
        public void RemoveComponent<T>(int entity) where T : IComponent
        {
            if (!this.IsEntityAlive(entity)) { return; } // entity is either destroyed or has not been created yet
            if (!componentLists.TryGetValue(typeof(T), out List<IComponent> componentList)) { return; }
            if (entity >= componentList.Count) { return; } // the component list is too small, so no need to remove anything

            componentList[entity] = T.MakeEmptyComponent();
        }

        /// <summary>
        /// creates a enumeration query of the type of components specified.
        /// </summary>
        /// <typeparam name="T"> type of component. </typeparam>
        /// <returns> query of components of the type. </returns>
        public IEnumerable<T> GetComponents<T>() where T : IComponent
        {
            if (!componentLists.TryGetValue(typeof(T), out var componentList)) { return []; };

            return componentList.Cast<T>().Where<T>((component) => !component.IsEmptyComponent());
        }

        /// <summary>
        /// Creates a enumeration query of tuples of the two types of components specified. Only gets a tuple if the entity has both components (thus, the intersection).
        /// </summary>
        /// <typeparam name="T"> type of component. </typeparam>
        /// <returns> query of components of the type. </returns>
        public IEnumerable<(T1, T2)> GetComponentsIntersection<T1, T2>()
            where T1 : IComponent
            where T2 : IComponent
        {
            if (!componentLists.TryGetValue(typeof(T1), out var componentList1)) { return []; };
            if (!componentLists.TryGetValue(typeof(T2), out var componentList2)) { return []; };

            return componentList1.Cast<T1>().Zip(
                   componentList2.Cast<T2>()
                ).Where<(T1, T2)>((components) => !components.Item1.IsEmptyComponent() && !components.Item2.IsEmptyComponent());
        }

        /// <summary>
        /// Gets the component from the entity specified.
        /// </summary>
        /// <typeparam name="T"> type of IComponent </typeparam>
        /// <param name="entity"> entity ID. </param>
        /// <returns> component of entity. </returns>
        public T GetComponentFromEntity<T>(int entity) where T : IComponent
        {
            if (!this.IsEntityAlive(entity)) { return (T)T.MakeEmptyComponent(); } // entity is either destroyed or has not been created yet
            if (!componentLists.TryGetValue(typeof(T), out List<IComponent> componentList)) { return (T)T.MakeEmptyComponent(); }
            if (entity >= componentList.Count) { return (T)T.MakeEmptyComponent(); } // the component list is too small

            return (T)componentList[entity];
        }

        /// <summary>
        /// sets the component from the entity specified.
        /// </summary>
        /// <typeparam name="T"> type of IComponent </typeparam>
        /// <param name="entity"> entity ID. </param>
        /// <param name="component"> component that is overriding entity component. </param></param>
        /// <returns> false if failed true when successfull. </returns>
        public bool SetComponentInEntity<T>(int entity, T component) where T : IComponent
        {
            if (!this.IsEntityAlive(entity)) { return false; } // entity is either destroyed or has not been created yet
            if (!componentLists.TryGetValue(typeof(T), out List<IComponent> componentList)) { return false; }
            if (entity >= componentList.Count) { return false; } // the component list is too small

            componentList[entity] = component;
            return true;
        }

        /// <summary>
        /// Gets all the entities with the specified component.
        /// </summary>
        /// <typeparam name="T"> component type. </typeparam>
        /// <returns> list of ID of entities. </returns>
        public List<int> GetEntitiesWithComponent<T>() where T : IComponent
        {
            if (!componentLists.TryGetValue(typeof(T), out List<IComponent> componentList)) { return []; };

            var entities = new List<int>();
            for (int i = 0; i < componentList.Count; i++)
            {
                if (!componentList[i].IsEmptyComponent())
                {
                    entities.Add(i);
                }
            }

            return entities;
        }

        /// <summary>
        /// Gets all the entities with the specified component.
        /// </summary>
        /// <typeparam name="T"> component type. </typeparam>
        /// <returns> list of ID of entities. </returns>
        public List<int> GetEntitiesWithComponent<T>(out Dictionary<int, T> entityWithComponent) where T : IComponent
        {
            entityWithComponent = null;

            if (!componentLists.TryGetValue(typeof(T), out List<IComponent> componentList)) { return []; };

            var entities = new List<int>();
            entityWithComponent = new Dictionary<int, T>();
            for (int i = 0; i < componentList.Count; i++)
            {
                if (!componentList[i].IsEmptyComponent())
                {
                    entities.Add(i);
                    entityWithComponent.Add(i, (T)componentList[i]);
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
        public bool EntityHasComponent<T>(int entity) where T : IComponent
        {
            if (!this.IsEntityAlive(entity)) { return false; } // entity is either destroyed or has not been created yet
            if (!componentLists.TryGetValue(typeof(T), out List<IComponent> componentList)) { return false; } // does not have component since it does not exist in dictionary
            if (entity >= componentList.Count) { return false; } // does not have component since the component list is too small

            return !componentList[entity].IsEmptyComponent();
        }

        /// <summary>
        /// check whether the entity has the component specified and
        /// </summary>
        /// <typeparam name="T"> component being checked on. </typeparam>
        /// <param name="entity"> the id of the entity wanting to check. </param>
        /// <param name="component"> outputs the component that the entity has if it has it. </param>
        /// <returns> true when entity has component otherwise false. </returns>
        public bool EntityHasComponent<T>(int entity, out T component) where T : IComponent
        {
            if (!this.IsEntityAlive(entity)) { component = default; return false; } // entity is either destroyed or has not been created yet
            if (!componentLists.TryGetValue(typeof(T), out List<IComponent> componentList)) { component = default; return false; } // does not have component since it does not exist in dictionary
            if (entity >= componentList.Count) { component = default; return false; } // does not have component since the component list is too small

            if (!componentList[entity].IsEmptyComponent())
            {
                component = (T)componentList[entity];
                return true;
            }
            else
            {
                component = default;
                return false;
            }
        }

        /// <summary>
        /// whether the entity is currently not destroyed and has been created.
        /// </summary>
        /// <param name="entity"> current entity under check. </param>
        /// <returns> true when entity is not destroyed. </returns>
        public bool IsEntityAlive(int entity)
        {
            return !destroyedEntities.UnorderedItems.Any((element) => element.ToTuple().Item1 == entity) && // not in destroyed
                entity < nextId; // entity has been created in ECS before
        }

        public void CheckActiveDependency<T>(Type dependentComponent, int entityID) where T : IComponent
        {
            if (!this.EntityHasComponent<T>(entityID))
            {
                throw new Exception($"Component {nameof(T)} does not exist in entity {entityID} but is required by {dependentComponent.Name}.");
            }
        }

    }
}
