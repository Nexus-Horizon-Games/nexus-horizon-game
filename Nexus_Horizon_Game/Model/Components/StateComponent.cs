using Nexus_Horizon_Game.States;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Components
{
    /// <summary>
    /// A component that stores a single state (an enum) for an entity.
    /// </summary>
    internal struct StateComponent : IComponent
    {
        private bool isEmpty;

        public int currentState = 0;
        public List<State> states;

        public StateComponent(List<State> states)
        {
            this.isEmpty = false;
            this.states = states;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is StateComponent o)
            {
                if (states == o.states)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsEmptyComponent()
        {
            return isEmpty;
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            StateComponent comp = new()
            {
                isEmpty = true
            };
            return comp;
        }
    }
}
