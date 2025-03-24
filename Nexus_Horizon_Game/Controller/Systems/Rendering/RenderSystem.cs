using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Nexus_Horizon_Game.Model.Components;

namespace Nexus_Horizon_Game
{
    internal static class RenderSystem
    {
        public static void Draw(GameTime gameTime, Scene currentScene)
        {
            // Sprite Rendering
            var transformAndSpriteComponents = currentScene.ECS.GetComponentsIntersection<TransformComponent, SpriteComponent>().Where((data) => data.Item2.IsVisible).OrderBy((data) => data.Item2.SpriteLayer);

            foreach (var tuple in transformAndSpriteComponents)
            {
                var transformComp = tuple.Item1;
                var spriteComp = tuple.Item2;

                if (spriteComp.centered)
                {
                    spriteComp.position -= (Renderer.GetTextureBounds(spriteComp.textureName) * spriteComp.scale) / 2.0f;
                }

                if (!spriteComp.IsUI)
                {
                    Renderer.Draw(spriteComp.textureName, transformComp.position + spriteComp.position, spriteComp.sourceRectangle, spriteComp.color, (float)transformComp.rotation + spriteComp.rotation, Vector2.Zero, spriteComp.scale, SpriteEffects.None, spriteComp.Z);
                }
                else
                {
                    Renderer.DrawUI(spriteComp.textureName, transformComp.position + spriteComp.position);
                }
            }


            // SpriteFont Rendering
            var transformAndSpriteFontComponents = currentScene.ECS.GetComponentsIntersection<TransformComponent, SpriteFontComponent>().Where((data) => data.Item2.IsVisible).OrderBy((data) => data.Item2.SpriteLayer);

            foreach (var tuple in transformAndSpriteFontComponents)
            {
                var transformComp = tuple.Item1;
                var spriteFontComp = tuple.Item2;

                if (spriteFontComp.centered)
                {
                    spriteFontComp.position -= (Renderer.GetStringBounds(spriteFontComp.fontPath, spriteFontComp.text) * spriteFontComp.scale) / 2.0f;
                }

                Renderer.DrawFont(spriteFontComp.fontPath, spriteFontComp.text, transformComp.position + spriteFontComp.position, spriteFontComp.color, (float)transformComp.rotation + spriteFontComp.rotation, Vector2.Zero, spriteFontComp.scale, SpriteEffects.None, spriteFontComp.Z);
            }

        }
    }
}
