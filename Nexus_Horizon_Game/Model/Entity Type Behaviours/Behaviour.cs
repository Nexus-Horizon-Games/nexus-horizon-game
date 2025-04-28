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
        private int entity = 0;

        protected Behaviour() { }

        protected Behaviour(int entity)
        {
            this.entity = entity;
        }

        protected int Entity
        {
            get => entity;
        }


        public virtual void Initalize(int entity) { this.entity = entity; }
        public virtual void OnUpdate(GameTime gameTime) { }
    }
}
