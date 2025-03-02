using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Transactions;

namespace Nexus_Horizon_Game
{
    internal static class RenderSystem
    {
        public static void Draw(GameTime gameTime, Scene currentScene)
        {
            var transformAndSpriteComponents = currentScene.ECS.GetComponentsIntersection<TransformComponent, SpriteComponent>().Where((data) => data.Item2.IsVisible).OrderBy((data) => data.Item2.SpriteLayer);

            foreach (var tuple in transformAndSpriteComponents)
            {
                var transformComp = tuple.Item1;
                var spriteComp = tuple.Item2;

                if (spriteComp.centered)
                {
                    spriteComp.position -= (Renderer.GetTextureBounds(spriteComp.textureName) * spriteComp.scale) / 2.0f;
                }

                Renderer.Draw(spriteComp.textureName, transformComp.position + spriteComp.position, spriteComp.sourceRectangle, spriteComp.color, (float)transformComp.rotation + spriteComp.rotation, Vector2.Zero, spriteComp.scale, SpriteEffects.None, spriteComp.Z);
            }
        }
    }
}
