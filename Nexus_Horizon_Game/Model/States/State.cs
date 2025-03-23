using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.States
{
    internal abstract class State
    {
        private readonly int entity = 0;

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
        public virtual void OnStart() { }
        public virtual void OnStop()
        {
            OnStopEvent?.Invoke();
        }
    }
}
