using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nexus_Horizon_Game.Components;
using System.Linq;

namespace Nexus_Horizon_Game
{
    internal class RenderSystem
    {
        public static void Draw(World world, GameTime gameTime, Texture2D spriteTexture) // spriteTexture being passed here should only be temporary
        {
            var transformComponents = world.GetComponents<TransformComponent>().ToList();
            var spriteComponents = world.GetComponents<SpriteComponent>().ToList();

            for (var i = 0; i < MathHelper.Min(transformComponents.Count(), spriteComponents.Count()); i++)
            {
                var transformComp = transformComponents[i];
                var spriteComp = spriteComponents[i];

                if (!transformComp.IsEmptyComponent() && !spriteComp.IsEmptyComponent())
                {
                    Renderer.Draw(spriteTexture, transformComp.position + spriteComp.position, spriteComp.sourceRectangle, spriteComp.color, (float)transformComp.rotation + spriteComp.rotation, Vector2.Zero, spriteComp.scale, SpriteEffects.None, spriteComp.z);
                }
            }
        }
    }
}
