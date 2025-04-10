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

        private float? stopAfter;

        private double startTime;
        private double startIntervalTime;

        private object data = null;

        /// <summary>
        /// initializes a timer.
        /// </summary>
        /// <param name="timeInterval"> the time between each invoke. </param>
        /// <param name="onElapsed"> listener after timer expires. </param>
        /// <param name="data"> data wanted to set through to listener. </param>
        public LoopTimer(float timeInterval, object data = null, float? stopAfter = null)
        {
            interval = timeInterval;
            this.data = data;
            this.stopAfter = stopAfter;
        }

        /// <summary>
        /// initializes a timer.
        /// </summary>
        /// <param name="timeInterval"> the time between each invoke. </param>
        /// <param name="onElapsed"> listener after timer expires. </param>
        /// <param name="data"> data wanted to set through to listener. </param>
        public LoopTimer(float timeInterval, OnElapsed onElapsed, object data = null, float? stopAfter = null)
        {
            interval = timeInterval;
            OnElapsedEvent += onElapsed;
            this.data = data;
            this.stopAfter = stopAfter;
        }

        public override void Start()
        {
            startTime = 0.0;
            startIntervalTime = 0.0;
            isOn = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isOn)
            {
                return;
            }

            startIntervalTime += gameTime.ElapsedGameTime.TotalSeconds;
            startTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (startIntervalTime > interval)
            {
                InvokeElapsedEvent(gameTime, data);
                startIntervalTime = 0.0;
            }

            if (startTime > stopAfter)
            {
                Stop();
            }
        }
    }
}
