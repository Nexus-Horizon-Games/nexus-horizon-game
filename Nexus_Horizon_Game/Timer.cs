
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game
{
    internal class Timer
    {
        private float interval = 0.0f;

        public delegate void OnElapsed(GameTime gameTime, object? data);

        private OnElapsed onElapsed;

        private bool isOn = false;
        private bool stopOnInterval;
        private double startTime;
        private object? data = null;

        /// <summary>
        /// initializes a timer.
        /// </summary>
        /// <param name="timeInterval"> the time between each invoke. </param>
        /// <param name="onElapsed"> listener after timer expires. </param>
        /// <param name="data"> data wanted to set through to listener. </param>
        public Timer(float timeInterval, OnElapsed onElapsed, object? data = null, bool stopOnInterval = false)
        {
            interval = timeInterval;
            this.onElapsed = onElapsed;
            this.data = data;
            this.stopOnInterval = stopOnInterval;
        }

        public bool IsOn
        {
            get => isOn;
        }

        public void Start()
        {
            startTime = 0.0;
            isOn = true;
        }

        /// <summary>
        /// stops the timer.
        /// </summary>
        /// <returns> the time the timer was stopped at. </returns>
        public double Stop()
        {
            isOn = false;
            return startTime;
        }

        public void Update(GameTime gameTime)
        {
            if (!isOn)
            {
                return;
            }

            startTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (startTime > interval)
            {
                onElapsed?.Invoke(gameTime, data);
                startTime = 0.0; // why -= interval? could be a few microseconds off
                
                if (stopOnInterval)
                {
                    this.Stop();
                }
            }
        }
    }
}
