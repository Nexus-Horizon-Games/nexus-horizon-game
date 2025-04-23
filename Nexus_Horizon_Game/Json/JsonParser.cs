
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Controller.Waves;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Nexus_Horizon_Game.Json
{
    internal static class JsonParser
    {
        static private Dictionary<string, PrefabEntity> ParseEntityTypes(JsonEnvironment env, JArray entityTypes)
        {
            Dictionary<string, PrefabEntity> entityTypesLookup = new();

            foreach (dynamic entityType in entityTypes)
            {
                var prefabEntity = new PrefabEntity(ComponentParser.ParseComponentList(env, entityType.components));

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

            JsonEnvironment env = new JsonEnvironment();

            JsonConstantParser.ParseConstants(env, json.constants);
            env.constants["arena_mid_x"] = Arena.Size.X / 2.0f;
            env.constants["arena_mid_y"] = Arena.Size.Y / 2.0f;

            JArray movementTypes = json.movementTypes;

            var entityTypesLookup = ParseEntityTypes(env, json.entityTypes);

            JArray stages = json.stages;
            Debug.WriteLine($"stages: {stages.Count}");

            int waveIndex = 0;
            foreach (dynamic stage in stages)
            {
                Wave wave = new Wave();
                wave.id = waveIndex;
                wave.duration = JsonHelper.TryParseFloat(env, stage, "duration", defaultValue: float.PositiveInfinity);

                wave.startTime = (double)stage.startTime;

                JToken relativeTo = stage.startTimeRelativeTo;
                if (relativeTo != null)
                {
                    if (relativeTo.Type == JTokenType.String)
                    {
                        var str = (string)relativeTo;

                        switch (str)
                        {
                            case "previous":
                                {
                                    wave.startTimeRelativeTo = waveIndex - 1; // if waveIndex is 0, then startTimeRelativeTo will just become -1 (which is intended)
                                    break;
                                }
                            case "beginning":
                                {
                                    wave.startTimeRelativeTo = -1;
                                    break;
                                }
                        }
                    }
                    else if (relativeTo.Type == JTokenType.Integer)
                    {
                        wave.startTimeRelativeTo = (int)relativeTo;
                    }
                    else
                    {
                        throw new Exception("Invalid startTimeRelativeTo type");
                    }
                }

                wave.endWhenEntitiesDie = JsonHelper.TryParseBool(env, json, "endWhenEntitiesDie", true);

                JArray spawners = stage.spawners;

                foreach (dynamic spawner in spawners)
                {
                    string type = spawner.spawnerType;
                    double startTime = spawner.time;

                    dynamic entityToSpawn = spawner.entity;
                    PrefabEntity entityPrefab = entityTypesLookup[(string)entityToSpawn.entityType].Clone();
                    JObject componentsToSet = entityToSpawn.setComponents;
                    if (componentsToSet != null)
                    {
                        var listOfComponents = ComponentParser.ParseComponentList(env, componentsToSet);

                        foreach (var componentToSet in listOfComponents)
                        {
                            for (int i = 0; i < entityPrefab.Components.Count; i++)
                            {
                                if (entityPrefab.Components[i].GetType() == componentToSet.GetType())
                                {
                                    entityPrefab.Components[i] = componentToSet;
                                }
                            }
                        }
                    }

                    if (type == "multiple")
                    {
                        int entityCount = spawner.entityCount;
                        double interval = spawner.interval;

                        wave.entitiesToSpawn.Enqueue(new PrefabEntity(new List<IComponent>
                        {
                            new BehaviourComponent(new TimedEntitySpanwerBehaviour(entityPrefab, new LoopTimer((float)interval), (float)interval * entityCount)),
                            new TagComponent(Tag.ENEMY) // so that the wave does not end prematurely
                        }), startTime);
                    }
                    else if (type == "single")
                    {
                        wave.entitiesToSpawn.Enqueue(entityPrefab, startTime);
                    }
                }

                waves.Add(wave);
                waveIndex++;
            }

            return waves;
        }
    }
}
