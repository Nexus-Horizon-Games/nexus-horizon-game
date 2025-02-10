
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Paths
{
    internal struct WaitPath : IPath
    {
        public Vector2 point;
        public float time;

        public WaitPath(Vector2 point, float time)
        {
            this.point = point;
            this.time = time;
        }

        /// <inheritdoc/>
        public Vector2 GetPoint(float t)
        {
            return point;
        }

        /// <inheritdoc/>
        public float GetDeltaT(float t, float speed)
        {
            return speed / time;
        }

        /// <inheritdoc/>
        public Vector2 GetDerivative(float t)
        {
            return new Vector2(1,0) ;
        }
    }
}
