using Microsoft.Xna.Framework;

namespace Nexus_Horizon_Game.Components
{
    internal struct TransformComponent : IComponent
    {
        private bool isEmpty;

        public Vector2 position = new Vector2(0.0f, 0.0f);
        public double rotation = double.PositiveInfinity;

        public TransformComponent(Vector2 position, double rotation = 0.0)
        {
            this.isEmpty = false;
            this.position = position;
            this.rotation = rotation;
        }

        bool IComponent.IsEmpty
        {
            get => isEmpty;
            set => isEmpty = value;
        }

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
            return isEmpty;
        }

        /// <inheritdoc/>
        public static IComponent MakeEmptyComponent()
        {
            TransformComponent transfrom = new TransformComponent(new Vector2(0.0f, 0.0f), double.NegativeInfinity);
            transfrom.isEmpty = true;
            return transfrom;
        }
    }
}
