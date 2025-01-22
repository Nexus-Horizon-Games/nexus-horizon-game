using System.Numerics;

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
    }
}
