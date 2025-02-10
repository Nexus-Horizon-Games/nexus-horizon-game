using Microsoft.Xna.Framework;
using System;

namespace Nexus_Horizon_Game.Paths
{
    internal class CubicCurvePath : IPath
    {
        public Vector2 start;
        public Vector2 controlStart;
        public Vector2 controlEnd;
        public Vector2 end;

        // chached vectors to calculate the derivative
        private readonly Vector2 v1;
        private readonly Vector2 v2;
        private readonly Vector2 v3;

        public CubicCurvePath(Vector2 start, Vector2 controlStart, Vector2 controlEnd, Vector2 end)
        {
            this.start = start;
            this.controlStart = controlStart;
            this.controlEnd = controlEnd;
            this.end = end;

            this.v1 = -3 * start + 9 * controlStart - 9 * controlEnd + 3 * end;
            this.v2 = 6 * start - 12 * controlStart + 6 * controlEnd;
            this.v3 = -3 * start + 3 * controlStart;
        }

        /// <inheritdoc/>
        public Vector2 GetPoint(float t)
        {
            return MathF.Pow((1f - t), 3f) * start +
                3f * t * MathF.Pow((1f - t), 2f) * controlStart +
                3f * MathF.Pow(t, 2f) * (1f - t) * controlEnd +
                MathF.Pow(t, 3f) * end;
        }

        /// <inheritdoc/>
        public Vector2 GetDerivative(float t)
        {
            return MathF.Pow(t, 2) * v1 + t * v2 + v3;
        }

        /// <inheritdoc/>
        public float GetDeltaT(float t, float speed)
        {
            // Help from: https://gamedev.stackexchange.com/questions/27056/how-to-achieve-uniform-speed-of-movement-on-a-bezier-curve
            return speed / GetDerivative(t).Length();
        }
    }
}
