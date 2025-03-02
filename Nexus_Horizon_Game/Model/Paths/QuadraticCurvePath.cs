using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Paths
{
    internal struct QuadraticCurvePath : IPath
    {
        public Vector2 start;
        public Vector2 control;
        public Vector2 end;

        // chached vectors to calculate the derivative
        private readonly Vector2 v1;
        private readonly Vector2 v2;

        public QuadraticCurvePath(Vector2 start, Vector2 control, Vector2 end)
        {
            this.start = start;
            this.control = control;
            this.end = end;

            this.v1 = 2 * start - 4 * control + 2 * end;
            this.v2 = -2 * start + 2 * control;
        }

        /// <inheritdoc/>
        public Vector2 GetPoint(float t)
        {
            return ((1.0f - t) * (1 - t)) * start +
                (2.0f * (1.0f - t) * t) * control +
                (t * t) * end;
        }

        /// <inheritdoc/>
        public Vector2 GetDerivative(float t)
        {
            return t * v1 + v2;
        }

        /// <inheritdoc/>
        public float GetDeltaT(float t, float speed)
        {
            // Help from: https://gamedev.stackexchange.com/questions/27056/how-to-achieve-uniform-speed-of-movement-on-a-bezier-curve
            return speed / GetDerivative(t).Length();
        }
    }
}
