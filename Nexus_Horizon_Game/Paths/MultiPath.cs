using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Paths
{
    internal struct MultiPath : IPath
    {
        private List<IPath> paths;

        public MultiPath(List<IPath> paths)
        {
            this.paths = paths;
        }

        public Vector2 GetPoint(float t)
        {
            if (paths.Count == 0)
            {
                return Vector2.Zero;
            }

            int pathIndex = (int)(t * paths.Count);

            if (pathIndex >= paths.Count)
            {
                return paths[paths.Count - 1].GetPoint(1.0f);
            }
            else if (pathIndex < 0)
            {
                return paths[0].GetPoint(0.0f);
            }

            return paths[pathIndex].GetPoint((t * paths.Count) - pathIndex);
        }

        public Vector2 GetDerivative(float t)
        {
            if (paths.Count == 0)
            {
                return Vector2.Zero;
            }

            int pathIndex = (int)(t * paths.Count);

            if (pathIndex >= paths.Count)
            {
                return paths[paths.Count - 1].GetDerivative(1.0f);
            }
            else if (pathIndex < 0)
            {
                return paths[0].GetDerivative(0.0f);
            }

            return paths[pathIndex].GetDerivative((t * paths.Count) - pathIndex);
        }
        public int getIndex(float t)
        {
            return (int)(t * paths.Count); 
        }
        public float GetDeltaT(float t, float speed)
        {
            if (paths.Count == 0)
            {
                return 0.0f;
            }

            int pathIndex = (int)(t * paths.Count);

            if (pathIndex >= paths.Count)
            {
                return 0.0f;
            }
            else if (pathIndex < 0)
            {
                return 0.0f;
            }

            return paths[pathIndex].GetDeltaT((t * paths.Count) - pathIndex, speed);
        }
    }
}
