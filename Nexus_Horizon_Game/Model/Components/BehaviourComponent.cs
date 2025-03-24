using Nexus_Horizon_Game.Entity_Type_Behaviours;

namespace Nexus_Horizon_Game.Components
{
    internal struct BehaviourComponent : IComponent
    {
        private bool isEmpty;

        private Behaviour behaviour = null;

        public BehaviourComponent(Behaviour behaviour)
        {
            this.isEmpty = false;
            this.behaviour = behaviour;
        }

        public Behaviour Behaviour
        {
            get => behaviour;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is BehaviourComponent o)
            {
                if (behaviour == o.behaviour)
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
            BehaviourComponent comp = new();
            comp.isEmpty = true;
            return comp;
        }
    }
}
