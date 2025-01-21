using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Components
{
    internal struct TransformComponent : IComponent
    {
        public TransformComponent(Vector2 position, double rotation = 0.0)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public Vector2 position;
        public double rotation;
    }
}
