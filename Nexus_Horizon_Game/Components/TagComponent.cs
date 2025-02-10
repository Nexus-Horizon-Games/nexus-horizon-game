using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Components
{
    internal enum Tag
    {
        PLAYER,
        ENEMY,
        PROJECTILE,
    }

    internal struct TagComponent : IComponent
    {
        private bool isEmpty;

        private Tag tag;

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
