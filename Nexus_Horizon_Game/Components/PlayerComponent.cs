using Nexus_Horizon_Game.EntityFactory;

namespace Nexus_Horizon_Game.Components
{
    internal struct PlayerComponent : IComponent
    {
        private bool isEmpty;

        private int hitboxVisualEntityID;
        private BulletFactory bulletFactory;

        public PlayerComponent(int hitboxVisualEntityID)
        {
            this.hitboxVisualEntityID = hitboxVisualEntityID;
            this.bulletFactory = new BulletFactory("BulletSample");
            isEmpty = false;
        }

        public int HitboxVisualEntityID
        {
            get => this.hitboxVisualEntityID;
        }

        public BulletFactory BulletFactory
        {
            get => bulletFactory;
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
