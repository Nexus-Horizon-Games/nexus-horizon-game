using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Components
{
    internal struct SpriteComponent : IComponent
    {
        public SpriteComponent(int spriteID)
        {
            this.spriteID = spriteID;
        }

        int spriteID = -1;

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is SpriteComponent o)
            {
                if (spriteID == o.spriteID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsEmptyComponent()
        {
            return spriteID == -1;
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            return new SpriteComponent(-1);
        }
    }
}
