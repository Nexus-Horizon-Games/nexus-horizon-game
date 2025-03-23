using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Paths
{
    internal interface IPath
    {
        /// <summary>
        /// Gets a point on the path.
        /// </summary>
        /// <param name="t">Should be between 0 and 1.</param>
        /// <returns>The point.</returns>
        Vector2 GetPoint(float t);

        /// <summary>
        /// Gets the derivative of a point on the curve.
        /// </summary>
        /// <param name="t">Should be between 0 and 1.</param>
        /// <returns>The derivative.</returns>
        Vector2 GetDerivative(float t);

        /// <summary>
        /// Gets the change in 't', given the speed.
        /// </summary>
        /// <param name="t">Should be between 0 and 1.</param>
        /// <param name="speed">The speed.</param>
        /// <returns>The change in 't' that chould take place.</returns>
        float GetDeltaT(float t, float speed);
    }
}
