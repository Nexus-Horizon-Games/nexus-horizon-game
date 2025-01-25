using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Nexus_Horizon_Game
{
    internal class RenderSystem
    {
        public static void Draw(World world, GameTime gameTime)
        {
            var transformComponents = world.GetComponents<TransformComponent>().ToList();
            var spriteComponents = world.GetComponents<SpriteComponent>().ToList();

            var entitiesWithSprites = world.GetEntitiesWithComponent<SpriteComponent>();

            foreach (int entity in entitiesWithSprites)
            {

                if (world.EntityHasComponent<SpriteComponent>(entity))
                {
                    var transformComp = world.GetComponentFromEntity<TransformComponent>(entity);
                    var spriteComp = world.GetComponentFromEntity<SpriteComponent>(entity);
                    Renderer.Draw(spriteComp.textureName, transformComp.position + spriteComp.position, spriteComp.sourceRectangle, spriteComp.color, (float)transformComp.rotation + spriteComp.rotation, Vector2.Zero, spriteComp.scale, SpriteEffects.None, spriteComp.z);
                }
            }
        }
    }
}
