using System;
using System.Collections.Generic;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model;
using Nexus_Horizon_Game.Model.Components;

namespace Nexus_Horizon_Game.Model.Prefab
{
    internal class PrefabEntity : IPrototype<PrefabEntity>
    {
        private List<IComponent> components;

        public PrefabEntity()
        {
            components = new List<IComponent>();
        }

        public PrefabEntity(List<IComponent> components)
        {
            this.components = components;
        }

        public PrefabEntity Clone()
        {
            var clonedComponents = new List<IComponent>(this.components);

            return new PrefabEntity(clonedComponents);
        }

        public List<IComponent> Components { get { return components; } }

        public List<IComponent> getComponents()
        {
            return components;
        }
    }
}
