using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Linq;

namespace Nexus_Horizon_Game.Systems
{
    internal static class OnUpdateSystem
    {
        public static void Update(GameTime gameTime)
        {
            var entities = GameM.CurrentScene.World.GetEntitiesWithComponent<OnUpdateComponent>().ToList();

            foreach (var entity in entities)
            {
                var component = GameM.CurrentScene.World.GetComponentFromEntity<OnUpdateComponent>(entity);

                component.onUpdate(entity, gameTime);
            }
        }
    }
}
