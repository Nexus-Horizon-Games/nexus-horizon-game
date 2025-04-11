
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.Controller;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Model.Entity_Type_Behaviours;
using Nexus_Horizon_Game.Model.Prefab;
using Nexus_Horizon_Game.States;
using Nexus_Horizon_Game.Timers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Nexus_Horizon_Game.Json
{
    internal static class JsonParser
    {
        static private Vector2 TryParseVector2(JObject json, string name, Vector2 defaultValue = default)
        {
            if (!json.TryGetValue(name, out JToken vector2))
                return defaultValue;

            if (vector2.Type != JTokenType.Array)
                throw new Exception("Vector 2 must be represented as a json array");

            var array = (JArray)vector2;
            if (array.Count != 2)
                throw new Exception("Vector 2 does not have the correct number of values");

            return new Vector2((float)array[0], (float)array[1]);
        }

        static private float TryParseFloat(JObject json, string name, float defaultValue = 0.0f)
        {
            if (!json.TryGetValue(name, out JToken value))
                return defaultValue;

            if (value.Type != JTokenType.Float)
                throw new Exception("Expected a floating point type");

            return (float)value;
        }

        static private string TryParseString(JObject json, string name, string defaultValue = "")
        {
            if (!json.TryGetValue(name, out JToken value))
                return defaultValue;

            if (value.Type != JTokenType.String)
                throw new Exception("Expected a string type");

            return (string)value;
        }

        static private TransformComponent ParseTransformComponent(JToken componentJson)
        {
            TransformComponent component = new TransformComponent();

            if (componentJson.Type == JTokenType.String)
            {
                if ((string)componentJson == "default")
                {
                    return component;
                }
                else
                {
                    throw new Exception("Unrecognized component value.");
                }
            }
            else if (componentJson.Type == JTokenType.Object)
            {
                component.position = TryParseVector2((JObject)componentJson, "position", Vector2.Zero);
                component.rotation = TryParseFloat((JObject)componentJson, "rotation", 0.0f);
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        static private PhysicsBody2DComponent ParsePhysicsBody2DComponent(JToken componentJson)
        {
            PhysicsBody2DComponent component = new PhysicsBody2DComponent();

            if (componentJson.Type == JTokenType.String)
            {
                if ((string)componentJson == "default")
                {
                    return component;
                }
                else
                {
                    throw new Exception("Unrecognized component value.");
                }
            }
            else if (componentJson.Type == JTokenType.Object)
            {
                throw new NotImplementedException("Not supported yet!");
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        static private SpriteComponent ParseSpriteComponent(JToken componentJson)
        {
            SpriteComponent component = new SpriteComponent();

            if (componentJson.Type == JTokenType.String)
            {
                if ((string)componentJson == "default")
                {
                    throw new Exception("\"default\" is not a supported value for sprite components.");
                }
                else
                {
                    throw new Exception("Unrecognized component value.");
                }
            }
            else if (componentJson.Type == JTokenType.Object)
            {
                component = new SpriteComponent(TryParseString((JObject)componentJson, "textureName"));
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        static private StateComponent ParseStateComponent(JToken componentJson)
        {
            StateComponent component = new StateComponent(new List<State>());

            if (componentJson.Type == JTokenType.String)
            {
                if ((string)componentJson == "default")
                {
                    throw new Exception("\"default\" is not a supported value for state components.");
                }
                else
                {
                    throw new Exception("Unrecognized component value.");
                }
            }
            else if (componentJson.Type == JTokenType.Object)
            {
                var componentObject = (JObject)componentJson;

                if (!componentObject.TryGetValue("states", out JToken statesJson))
                    throw new Exception("StateCompnent must have a \"states\" property");

                JArray statesArray = (JArray)statesJson;

                foreach (JObject state in statesArray)
                {
                    if (!state.TryGetValue("stateType", out JToken stateType))
                        throw new Exception("Every state must have a \"stateType\" property");

                    string stateTypeString = (string)stateType;

                    if (stateTypeString == "BirdEnemyState")
                    {
                        component.states.Add(new BirdEnemyState(EnemyFactory.sampleBirdPath1(), new[] { 1 }));
                    }
                    else if (stateTypeString == "CatEnemyState")
                    {
                        component.states.Add(new CatEnemyState(EnemyFactory.sampleCatPath1(80), new[] { 1, 2 }));
                    }
                }
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        static private List<IComponent> ParseComponentList(JObject jsonComponents)
        {
            List<IComponent> components = new List<IComponent>();

            if (jsonComponents.ContainsKey("TransformComponent")) components.Add(ParseTransformComponent(jsonComponents.GetValue("TransformComponent")));
            if (jsonComponents.ContainsKey("PhysicsBody2DComponent")) components.Add(ParsePhysicsBody2DComponent(jsonComponents.GetValue("PhysicsBody2DComponent")));
            if (jsonComponents.ContainsKey("SpriteComponent")) components.Add(ParseSpriteComponent(jsonComponents.GetValue("SpriteComponent")));
            if (jsonComponents.ContainsKey("StateComponent")) components.Add(ParseStateComponent(jsonComponents.GetValue("StateComponent")));

            return components;
        }

        static private Dictionary<string, PrefabEntity> ParseEntityTypes(JArray entityTypes)
        {
            Dictionary<string, PrefabEntity> entityTypesLookup = new();

            foreach (dynamic entityType in entityTypes)
            {
                var prefabEntity = new PrefabEntity(ParseComponentList(entityType.components));

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
                    int entityCount = spawner.entityCount;
                    double interval = spawner.interval;

                    dynamic entityToSpawn = spawner.entity;
                    string entityType = entityToSpawn.entityType;

                    wave.entitiesToSpawn.Enqueue(new PrefabEntity(new List<IComponent>
                    {
                        new BehaviourComponent(new TimedEntitySpanwerBehaviour(entityTypesLookup[entityType], new LoopTimer((float)interval), (float)interval * entityCount))
                    }), startTime);

                    /*for (int i = 0; i < entityCount; i++)
                    {
                        wave.entitiesToSpawn.Enqueue(entityTypesLookup[entityType], startTime + i * interval);
                    }*/
                }

                waves.Add(wave);

                break; // TODO: remove
            }

            return waves;
        }
    }
}
