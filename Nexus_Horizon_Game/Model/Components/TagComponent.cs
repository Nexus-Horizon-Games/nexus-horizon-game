using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Components
{
    [Flags]
    internal enum Tag
    {
        PLAYER = 1,
        ENEMY = 2,
        PLAYER_PROJECTILE= 4,
        ENEMY_PROJECTILE = 8,
        SMALLGRUNT = 16,
        MEDIUMGRUNT = 32,
        HALFBOSS = 64,
        BOSS = 128,
        POWERDROP = 256,
        POINTDROP = 512,
    }

    internal struct TagComponent : IComponent
    {
        private bool isEmpty;

        private Tag tag = 0;

        public TagComponent(Tag tag)
        {
            this.isEmpty = false;
            this.tag = tag;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        public Tag Tag
        {
            get => tag;
            set => tag = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is TagComponent o)
            {
                if (tag == o.tag)
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
            TagComponent comp = new()
            {
                isEmpty = true
            };
            return comp;
        }
    }
}
