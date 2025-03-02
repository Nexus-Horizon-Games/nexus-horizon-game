using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Nexus_Horizon_Game.Timers
{
    internal class TimerContainer
    {
        private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();

        /// <summary>
        /// Adds a timer.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="name">An identifier to refernce the timer with.</param>
        public void AddTimer(Timer timer, string name)
        {
            timers.Add(name, timer);
        }

        /// <summary>
        /// Gets a timer.
        /// </summary>
        /// <param name="name">The name of the timer.</param>
        /// <returns>The timer with that name.</returns>
        public Timer GetTimer(string name)
        {
            return timers[name];
        }

        /// <summary>
        /// Adds and starts a timer which will be removed when it stops.
        /// </summary>
        /// <param name="timer">The timer.</param>
        /// <param name="name">An identifier to reference the timer with.</param>
        public void StartTemporaryTimer(Timer timer, string name = null)
        {
            if (name == null)
            {
                name = Guid.NewGuid().ToString();
            }

            timer.OnStopEvent += () =>
            {
                timers.Remove(name);
            };

            timers.Add(name, timer);
            timer.Start();
        }

        /// <summary>
        /// Updates all the timers.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        public void Update(GameTime gameTime)
        {
            foreach (var timer in timers.Values.ToList())
            {
                timer.Update(gameTime);
            }
        }
    }
}
