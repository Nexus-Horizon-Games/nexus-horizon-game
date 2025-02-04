using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Timers
{
    internal class LoopTimer : Timer
    {
        private float interval = 0.0f;

        private bool stopOnInterval;
        private double startTime;
        private object data = null;

        /// <summary>
        /// initializes a timer.
        /// </summary>
        /// <param name="timeInterval"> the time between each invoke. </param>
        /// <param name="onElapsed"> listener after timer expires. </param>
        /// <param name="data"> data wanted to set through to listener. </param>
        public LoopTimer(float timeInterval, OnElapsed onElapsed, object data = null, bool stopOnInterval = false)
        {
            interval = timeInterval;
            OnElapsedEvent += onElapsed;
            this.data = data;
            this.stopOnInterval = stopOnInterval;
        }

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

            if (startTime > interval)
            {
                InvokeElapsedEvent(gameTime, data);
                startTime = 0.0;

                if (stopOnInterval)
                {
                    Stop();
                }
            }
        }
    }
}
