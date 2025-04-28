using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Model.Components.Interfaces;

namespace Nexus_Horizon_Game.Components
{
    internal struct SpriteComponent : IComponent, ISpriteTypeComponent
    {
        private bool isEmpty;
        private bool isUI = false;
        public string textureName = "";
        public string sourceTexture = "";
        public bool isVisible = true;
        public Vector2 position = Vector2.Zero;
        public float rotation = 0.0f;
        public Color color = Color.White;
        public float scale = 1.0f;
        public Rectangle? sourceRectangle = null; // used to render only a section of an image (for tiles)
        public bool centered = false;
        public uint spriteLayer = 0; // layer order of sprite  (Bring to Front >) (bring to back <)
        public int animationFrames = 0;
        public int currentFrame = 0;

        public SpriteComponent(string textureName, Color? color = null, Rectangle? sourceRectangle = null, float scale = 1.0f, uint spriteLayer = 0, bool centered = false, bool isVisible = true, bool isUI = false, int animationFrames = 0)
        {
            this.textureName = textureName;
            this.sourceTexture = textureName;
            this.color = color ?? Color.White;
            this.sourceRectangle = sourceRectangle;
            this.scale = scale;
            this.spriteLayer = spriteLayer;
            this.centered = centered;
            this.isVisible = isVisible;
            this.animationFrames = animationFrames;
            this.isUI = isUI;
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

        public float Scale
        {
            get => scale;
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

        public bool IsUI
        {
            get => this.isUI;
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

        public void incrementAnimation()
        {
            if (animationFrames == 0)
            {
                return;
            }
            if (currentFrame < animationFrames) 
            {
                currentFrame++;
                textureName = sourceTexture + currentFrame;
            }
        }

        public void decrementAnimation()
        {
            if (animationFrames == 0)
            {
                return;
            }
            if (currentFrame > 1)
            {
                currentFrame--;
                textureName = sourceTexture + currentFrame;
            }
            else
            {
                textureName = sourceTexture;
            }
        }

        public void incrementAnimationWrap()
        {
            if (animationFrames == 0)
            {
                return;
            }
            if (currentFrame < animationFrames)
            {
                currentFrame++;
                textureName = sourceTexture + currentFrame;
            }
            else
            {
                currentFrame = 0;
                textureName = sourceTexture;
            }
        }
    }
}
