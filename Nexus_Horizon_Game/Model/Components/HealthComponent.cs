using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    internal struct HealthComponent : IComponent
    {
        private bool isEmpty;

        /// <summary>
        /// The health of the entity (can also represent lives, as with the player)
        /// </summary>
        public float health;

        public delegate void OnDeath();

        /// <summary>
        /// Triggered when health goes below 1.
        /// </summary>
        public event OnDeath onDeathEvent;

        public HealthComponent(float health)
        {
            this.health = health;
        }

        public HealthComponent(float health, OnDeath deathHandler)
        {
            this.health = health;
            this.onDeathEvent += deathHandler;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <summary>
        /// Checks whether death has happened and triggers the onDeathEvent.
        /// </summary>
        /// <returns>Whether death has happened.</returns>
        public bool CheckForDeath()
        {
            if (health <= 0)
            {
                onDeathEvent?.Invoke();
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is HealthComponent o)
            {
                if (health == o.health)
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
            HealthComponent healthComp = new HealthComponent(-1)
            {
                isEmpty = true,
            };

            return healthComp;
        }
    }
}
