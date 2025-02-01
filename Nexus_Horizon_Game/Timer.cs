
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game
{
    internal class Timer
    {
        private float interval = 0.0f;

        public delegate void OnElapsed(GameTime gameTime, object? data);

        private OnElapsed onElapsed;

        private bool isOn = false;
        private double startTime;
        private object? data = null;

        public Timer(float timeInterval, OnElapsed onElapsed, object? data = null)
        {
            interval = timeInterval;
            this.onElapsed = onElapsed;
            this.data = data;
        }

        public void Start()
        {
            startTime = 0.0;
            isOn = true;
        }

        public void Stop()
        {
            isOn = false;
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
                onElapsed.Invoke(gameTime, data);
                startTime -= interval;
            }
        }
    }
}
