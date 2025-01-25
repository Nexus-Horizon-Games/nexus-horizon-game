namespace Nexus_Horizon_Game.Components
{
    internal struct PlayerComponent : IComponent
    {
        private bool isEmpty;

        public PlayerComponent()
        {
            isEmpty = false;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is PlayerComponent o)
            {
                if ((object)other == (object)this)
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
            PlayerComponent bullet = new PlayerComponent();
            bullet.isEmpty = true;
            return bullet;
        }
    }
}
