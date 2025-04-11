
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Controller;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Timers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Nexus_Horizon_Game.Json
{
    internal static class JsonParser
    {
        static private Dictionary<string, PrefabEntity> ParseEntityTypes(JArray entityTypes)
        {
            Dictionary<string, PrefabEntity> entityTypesLookup = new();

            foreach (dynamic entityType in entityTypes)
            {
                var prefabEntity = new PrefabEntity(ComponentParser.ParseComponentList(entityType.components));

                entityTypesLookup[(string)entityType.typeNameId] = prefabEntity;
            }

            return entityTypesLookup;
        }

        static public List<Wave> Parse(string levelFilename)
        {
            string jsonString = File.ReadAllText($"Content/Levels/{levelFilename}");
            dynamic json = JsonConvert.DeserializeObject(jsonString);

            List<Wave> waves = new List<Wave>();

            int[] attack = { 1 };
            int[] catattack = { 1, 2 };

            JArray movementTypes = json.movementTypes;

            var entityTypesLookup = ParseEntityTypes(json.entityTypes);

            JArray stages = json.stages;
            Debug.WriteLine($"stages: {stages.Count}");

            foreach (dynamic stage in stages)
            {
                Wave wave = new Wave();
                wave.duration = (double)stage.duration;
                wave.startTime = (double)stage.startTime;

                JArray spawners = stage.spawners;

                foreach (dynamic spawner in spawners)
                {
                    string type = spawner.spawnerType;
                    double startTime = spawner.time;

                    dynamic entityToSpawn = spawner.entity;
                    string entityType = entityToSpawn.entityType;

                    if (type == "multiple")
                    {
                        int entityCount = spawner.entityCount;
                        double interval = spawner.interval;

                        wave.entitiesToSpawn.Enqueue(new PrefabEntity(new List<IComponent>
                        {
                            new BehaviourComponent(new TimedEntitySpanwerBehaviour(entityTypesLookup[entityType], new LoopTimer((float)interval), (float)interval * entityCount))
                        }), startTime);
                    }
                    else if (type == "single")
                    {
                        wave.entitiesToSpawn.Enqueue(entityTypesLookup[entityType], startTime);
                    }
                }

                waves.Add(wave);
            }

            return waves;
        }
    }
}
