
using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Paths
{
    internal struct LinePath : IPath
    {
        public Vector2 start;
        public Vector2 end;

        public LinePath(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }

        /// <inheritdoc/>
        public Vector2 GetPoint(float t)
        {
            return (1 - t) * start + t * end;
        }

        /// <inheritdoc/>
        public float GetDeltaT(float t, float speed)
        {
            return speed / (start - end).Length();
        }

        /// <inheritdoc/>
        public Vector2 GetDerivative(float t)
        {
            return end - start;
        }
    }
}
