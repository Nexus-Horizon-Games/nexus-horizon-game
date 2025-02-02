using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    internal struct SpriteComponent : IComponent
    {
        private bool isEmpty;
        public string textureName = "";
        public Vector2 position = Vector2.Zero;
        public float rotation = 0.0f;
        public Color color = Color.White;
        public float scale = 1.0f;
        public Rectangle? sourceRectangle = null; // used to render only a section of an image (for tiles)
        public bool centered = false;
        public uint spriteLayer; // layer order of sprite

        public SpriteComponent(string textureName, uint spriteLayer = 0, bool centered = false)
        {
            this.isEmpty = false;
            this.textureName = textureName;
            this.spriteLayer = spriteLayer;
            this.centered = centered;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

        public float Z // between 0 and 1
        {
            get
            {
                return this.spriteLayer / uint.MaxValue;
            }
        }

        public uint SpriteLayer
        {
            get => spriteLayer;
            set => spriteLayer = value;
        }

        public SpriteComponent(string textureName, Color color, Rectangle? sourceRectangle = null, float scale = 1.0f, uint spriteLayer = 0)
        {
            this.textureName = textureName;
            this.color = color;
            this.sourceRectangle = sourceRectangle;
            this.scale = scale;
            this.spriteLayer = spriteLayer;
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
                    spriteLayer == o.spriteLayer &&
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
            return isEmpty;
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            SpriteComponent sprite = new SpriteComponent("")
            {
                isEmpty = true,
            };
            return sprite;
        }
    }
}
