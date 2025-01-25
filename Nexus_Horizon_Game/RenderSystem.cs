using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Nexus_Horizon_Game
{
    internal class RenderSystem
    {
        public static void Draw(World world, GameTime gameTime, SpriteBatch spriteBatch, Texture2D spriteTexture) // spriteBatch and spriteTexture being passed here should only be temporary
        {
            var transformComponents = world.GetComponents<TransformComponent>().ToList();
            var spriteComponents = world.GetComponents<SpriteComponent>().ToList();

            var entitiesWithSprites = world.GetEntitiesWithComponent<SpriteComponent>();

            foreach (int entity in entitiesWithSprites)
            {

                if (world.EntityHasComponent<SpriteComponent>(entity))
                {
                    var transform = world.GetComponentFromEntity<TransformComponent>(entity);
                    var sprite = world.GetComponentFromEntity<SpriteComponent>(entity);

                    spriteBatch.Draw(spriteTexture, transform.position, Color.White);
                }
            }
        }
    }
}
