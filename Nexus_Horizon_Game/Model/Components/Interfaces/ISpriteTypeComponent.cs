using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Model.Components.Interfaces
{
    internal interface ISpriteTypeComponent
    {
        public uint SpriteLayer
        {
            get;
            set;
        }

        public bool IsVisible
        {
            get;
            set;
        }
    }
}
