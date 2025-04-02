using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.States
{
    internal abstract class State
    {
        private int entity = 0;

        protected State() { }

        protected State(int entity)
        {
            this.entity = entity;
        }

        protected int Entity
        {
            get => entity;
        }

        public delegate void OnStopped();

        public event OnStopped OnStopEvent;

        public virtual void OnUpdate(GameTime gameTime) { }
        public virtual void Initalize(int entity) { this.entity = entity; }
        public virtual void OnStart() { }
        public virtual void OnStop()
        {
            OnStopEvent?.Invoke();
        }

        public virtual State Clone()
        {
            return this;
        }
    }
}
