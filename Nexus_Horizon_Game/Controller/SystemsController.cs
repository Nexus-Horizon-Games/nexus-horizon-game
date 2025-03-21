using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Controller.Systems;
using Nexus_Horizon_Game.Systems;

namespace Nexus_Horizon_Game.Controller
{
    internal delegate void SystemUpdate(GameTime gameTime);

    internal class SystemsController
    {
        public event SystemUpdate SystemUpdate = (gametime) => { };

        public SystemsController()
        {
            this.SystemUpdate += MovementSystem.Update;
            this.SystemUpdate += PhysicsSystem.Update;
            this.SystemUpdate += BehaviourSystem.Update;
            this.SystemUpdate += StateSystem.Update;
            this.SystemUpdate += TimerSystem.Update;
            this.SystemUpdate += CollisionSystem.Update;
            this.SystemUpdate += HealthSystem.Update;
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
