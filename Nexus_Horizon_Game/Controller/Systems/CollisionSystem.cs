using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Controller
{
    /// <summary>
    /// We Can use Spatial Partitioning if the n^2 time complexity starts lagging otherwise this'll work fine
    /// </summary>
    internal static class CollisionSystem
    {
        public static void Update(GameTime gametime)
        {
            // get entities with colliders

            List<int> entityIDList = new List<int>();

            /// foreach collidercomponent1
            ///     List<int> entityIDList = new List<int>();
            ///     
            ///     foreach collidercomponent2
            ///         check to see if collider1 hit collider 2 // use rectangle variable in collidercomponent and the AABB thing
            ///             add entityID to list if they do
            ///     
            ///     collidercomponent1.SendOnCollisionInfo(entityIDList)
            ///     


        }
    }
}
