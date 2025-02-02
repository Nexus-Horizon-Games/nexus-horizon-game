using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Nexus_Horizon_Game.Systems
{
    internal static class RenderSystem
    {
        public static void Draw(GameTime gameTime)
        {
            var transformAndSpriteComponents = GameM.CurrentScene.World.GetComponentsIntersection<TransformComponent, SpriteComponent>().ToList();

            foreach (var tuple in transformAndSpriteComponents)
            {
                var transformComp = tuple.Item1;
                var spriteComp = tuple.Item2;

                if (spriteComp.centered)
                {
                    spriteComp.position -= Renderer.GetTextureBounds(spriteComp.textureName) / 2.0f;
                }

                Renderer.Draw(spriteComp.textureName, transformComp.position + spriteComp.position, spriteComp.sourceRectangle, spriteComp.color, (float)transformComp.rotation + spriteComp.rotation, Vector2.Zero, spriteComp.scale, SpriteEffects.None, spriteComp.Z);
            }
        }
    }
}
