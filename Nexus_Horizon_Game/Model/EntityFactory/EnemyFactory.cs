using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Paths;

namespace Nexus_Horizon_Game.EntityFactory
{
    internal static class EnemyFactory
    {
        public static MultiPath sampleBirdPath1()
        {
            Vector2 point1 = new Vector2(0, 0);
            Vector2 point2 = new Vector2(0, 44);
            Vector2 point3 = new Vector2(44, 44);
            Vector2 point4 = new Vector2(132, 44);
            Vector2 point5 = new Vector2(176, 44);
            Vector2 point6 = new Vector2(176, 0);
            QuadraticCurvePath enteringPath = new QuadraticCurvePath(point1, point2, point3);
            LinePath straightPath = new LinePath(point3, point4);
            QuadraticCurvePath leavingPath = new QuadraticCurvePath(point4, point5, point6);
            List<IPath> pathList = new List<IPath>();
            pathList.Add(enteringPath);
            pathList.Add(straightPath);
            pathList.Add(leavingPath);
            MultiPath movementPath = new MultiPath(pathList);
            return movementPath;
        }

        public static MultiPath sampleBirdPath2(float startX)
        {
            Vector2 point1 = new Vector2(startX, 0);
            Vector2 point2 = new Vector2(startX, 44);
            LinePath enteringPath = new LinePath(point1, point2);
            WaitPath waitingPath = new WaitPath(point2, 40);
            LinePath leavingPath = new LinePath(point2, point1);
            List<IPath> pathList = new List<IPath>();
            pathList.Add(enteringPath);
            pathList.Add(waitingPath);
            pathList.Add(leavingPath);
            MultiPath movementPath = new MultiPath(pathList);
            return movementPath;
        }

        public static MultiPath sampleCatPath1(float startX)
        {
            Vector2 point1 = new Vector2(startX, 0);
            Vector2 point2 = new Vector2(startX, 85);
            Vector2 point3 = new Vector2(0, 150);
            LinePath enteringPath = new LinePath(point1, point2);
            WaitPath waitingPath = new WaitPath(point2, 90);
            LinePath leavingPath = new LinePath(point2, point3);
            List<IPath> pathList = new List<IPath>();
            pathList.Add(enteringPath);
            pathList.Add(waitingPath);
            pathList.Add(leavingPath);
            MultiPath movementPath = new MultiPath(pathList);
            return movementPath;
        }

        public static MultiPath sampleCatPath2(float startX)
        {
            Vector2 point1 = new Vector2(startX, 0);
            Vector2 point2 = new Vector2(startX, 85);
            Vector2 point3 = new Vector2(176, 150);
            LinePath enteringPath = new LinePath(point1, point2);
            WaitPath waitingPath = new WaitPath(point2, 90);
            LinePath leavingPath = new LinePath(point2, point3);
            List<IPath> pathList = new List<IPath>();
            pathList.Add(enteringPath);
            pathList.Add(waitingPath);
            pathList.Add(leavingPath);
            MultiPath movementPath = new MultiPath(pathList);
            return movementPath;
        }
    }
}
