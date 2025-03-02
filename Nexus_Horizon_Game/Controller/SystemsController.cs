using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nexus_Horizon_Game.GameM;

namespace Nexus_Horizon_Game.Controller
{
    internal delegate void SystemUpdate(GameTime gameTime);

    internal class SystemsController
    {
        public event SystemUpdate SystemUpdate = (gametime) => { };

        public SystemsController() 
        {
            this.SystemUpdate += PhysicsSystem.Update;
            this.SystemUpdate += BehaviourSystem.Update;
            this.SystemUpdate += StateSystem.Update;
            this.SystemUpdate += TimerSystem.Update;
            this.SystemUpdate += InputSystem.Update;
        }

        public void Update(GameTime gameTime)
        {
            SystemUpdate.Invoke(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            RenderSystem.Draw(gameTime, Scene.Loaded);
        }
    }
}
