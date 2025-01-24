using Microsoft.Xna.Framework;
using System.Reflection.Metadata.Ecma335;

namespace Nexus_Horizon_Game.Components
{
    internal struct SpriteComponent : IComponent
    {
        public string textureName = "";
        public Vector2 position = Vector2.Zero;
        public float rotation = 0.0f;
        public Color color = Color.White;
        public float scale = 1.0f;
        public float z = 0.0f; // between 0 and 1
        public Rectangle? sourceRectangle = null; // used to render only a section of an image (for tiles)

        public SpriteComponent(string textureName)
        {
            this.textureName = textureName;
        }

        public SpriteComponent(string textureName, Color color, Rectangle? sourceRectangle = null, float scale = 1.0f, float z = 0.0f)
        {
            this.textureName = textureName;
            this.color = color;
            this.sourceRectangle = sourceRectangle;
            this.scale = scale;
            this.z = z;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is SpriteComponent o)
            {
                if (textureName == o.textureName &&
                    position == o.position &&
                    rotation == o.rotation &&
                    color == o.color &&
                    scale == o.scale &&
                    z == o.z &&
                    sourceRectangle == o.sourceRectangle)
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsEmptyComponent()
        {
            return textureName == "";
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            return new SpriteComponent("");
        }
    }
}
