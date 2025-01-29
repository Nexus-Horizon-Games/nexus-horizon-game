using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    /// <summary>
    /// A component that stores a single state (an enum) for an entity.
    /// </summary>
    internal struct StateComponent : IComponent
    {
        private bool isEmpty;

        public System.Enum state;

        public StateComponent(System.Enum state)
        {
            this.isEmpty = false;
            this.state = state;
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
                if (state == o.state)
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
