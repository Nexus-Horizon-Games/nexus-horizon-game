
using Microsoft.Xna.Framework;
using Nexus_Horizon_Game.Components;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nexus_Horizon_Game.Controller
{
    internal class WaveHandler
    {
        private List<Wave> waves;
        private List<Wave> currentWaves = new();
        private Dictionary<int, double> waveEndTimes = new();
        private double startTime = 0.0;

        public WaveHandler()
        {
            this.waves = new List<Wave>();
        }

        public WaveHandler(IEnumerable<Wave> waves)
        {
            this.waves = new List<Wave>(waves);
        }

        public void AddWave(Wave wave)
        {
            this.waves.Add(wave);
        }

        public bool Started { get; set; } = false;

        public void Start(GameTime gameTime)
        {
            startTime = gameTime.TotalGameTime.TotalSeconds;
            Started = true;
        }

        public void Update(GameTime gameTime)
        {
            var elapsedSinceStart = gameTime.TotalGameTime.TotalSeconds - startTime;

            //Debug.WriteLine($"currentWaves: {currentWaves.Count}, waves: {waves.Count}");

            for (int i = waves.Count - 1; i >= 0; i--) // Iterate backwards since we are potentially removing elements
            {
                Wave wave = waves[i];

                double waveStartTime = 0.0;
                if (wave.startTimeRelativeTo == -1)
                {
                    waveStartTime = wave.startTime;
                }
                else                
                {
                    waveStartTime = waveEndTimes[wave.startTimeRelativeTo] + wave.startTime;
                }

                //Debug.WriteLine($"wave start time: {waveStartTime}");

                if (elapsedSinceStart >= waveStartTime)
                {
                    currentWaves.Add(wave);
                    waves.Remove(wave);
                }
            }

            for (int i = currentWaves.Count - 1; i >= 0; i--) // Iterate backwards since we are potentially removing elements
            {
                Wave wave = currentWaves[i];

                double waveStartTime = 0.0;
                if (wave.startTimeRelativeTo == -1)
                {
                    waveStartTime = wave.startTime;
                }
                else
                {
                    waveStartTime = waveEndTimes[wave.startTimeRelativeTo] + wave.startTime;
                }

                if (elapsedSinceStart >= waveStartTime + wave.duration) // the duration of this wave has passed
                {
                    waveEndTimes[wave.id] = waveStartTime + wave.duration;
                    currentWaves.Remove(wave);
                    continue;
                }

                // Update the wave:
                var success = wave.entitiesToSpawn.TryPeek(out var entity, out double spawnTime);

                if (success && elapsedSinceStart >= waveStartTime + spawnTime)
                {
                    wave.entitiesToSpawn.Dequeue();

                    Debug.WriteLine("spawning an entity -----");
                    int entityID = Scene.Loaded.ECS.CreateEntity(entity.Clone()); // spawn the entity

                    // Need to get a following entityID to find it transform to check for collision
                    if (Scene.Loaded.ECS.EntityHasComponent<ColliderComponent>(entityID, out ColliderComponent Collcomponent))
                    {
                        Collcomponent.SetEntityIDFollowingTo(entityID);
                        Scene.Loaded.ECS.SetComponentInEntity<ColliderComponent>(entityID, Collcomponent);
                    }
                }
            }
        }
    }
}
