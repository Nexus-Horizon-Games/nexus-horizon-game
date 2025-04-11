
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Nexus_Horizon_Game.Json
{
    internal static class JsonHelper
    {
        static public float ParseFloat(JToken json)
        {
            if (json.Type == JTokenType.Float)
            {
                return (float)json;
            }
            else if (json.Type == JTokenType.String)
            {
                var str = (string)json;
                switch (str)
                {
                    case "arena_mid_x": return Arena.Size.X / 2.0f;
                    case "arena_mid_y": return Arena.Size.Y / 2.0f;
                }
            }

            return 0.0f;
        }

        static public Vector2? TryParseVector2(JObject json, string name, Vector2? defaultValue = null, bool required = false)
        {
            if (!json.TryGetValue(name, out JToken vector2))
            {
                if (!required)
                    return defaultValue;
                else
                    throw new Exception("This field is required");
            }

            if (vector2.Type != JTokenType.Array)
                throw new Exception("Vector 2 must be represented as a json array");

            var array = (JArray)vector2;
            if (array.Count != 2)
                throw new Exception("Vector 2 does not have the correct number of values");

            return new Vector2(ParseFloat(array[0]), ParseFloat(array[1]));
        }

        static public float TryParseFloat(JObject json, string name, float defaultValue = 0.0f, bool required = false)
        {
            if (!json.TryGetValue(name, out JToken value))
            {
                if (!required)
                    return defaultValue;
                else
                    throw new Exception("This field is required");
            }

            if (value.Type != JTokenType.Float && value.Type != JTokenType.Integer)
                throw new Exception("Expected a floating point type");

            return (float)value;
        }

        static public string TryParseString(JObject json, string name, string defaultValue = "", bool required = false)
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

        static public bool TryParseBool(JObject json, string name, bool defaultValue = false)
        {
            if (!json.TryGetValue(name, out JToken value))
                return defaultValue;

            if (value.Type != JTokenType.Boolean)
                throw new Exception("Expected a boolean type");

            return (bool)value;
        }
    }
}
