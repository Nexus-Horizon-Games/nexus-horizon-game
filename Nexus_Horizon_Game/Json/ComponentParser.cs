
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using Nexus_Horizon_Game.Components;
using Nexus_Horizon_Game.EntityFactory;
using Nexus_Horizon_Game.Paths;
using Nexus_Horizon_Game.States;
using System;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Json
{
    internal static class ComponentParser
    {
        static private TransformComponent ParseTransformComponent(JsonEnvironment env, JToken componentJson)
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
                component.position = JsonHelper.TryParseVector2(env, (JObject)componentJson, "position") ?? Vector2.Zero;
                component.rotation = JsonHelper.TryParseFloat(env, (JObject)componentJson, "rotation", 0.0f);
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        static private PhysicsBody2DComponent ParsePhysicsBody2DComponent(JsonEnvironment env, JToken componentJson)
        {
            PhysicsBody2DComponent component = new PhysicsBody2DComponent();

            if (componentJson.Type == JTokenType.String)
            {
                if ((string)componentJson == "default")
                {
                    return component;
                }
                else if ((string)componentJson == "default_with_acceleration")
                {
                    return new PhysicsBody2DComponent(accelerationEnabled: true);
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

        static private SpriteComponent ParseSpriteComponent(JsonEnvironment env, JToken componentJson)
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
                component = new SpriteComponent(JsonHelper.TryParseString(env, (JObject)componentJson, "textureName", required: true));
                component.centered = JsonHelper.TryParseBool(env, (JObject)componentJson, "centered", false);
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        static private State ParseState(JsonEnvironment env, JObject json)
        {
            if (!json.TryGetValue("stateType", out JToken stateType))
                throw new Exception("Every state must have a \"stateType\" property");

            string stateTypeString = (string)stateType;

            switch (stateTypeString)
            {
                case "BirdEnemyState":
                    {
                        var path = JsonConstantParser.ParseMultiPath(env, json["movementPath"]);
                        var attackPaths = JsonHelper.ParseIntegerArray(env, json["attackPaths"]);

                        return new BirdEnemyState(path, attackPaths);
                    }
                case "CatEnemyState":
                    {
                        var path = JsonConstantParser.ParseMultiPath(env, json["movementPath"]);
                        var attackPaths = JsonHelper.ParseIntegerArray(env, json["attackPaths"]);

                        return new CatEnemyState(path, attackPaths);
                    }
                case "ChefBossStage1State":
                    {
                        var timeLength = JsonHelper.TryParseFloat(env, json, "timeLength", required: true);

                        return new ChefBossStage1State(timeLength, Tag.ENEMY_PROJECTILE);
                    }
                case "ChefBossStage2State":
                    {
                        var timeLength = JsonHelper.TryParseFloat(env, json, "timeLength", required: true);

                        return new ChefBossStage2State(timeLength, Tag.ENEMY_PROJECTILE);
                    }
                case "DeathState":
                    {
                        throw new Exception("Not supported yet.");
                    }
                case "GuineaPigBossState":
                    {
                        var timeLength = JsonHelper.TryParseFloat(env, json, "timeLength", required: true);

                        return new GuineaPigBossState(timeLength, Tag.ENEMY_PROJECTILE);
                    }
                case "MoveToPointState":
                    {
                        var stopPoint = JsonHelper.TryParseVector2(env, json, "stopPoint", required: true) ?? Vector2.Zero;
                        var speed = JsonHelper.TryParseFloat(env, json, "speed", required: true);

                        return new MoveToPointState(stopPoint, speed);
                    }
            }

            throw new Exception("Unrecognized state.");
        }

        static private StateComponent ParseStateComponent(JsonEnvironment env, JToken componentJson)
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
                    component.states.Add(ParseState(env, state));
                }
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        static private TagComponent ParseTagComponent(JsonEnvironment env, JToken componentJson)
        {
            TagComponent component = new TagComponent();

            if (componentJson.Type == JTokenType.String)
            {
                if ((string)componentJson == "default")
                {
                    throw new Exception("\"default\" is not a supported value for tag components.");
                }
                else
                {
                    throw new Exception("Unrecognized component value.");
                }
            }
            else if (componentJson.Type == JTokenType.Object)
            {
                var componentObject = (JObject)componentJson;

                if (!componentObject.TryGetValue("tags", out JToken tagsJson))
                    throw new Exception("TagComponent must have a \"tags\" property");

                JArray tagsArray = (JArray)tagsJson;

                foreach (string tag in tagsArray)
                {
                    switch (tag)
                    {
                        case "player": component.Tag |= Tag.PLAYER; break;
                        case "enemy": component.Tag |= Tag.ENEMY; break;
                        case "player_projectile": component.Tag |= Tag.PLAYER_PROJECTILE; break;
                        case "enemy_projectile": component.Tag |= Tag.ENEMY_PROJECTILE; break;
                        case "smallgrunt": component.Tag |= Tag.SMALLGRUNT; break;
                        case "mediumgrunt": component.Tag |= Tag.MEDIUMGRUNT; break;
                        case "halfboss": component.Tag |= Tag.HALFBOSS; break;
                        case "boss": component.Tag |= Tag.BOSS; break;
                        case "powerdrop": component.Tag |= Tag.POWERDROP; break;
                        case "pointdrop": component.Tag |= Tag.POINTDROP; break;
                        default:
                            throw new Exception($"Unrecognized tag: {tag}");
                    }
                }
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        static private ColliderComponent ParseColliderComponent(JsonEnvironment env, JToken componentJson)
        {
            ColliderComponent component;

            if (componentJson.Type == JTokenType.String)
            {
                if ((string)componentJson == "default")
                {
                    throw new Exception("\"default\" is not a supported value for collider components.");
                }
                else
                {
                    throw new Exception("Unrecognized component value.");
                }
            }
            else if (componentJson.Type == JTokenType.Object)
            {
                var size = JsonHelper.TryParseVector2(env, (JObject)componentJson, "size", required: true) ?? Vector2.Zero;
                var position = JsonHelper.TryParseVector2(env, (JObject)componentJson, "position", null);

                component = new ColliderComponent(new Point((int)size.X, (int)size.Y),
                    position == null ? null : new Point((int)position?.X, (int)position?.Y));
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        /// <summary>
        /// Called when an enemy dies.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void SetToDeathState(int entity)
        {
            var stateComp = Scene.Loaded.ECS.GetComponentFromEntity<StateComponent>(entity);

            // Add death state
            stateComp.states.Add(new DeathState(entity));

            // Set transtion from current state to death state
            stateComp.transitionFunction = stateComp.UseDictionaryTransitionFunction;
            stateComp.transitions[stateComp.currentState] = stateComp.states.Count - 1;

            // Stop current state to transition to death state
            stateComp.states[stateComp.currentState].OnStop();

            Scene.Loaded.ECS.SetComponentInEntity(entity, stateComp);
        }

        static private HealthComponent ParseHealthComponent(JsonEnvironment env, JToken componentJson)
        {
            HealthComponent component;

            if (componentJson.Type == JTokenType.String)
            {
                if ((string)componentJson == "default")
                {
                    throw new Exception("\"default\" is not a supported value for health components.");
                }
                else
                {
                    throw new Exception("Unrecognized component value.");
                }
            }
            else if (componentJson.Type == JTokenType.Object)
            {
                var health = JsonHelper.TryParseFloat(env, (JObject)componentJson, "health", required: true);

                component = new HealthComponent(health, (entity) =>
                {
                    SetToDeathState(entity);
                });
            }
            else
            {
                throw new Exception("Unrecognized component value.");
            }

            return component;
        }

        static public List<IComponent> ParseComponentList(JsonEnvironment env, JObject jsonComponents)
        {
            List<IComponent> components = new List<IComponent>();

            if (jsonComponents.ContainsKey("TransformComponent")) components.Add(ParseTransformComponent(env, jsonComponents.GetValue("TransformComponent")));
            if (jsonComponents.ContainsKey("PhysicsBody2DComponent")) components.Add(ParsePhysicsBody2DComponent(env, jsonComponents.GetValue("PhysicsBody2DComponent")));
            if (jsonComponents.ContainsKey("SpriteComponent")) components.Add(ParseSpriteComponent(env, jsonComponents.GetValue("SpriteComponent")));
            if (jsonComponents.ContainsKey("StateComponent")) components.Add(ParseStateComponent(env, jsonComponents.GetValue("StateComponent")));
            if (jsonComponents.ContainsKey("TagComponent")) components.Add(ParseTagComponent(env, jsonComponents.GetValue("TagComponent")));
            if (jsonComponents.ContainsKey("ColliderComponent")) components.Add(ParseColliderComponent(env, jsonComponents.GetValue("ColliderComponent")));
            if (jsonComponents.ContainsKey("HealthComponent")) components.Add(ParseHealthComponent(env, jsonComponents.GetValue("HealthComponent")));

            return components;
        }
    }
}
