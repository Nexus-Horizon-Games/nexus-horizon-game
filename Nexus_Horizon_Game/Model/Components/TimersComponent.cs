using Nexus_Horizon_Game.Timers;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Components
{
    internal struct TimersComponent : IComponent
    {
        private bool isEmpty;

        public Dictionary<string, Timer> timers;

        public TimersComponent(Dictionary<string, Timer> timers)
        {
            this.isEmpty = false;
            this.timers = timers;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is TimersComponent o)
            {
                if (timers == o.timers)
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
            TimersComponent comp = new([]);
            comp.isEmpty = true;
            return comp;
        }
    }
}
