using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Components
{
    internal struct OnUpdateComponent : IComponent
    {
        private bool isEmpty;

        public delegate void OnUpdate(World world, int thisEntity, GameTime gameTime);

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
            OnUpdateComponent comp = new((World world, int entity, GameTime gameTime) => {});
            comp.isEmpty = true;
            return comp;
        }
    }
}
