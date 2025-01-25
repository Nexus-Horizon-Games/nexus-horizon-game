using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nexus_Horizon_Game.Components;
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
                    Renderer.Draw(spriteComp.textureName, transformComp.position + spriteComp.position, spriteComp.sourceRectangle, spriteComp.color, (float)transformComp.rotation + spriteComp.rotation, Vector2.Zero, spriteComp.scale, SpriteEffects.None, spriteComp.z);
                    var transform = world.GetComponentFromEntity<TransformComponent>(entity);
                    var sprite = world.GetComponentFromEntity<SpriteComponent>(entity);

                    spriteBatch.Draw(spriteTexture, transform.position, Color.White);
                }
            }
        }
    }
}
