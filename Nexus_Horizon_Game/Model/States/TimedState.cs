﻿
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Timers;
using System.Diagnostics;

namespace Nexus_Horizon_Game.States
{
    internal class TimedState : State
    {
        protected readonly Timer timer;

        public TimedState(float timeLength)
        {
            timer = new DelayTimer(timeLength, (_, _) => { OnStop(); });
        }

        public TimedState(int entity, float timeLength)
            : base(entity)
        {
            timer = new DelayTimer(timeLength, (_, _) => { OnStop(); });
        }

        public override void OnUpdate(GameTime gameTime)
        {
            timer.Update(gameTime);
        }

        public override void OnStart()
        {
            timer.Start();
        }
    }
}
