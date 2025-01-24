using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    internal struct TransformComponent : IComponent
    {
        public TransformComponent(Vector2 position, double rotation = 0.0)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public Vector2 position = new Vector2(0.0f, 0.0f);
        public double rotation = double.PositiveInfinity;

        /// <inheritdoc/>
        public bool Equals(IComponent other)
        {
            if (other is TransformComponent o)
            {
                if (position == o.position && rotation == o.rotation)
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <inheritdoc/>
        public bool IsEmptyComponent()
        {
            return Equals(MakeEmptyComponent());
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            return new TransformComponent(new Vector2(0.0f, 0.0f), double.NegativeInfinity);
        }
    }
}
