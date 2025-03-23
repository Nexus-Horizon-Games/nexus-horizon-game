
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Nexus_Horizon_Game.Timers
{
    internal abstract class Timer
    {
        protected bool isOn = false;

        public bool IsOn
        {
            get => isOn;
        }


        public delegate void OnElapsed(GameTime gameTime, object data);
        public delegate void OnStop();

        public event OnElapsed OnElapsedEvent;
        public event OnStop OnStopEvent;

        public virtual void Start()
        {
            isOn = true;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public virtual void Stop()
        {
            isOn = false;
            OnStopEvent?.Invoke();
        }

        public abstract void Update(GameTime gameTime);

        protected virtual void InvokeElapsedEvent(GameTime gameTime, object data)
        {
            OnElapsedEvent?.Invoke(gameTime, data);
        }
    }
}
