
using Newtonsoft.Json.Linq;
using Nexus_Horizon_Game.Paths;
using System;

namespace Nexus_Horizon_Game.Json
{
    internal static class JsonConstantParser
    {
        public static LinePath ParseLinePath(JsonEnvironment env, JToken json)
        {
            if (json.Type == JTokenType.String)
            {
                return (LinePath)env.constants[(string)json];
            }
            else if (json.Type == JTokenType.Object)
            {
                var points = (JArray)json["points"];

                return new LinePath(
                    JsonHelper.ParseVector2(env, points[0]),
                    JsonHelper.ParseVector2(env, points[1])
                );
            }

            throw new Exception("Invalid LinePath format");
        }

        public static QuadraticCurvePath ParseQuadraticPath(JsonEnvironment env, JToken json)
        {
            if (json.Type == JTokenType.String)
            {
                return (QuadraticCurvePath)env.constants[(string)json];
            }
            else if (json.Type == JTokenType.Object)
            {
                var points = (JArray)json["points"];

                return new QuadraticCurvePath(
                    JsonHelper.ParseVector2(env, points[0]),
                    JsonHelper.ParseVector2(env, points[1]),
                    JsonHelper.ParseVector2(env, points[2])
                );
            }

            throw new Exception("Invalid QuadraticCurvePath format");
        }

        public static WaitPath ParseWaitPath(JsonEnvironment env, JToken json)
        {
            if (json.Type == JTokenType.String)
            {
                return (WaitPath)env.constants[(string)json];
            }
            else if (json.Type == JTokenType.Object)
            {
                return new WaitPath(
                    JsonHelper.ParseVector2(env, json["point"]),
                    JsonHelper.ParseFloat(env, json["time"])
                );
            }

            throw new Exception("Invalid LinePath format");
        }

        public static MultiPath ParseMultiPath(JsonEnvironment env, JToken json)
        {
            if (json.Type == JTokenType.String)
            {
                return (MultiPath)env.constants[(string)json];
            }
            else if (json.Type == JTokenType.Object)
            {
                var paths = (JArray)json["paths"];
                var multiPath = new MultiPath();

                foreach (JObject path in paths)
                {
                    switch ((string)path["type"])
                    {
                        case "LinePath":
                            {
                                multiPath.AddPath(ParseLinePath(env, path));
                                break;
                            }
                        case "QuadraticPath":
                            {
                                multiPath.AddPath(ParseQuadraticPath(env, path));
                                break;
                            }
                        case "WaitPath":
                            {
                                multiPath.AddPath(ParseWaitPath(env, path));
                                break;
                            }
                        case "MultiPath":
                            {
                                multiPath.AddPath(ParseMultiPath(env, path));
                                break;
                            }
                    }
                }

                return multiPath;
            }

            throw new Exception("Invalid MultiPath format");
        }

        public static object ParseObject(JsonEnvironment env, JToken json, string type)
        {
            switch (type)
            {
                case "float":
                    {
                        return JsonHelper.ParseFloat(env, json);
                    }
                case "LinePath":
                    {
                        return ParseLinePath(env, json);
                    }
                case "QuadraticPath":
                    {
                        return ParseQuadraticPath(env, json);
                    }
                case "MultiPath":
                    {
                        return ParseMultiPath(env, json);
                    }
            }

            throw new Exception("Unrecognized type");
        }

        public static object ParseObject(JsonEnvironment env, JToken json)
        {
            if (json.Type == JTokenType.String)
            {
                if (env.constants.TryGetValue((string)json, out var value))
                {
                    return value;
                }
            }
            else if (json.Type == JTokenType.Object)
            {
                string type = (string)json["type"];
                return ParseObject(env, json, type);
            }

            throw new Exception("Bad format");
        }

        public static void ParseConstants(JsonEnvironment env, JArray json)
        {
            foreach (JToken jsonConstant in json)
            {
                JToken value;
                string type;
                string name;

                if (jsonConstant.Type == JTokenType.Array)
                {
                    var defintionArray = (JArray)jsonConstant;

                    type = (string)defintionArray[0];
                    name = (string)defintionArray[1];
                    value = defintionArray[2];
                }
                else if (jsonConstant.Type == JTokenType.Object)
                {
                    var definitionObject = (JObject)jsonConstant;

                    type = (string)definitionObject["type"];
                    name = (string)definitionObject["name"];
                    value = definitionObject;
                }
                else
                {
                    throw new Exception("Invalid constant format");
                }

                env.constants[name] = ParseObject(env, value, type);
            }
        }
    }
}
