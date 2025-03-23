using Nexus_Horizon_Game.Model;

namespace Nexus_Horizon_Game.Components
{
    internal struct MovementControllerComponent : IComponent
    {
        private bool isEmpty;

        private MovementController controller;

        public MovementControllerComponent(MovementController controller, int entityID)
        {
            this.controller = controller;
        }

        public MovementController Controller
        {
            get => controller;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is MovementControllerComponent o)
            {
                if (this.controller == o.controller)
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
            MovementControllerComponent transfrom = new MovementControllerComponent(null, -1);
            transfrom.isEmpty = true;
            return transfrom;
        }
    }
}
