using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Timers
{
    internal class DelayTimer : Timer
    {
        private float delay = 0.0f;

        private double startTime;
        private object data = null;

        /// <summary>
        /// initializes a timer.
        /// </summary>
        /// <param name="timeInterval"> the time between each invoke. </param>
        /// <param name="onElapsed"> listener after timer expires. </param>
        /// <param name="data"> data wanted to set through to listener. </param>
        public DelayTimer(float delay, OnElapsed onElapsed, object data = null)
        {
            this.delay = delay;
            OnElapsedEvent += onElapsed;
            this.data = data;
        }

        public float Delay => delay;

        public override void Start()
        {
            startTime = 0.0;
            isOn = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isOn)
            {
                return;
            }

            startTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (startTime > delay)
            {
                InvokeElapsedEvent(gameTime, data);
                Stop();
            }
        }
    }
}
