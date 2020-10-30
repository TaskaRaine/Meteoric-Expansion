using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MeteoricExpansion
{
    public static class MeteoricExpansionHelpers
    {
        private static List<string> rockOptions = new List<string>();
        private static List<string> metalOptions = new List<string>();

        private static Random rand;

        public static void InitializeHelpers(int seed)
        {
            rand = new Random(seed);

            InitializeOptions("Rocks", new AssetLocation("game:assets/survival/worldproperties/block/rock.json").Path);
            InitializeOptions("Metals", new AssetLocation("game:assets/survival/worldproperties/block/ore-graded.json").Path);
        }
        public static Random GetRand()
        {
            return rand;
        }

        //-- Read all possible options from the file at the provided file path and store them within a list --//
        private static void InitializeOptions(string materialType, string jsonPath)
        {
            using (StreamReader reader = new StreamReader(jsonPath))
            {
                string jsonString = reader.ReadToEnd();
                JsonObject jsonFile = JsonObject.FromJson(jsonString);

                IJEnumerable<JToken> children = jsonFile.Token.Last.Last.Children();

                switch(materialType)
                {
                    case "Rocks":
                        foreach (JToken child in children)
                        {
                            string rockType = child.First.Value<dynamic>().Value;
                            rockOptions.Add(rockType);
                        }
                        break;
                    case "Metals":
                        foreach (JToken child in children)
                        {
                            string metalType = child.First.Value<dynamic>().Value;
                            metalOptions.Add(metalType);
                        }
                        break;
                    default:
                        return;
                }
            }
        }
        public static string SelectRandomRock()
        {
            int randomRockIndex = rand.Next(0, rockOptions.Count);

            return rockOptions[randomRockIndex];
        }
        public static string SelectRandomMetal()
        {
            int randomMetalIndex = rand.Next(0, metalOptions.Count);

            return metalOptions[randomMetalIndex];
        }
        public static string SelectRandomIndex()
        {
            string[] indexArray = { "1", "2", "3" };

            return indexArray[rand.Next(0, 2)];
        }

        //-- Math Helpers --//
        public static int ConvertMinutesToMilliseconds(int minutes)
        {
            return minutes * 60000;
        }
        public static int ConvertMinutesToMilliseconds(float minutes)
        {
            return (int)(minutes * 60000);
        }
        public static int ConvertMinutesToMilliseconds(double minutes)
        {
            return (int)(minutes * 60000);
        }

        public static Vec3d GetRandomVelocityVectorXYZ(int minVelocity, int maxVelocity)
        {
            int isMovingUp = rand.Next(0, 2);
            int isMovingEast = rand.Next(0, 2);
            int isMovingSouth = rand.Next(0, 2);

            Vec3d random3DVelocity = new Vec3d(rand.Next(minVelocity, maxVelocity) + rand.NextDouble() / 2, rand.Next(minVelocity, maxVelocity) + rand.NextDouble() / 2, rand.Next(minVelocity, maxVelocity) + rand.NextDouble() / 2);

            if (isMovingUp != 0)
                random3DVelocity.X *= -1;

            if (isMovingEast != 0)
                random3DVelocity.Y *= -1;

            if (isMovingSouth != 0)
                random3DVelocity.Z *= -1;

            return random3DVelocity;
        }
    }
}
