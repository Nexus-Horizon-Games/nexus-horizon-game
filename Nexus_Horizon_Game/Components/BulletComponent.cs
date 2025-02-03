namespace Nexus_Horizon_Game.Components
{
    internal struct BulletComponent : IComponent
    {
        private bool isEmpty;

        private bool flicker;

        public BulletComponent(bool flicker = false)
        {
            isEmpty = false;
            this.flicker = flicker;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is BulletComponent o)
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
            BulletComponent bullet = new BulletComponent();
            bullet.isEmpty = true;
            return bullet;
        }
    }
}
