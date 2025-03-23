﻿using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    internal struct OnUpdateComponent : IComponent
    {
        private bool isEmpty;

        public delegate void OnUpdate(int thisEntity, GameTime gameTime);

        public OnUpdate onUpdate;

        public OnUpdateComponent(OnUpdate onUpdate)
        {
            this.isEmpty = false;
            this.onUpdate = onUpdate;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is OnUpdateComponent o)
            {
                if (onUpdate == o.onUpdate)
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
            OnUpdateComponent comp = new((int entity, GameTime gameTime) => { });
            comp.isEmpty = true;
            return comp;
        }
    }
}
