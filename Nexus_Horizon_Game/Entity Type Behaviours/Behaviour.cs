using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Entity_Type_Behaviours
{
    internal abstract class Behaviour
    {
        protected int thisEntity = 0;

        protected Behaviour(int thisEntity)
        {
            this.thisEntity = thisEntity;
        }

        public virtual void OnUpdate(GameTime gameTime) { }
    }
}
