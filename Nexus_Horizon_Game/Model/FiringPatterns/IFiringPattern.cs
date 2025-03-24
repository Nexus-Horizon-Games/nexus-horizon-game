using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Model.Prefab;
using System;
using System.Collections.Generic;
using Nexus_Horizon_Game.EntityFactory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nexus_Horizon_Game.Timers;

namespace Nexus_Horizon_Game.Model.EntityPatterns
{
    internal interface IFiringPattern
    {
        
        void Fire(PrefabEntity prefab, GameTime gameTime, TimerContainer timerContainer);
        
    }
}
