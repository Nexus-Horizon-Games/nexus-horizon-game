using Nexus_Horizon_Game.Components;
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Model.Components.Interfaces;

namespace Nexus_Horizon_Game.Model.Components
{
    internal struct SpriteFontComponent : IComponent, ISpriteTypeComponent
    {
        private bool isEmpty = false;
        public string fontPath = "Fonts/";
        public string text = "";
        public bool isVisible = true;
        public Vector2 position = Vector2.Zero;
        public float rotation = 0.0f;
        public Color color = Color.White;
        public float scale = 1.0f;
        public bool centered = false;
        public uint spriteLayer = 0; // layer order of sprite  (Bring to Front >) (bring to back <)

        public SpriteFontComponent(string fontName = "", string text = "", Color? color = null, float scale = 1.0f, uint spriteLayer = 0, bool centered = false, bool isVisible = true)
        {
            fontPath += fontName;
            this.text = text;
            this.color = color ?? Color.White;
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

        public string Text
        {
            get => this.text;
            set => this.text = value;
        }

        public Color Color
        {
            get => this.color;
            set => this.color = value;
        }

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is SpriteFontComponent o)
            {
                if (fontPath == o.fontPath &&
                    position == o.position &&
                    rotation == o.rotation &&
                    color == o.color &&
                    scale == o.scale &&
                    spriteLayer == o.spriteLayer &&
                    text == o.text)
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
            SpriteFontComponent sprite = new SpriteFontComponent()
            {
                isEmpty = true,
            };
            return sprite;
        }
    }
}
