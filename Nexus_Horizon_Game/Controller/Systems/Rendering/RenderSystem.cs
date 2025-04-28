using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Nexus_Horizon_Game.Model.Components;
using System.Collections.Generic;
using Nexus_Horizon_Game.Model.Components.Interfaces;
using Microsoft.Xna.Framework.Content;

namespace Nexus_Horizon_Game
{
    internal static class RenderSystem
    {
        public static void Draw(GameTime gameTime, Scene currentScene)
        {
            // Sprite Rendering
            IEnumerable<(TransformComponent, ISpriteTypeComponent)> transformAndSpriteComponents = currentScene.ECS
                .GetComponentsIntersection<TransformComponent, SpriteComponent>()
                .Select(data => ((TransformComponent, ISpriteTypeComponent))(data.Item1, data.Item2))
                .Concat(currentScene.ECS.GetComponentsIntersection<TransformComponent, SpriteFontComponent>().Select(data => ((TransformComponent, ISpriteTypeComponent))(data.Item1, data.Item2)))
                .Where((data) => data.Item2.IsVisible)
                .OrderBy((data) => data.Item2.SpriteLayer);


            //var transformAndSpriteFontComponents = currentScene.ECS.GetComponentsIntersection<TransformComponent, SpriteFontComponent>().Select(data => ((TransformComponent, IComponent))(data.Item1, data.Item2));

            foreach ((TransformComponent, ISpriteTypeComponent) tuple in transformAndSpriteComponents)
            {
                if (tuple.Item2 is SpriteComponent spriteComponent)
                { 
                    var transformComp = tuple.Item1;
                    var spriteComp = spriteComponent;

                    if (spriteComp.centered)
                    {
                        spriteComp.position -= (Renderer.GetTextureBounds(spriteComp.textureName) * spriteComp.scale) / 2.0f;
                    }

                    if (!spriteComp.IsUI)
                    {
                        Renderer.Draw(spriteComp.textureName, transformComp.position + spriteComp.position + Arena.Position, spriteComp.sourceRectangle, spriteComp.color, (float)transformComp.rotation + spriteComp.rotation, Vector2.Zero, spriteComp.scale, SpriteEffects.None, spriteComp.Z);
                    }
                    else
                    {
                        Renderer.DrawUI(spriteComp.textureName, transformComp.position + spriteComp.position);
                    }
                }
                else if (tuple.Item2 is SpriteFontComponent spriteFontComponent)
                {
                    var transformComp = tuple.Item1;
                    var spriteFontComp = spriteFontComponent;

                    if (spriteFontComp.centered)
                    {
                        spriteFontComp.position -= (Renderer.GetStringBounds(spriteFontComp.fontPath, spriteFontComp.text) * spriteFontComp.scale) / 2.0f;
                    }

                    Renderer.DrawFont(spriteFontComp.fontPath, spriteFontComp.text, transformComp.position + spriteFontComp.position, spriteFontComp.color, (float)transformComp.rotation + spriteFontComp.rotation, Vector2.Zero, spriteFontComp.scale, SpriteEffects.None, spriteFontComp.Z);
                }
            }
        }
    }
}
