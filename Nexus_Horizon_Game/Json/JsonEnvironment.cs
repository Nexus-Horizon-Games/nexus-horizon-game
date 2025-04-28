
using Nexus_Horizon_Game.Model.Prefab;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Json
{
    internal class JsonEnvironment
    {
        public Dictionary<string, object> constants = new();
        public Dictionary<string, PrefabEntity> entities = new();
    }
}
