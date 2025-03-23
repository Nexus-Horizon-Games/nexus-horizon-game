
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Paths;

namespace Nexus_Horizon_Game.States
{
    internal class MoveToPointState : State
    {
        private readonly Vector2 stopPoint;
        private readonly float speed;
        private IPath path;
        private float time;

        public MoveToPointState(int entity, Vector2 stopPoint, float speed) : base(entity)
        {
            this.stopPoint = stopPoint;
            this.speed = speed;
        }

        public override void OnStart()
        {
            var transform = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity);
            path = new LinePath(transform.position, stopPoint);
        }

        public override void OnUpdate(GameTime gameTime)
        {
            time += path.GetDeltaT(time, (float)(speed * gameTime.ElapsedGameTime.TotalSeconds));
            if (time >= 1.0f)
            {
                OnStop();
                return;
            }

            var transform = Scene.Loaded.ECS.GetComponentFromEntity<TransformComponent>(this.Entity);
            transform.position = path.GetPoint(time);
            Scene.Loaded.ECS.SetComponentInEntity(this.Entity, transform);
        }
    }
}
