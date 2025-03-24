using System;
using System.Collections.Generic;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model;
using Nexus_Horizon_Game.Model.Components;

namespace Nexus_Horizon_Game.Model.Prefab
{
    internal class PrefabEntity : IPrototype<PrefabEntity>
    {
        private List<IComponent> Components;

        public PrefabEntity(List<IComponent> components)
        {
            Components = components;
        }

        public PrefabEntity Clone()
        {
            var clonedComponents = new List<IComponent>(this.Components);

            return new PrefabEntity(clonedComponents);
        }

        public List<IComponent> getComponents()
        {
            return Components;
        }
    }
}
