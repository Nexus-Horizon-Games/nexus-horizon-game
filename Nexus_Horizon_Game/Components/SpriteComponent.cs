using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    internal struct SpriteComponent : IComponent
    {
        private bool isEmpty;
        public string textureName = "";
        public bool isVisible = true;
        public Vector2 position = Vector2.Zero;
        public float rotation = 0.0f;
        public Color color = Color.White;
        public float scale = 1.0f;
        public Rectangle? sourceRectangle = null; // used to render only a section of an image (for tiles)
        public bool centered = false;
        public uint spriteLayer = 0; // layer order of sprite  (Bring to Front >) (bring to back <)

        public SpriteComponent(string textureName, Color? color = null, Rectangle? sourceRectangle = null, float scale = 1.0f, uint spriteLayer = 0, bool centered = false, bool isVisible = true)
        {
            this.textureName = textureName;
            this.color = color ?? Color.White;
            this.sourceRectangle = sourceRectangle;
            this.scale = scale;
            this.spriteLayer = spriteLayer;
            this.centered = centered;
            this.isVisible = isVisible;
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

        public bool IsVisible
        {
            get => this.isVisible;
            set => this.isVisible = value;
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
