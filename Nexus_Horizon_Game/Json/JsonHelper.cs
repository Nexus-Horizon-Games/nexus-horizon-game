
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Nexus_Horizon_Game.Json
{
    internal static class JsonHelper
    {
        static public float ParseFloat(JsonEnvironment env, JToken json)
        {
            if (json.Type == JTokenType.Float || json.Type == JTokenType.Integer)
            {
                return (float)json;
            }
            else if (json.Type == JTokenType.String)
            {
                var str = (string)json;
                return (float)env.constants[str];
            }
            else if (json.Type == JTokenType.Object)
            {
                var obj = (JObject)json;
                return ParseFloat(env, obj["value"]);
            }

            return 0.0f;
        }

        static public List<float> ParseNumberArray(JsonEnvironment env, JToken json)
        {
            if (json.Type == JTokenType.String)
            {
                var str = (string)json;
                return (List<float>)env.constants[str];
            }
            else if (json.Type == JTokenType.Array)
            {
                var jsonArray = (JArray)json;
                List<float> array = [];

                foreach (var num in jsonArray)
                {
                    array.Add(ParseFloat(env, num));
                }

                return array;
            }

            throw new Exception("Bad format");
        }

        static public List<int> ParseIntegerArray(JsonEnvironment env, JToken json)
        {
            if (json.Type == JTokenType.String)
            {
                var str = (string)json;
                return (List<int>)env.constants[str];
            }
            else if (json.Type == JTokenType.Array)
            {
                var jsonArray = (JArray)json;
                List<int> array = [];

                foreach (var num in jsonArray)
                {
                    array.Add((int)ParseFloat(env, num));
                }

                return array;
            }

            throw new Exception("Bad format");
        }

        static public Vector2 ParseVector2(JsonEnvironment env, JToken json)
        {
            if (json.Type == JTokenType.String)
            {
                return (Vector2)env.constants[(string)json];
            }
            else if (json.Type == JTokenType.Array)
            {
                var array = (JArray)json;

                if (array.Count != 2)
                    throw new Exception("Vector 2 does not have the correct number of values");

                return new Vector2(
                        ParseFloat(env, array[0]),
                        ParseFloat(env, array[1])
                    );
            }
            
            throw new Exception("Invalid Vector2 format");
        }

        static public Vector2? TryParseVector2(JsonEnvironment env, JObject json, string name, Vector2? defaultValue = null, bool required = false)
        {
            if (!json.TryGetValue(name, out JToken vector2))
            {
                if (!required)
                    return defaultValue;
                else
                    throw new Exception("This field is required");
            }

            return ParseVector2(env, vector2);
        }

        static public float TryParseFloat(JsonEnvironment env, JObject json, string name, float defaultValue = 0.0f, bool required = false)
        {
            if (!json.TryGetValue(name, out JToken value))
            {
                if (!required)
                    return defaultValue;
                else
                    throw new Exception("This field is required");
            }

            return ParseFloat(env, value);
        }

        static public string TryParseString(JsonEnvironment env, JObject json, string name, string defaultValue = "", bool required = false)
        {
            if (!json.TryGetValue(name, out JToken value))
            {
                if (!required)
                    return defaultValue;
                else
                    throw new Exception("This field is required");
            }

            if (value.Type != JTokenType.String)
                throw new Exception("Expected a string type");

            return (string)value;
        }

        static public bool TryParseBool(JsonEnvironment env, JObject json, string name, bool defaultValue = false)
        {
            if (!json.TryGetValue(name, out JToken value))
                return defaultValue;

            if (value.Type != JTokenType.Boolean)
                throw new Exception("Expected a boolean type");

            return (bool)value;
        }
    }
}
