using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using System.Linq;

namespace Nexus_Horizon_Game
{
    internal class RenderSystem
    {
        public static void Draw(World world, GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteTexture) // spriteBatch and spriteTexture being passed here should only be temporary
        {
            var transformComponents = world.GetComponents<TransformComponent>().ToList();
            var spriteComponents = world.GetComponents<SpriteComponent>().ToList();

            for (var i = 0; i < MathHelper.Min(transformComponents.Count(), spriteComponents.Count()); i++)
            {
                var transformComp = transformComponents[i];
                var spriteComp = spriteComponents[i];

                if (!transformComp.IsEmptyComponent() && !spriteComp.IsEmptyComponent())
                {
                    spriteBatch.Draw(spriteTexture, transformComp.position, Color.White);
                }
            }
        }
    }
}
