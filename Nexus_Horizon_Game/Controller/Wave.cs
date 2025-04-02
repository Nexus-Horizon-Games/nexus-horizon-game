using Nexus_Horizon_Game.Model.Prefab;
using System;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Controller
{
    internal class Wave
    {
        /// <summary>
        /// The entities to spawn where the priority value is the time at which they should spawn
        /// (this time is relative to the time at which this wave started).
        /// </summary>
        public PriorityQueue<PrefabEntity, double> entitiesToSpawn = new();

        /// <summary>
        /// An identifier to be referenced by other waves.
        /// </summary>
        public int id = 0;

        /// <summary>
        /// The time at which this wave should start.
        /// </summary>
        public double startTime = 0.0f;

        /// <summary>
        /// If -1 then start time is relative to the beginning of the game,
        /// else then the start time is relative to the end time of the wave with that id.
        /// </summary>
        public int startTimeRelativeTo = -1;

        /// <summary>
        /// If true, then when all the entities assosiated with this wave die, this wave will end.
        /// </summary>
        public bool endWhenEntitiesDie = true;

        /// <summary>
        /// The duration in seconds of this wave. Will be ignored if <see cref="endWhenEntitiesDie"/> is true.
        /// </summary>
        public double duration;
    }
}
