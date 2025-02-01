using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Systems
{
    internal static class OnUpdateSystem
    {
        public static void Update(World world, GameTime gameTime)
        {
            var entities = world.GetEntitiesWithComponent<OnUpdateComponent>().ToList();

            foreach (var entity in entities)
            {
                var component = world.GetComponentFromEntity<OnUpdateComponent>(entity);

                component.onUpdate(world, entity, gameTime);
            }
        }
    }
}
