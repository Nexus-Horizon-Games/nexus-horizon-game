using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus_Horizon_Game.Model.Prefab
{
    public interface IPrototype<T>
    {
        T Clone();
    }
}
