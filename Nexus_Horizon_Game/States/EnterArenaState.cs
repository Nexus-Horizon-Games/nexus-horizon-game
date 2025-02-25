
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Paths;

namespace Nexus_Horizon_Game.States
{
    internal class EnterArenaState : State
    {
        private readonly Vector2 stopPoint;
        private readonly float enteringSpeed;
        private IPath path;
        private float time;

        public EnterArenaState(int entity, Vector2 stopPoint, float enteringSpeed) : base(entity)
        {
            this.stopPoint = stopPoint;
            this.enteringSpeed = enteringSpeed;
        }

        public override void OnStart()
        {
            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
            path = new LinePath(transform.position, stopPoint);
        }

        public override void OnUpdate(GameTime gameTime)
        {
            time += path.GetDeltaT(time, (float)(enteringSpeed * gameTime.ElapsedGameTime.TotalSeconds));
            if (time >= 1.0f)
            {
                OnStop();
                return;
            }

            var transform = GameM.CurrentScene.World.GetComponentFromEntity<TransformComponent>(this.Entity);
            transform.position = path.GetPoint(time);
            GameM.CurrentScene.World.SetComponentInEntity(this.Entity, transform);
        }
    }
}
