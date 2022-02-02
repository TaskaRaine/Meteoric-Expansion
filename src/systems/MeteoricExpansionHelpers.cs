using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MeteoricExpansion
{
    public static class MeteoricExpansionHelpers
    {
        private static string[] meteorCodes;
        private static MeteorConfig meteorConfig;

        private static Random rand;

        public static void InitializeHelpers(int seed, string[] codes)
        {
            rand = new Random(seed);

            meteorCodes = codes;
        }
        public static bool GetConfigDestructive()
        {
            return meteorConfig.Destructive;
        }
        public static bool GetConfigClaimsProtected()
        {
            return meteorConfig.ClaimsProtected;
        }
        public static int GetMinSpawnTime()
        {
            return meteorConfig.MinimumMinutesBetweenMeteorSpawns;
        }
        public static int GetMaxSpawnTime()
        {
            if (meteorConfig.MaximumMinutesBetweenMeteorSpawns <= meteorConfig.MinimumMinutesBetweenMeteorSpawns)
                return meteorConfig.MinimumMinutesBetweenMeteorSpawns + 1;

            return meteorConfig.MaximumMinutesBetweenMeteorSpawns;
        }
        public static int GetMinSpawnDistance()
        {
            return meteorConfig.MinimumSpawnDistanceInChunks;
        }
        public static int GetMaxSpawnDistance()
        {
            if (meteorConfig.MaximumSpawnDistanceInChunks <= meteorConfig.MinimumSpawnDistanceInChunks)
                return meteorConfig.MinimumSpawnDistanceInChunks;

            return meteorConfig.MaximumSpawnDistanceInChunks;
        }
        public static int GetMinLifespan()
        {
            return meteorConfig.MinimumMeteorLifespanInSeconds;
        }
        public static int GetMaxLifespan()
        {
            if (meteorConfig.MaximumMeteorLifespanInSeconds > meteorConfig.MinimumMeteorLifespanInSeconds)
                return meteorConfig.MaximumMeteorLifespanInSeconds;
            else
                return meteorConfig.MinimumMeteorLifespanInSeconds;
        }
        public static int GetMinSmoulderTime()
        {
            return meteorConfig.MinimumCraterSmoulderTimeInMinutes;
        }
        public static int GetMaxSmoulderTime()
        {
            if (meteorConfig.MaximumCraterSmoulderTimeInMinutes > meteorConfig.MinimumCraterSmoulderTimeInMinutes)
                return meteorConfig.MaximumCraterSmoulderTimeInMinutes;
            else
                return meteorConfig.MinimumCraterSmoulderTimeInMinutes;
        }
        public static Random GetRand()
        {
            return rand;
        }

        public static void ReadConfig(ICoreServerAPI api)
        {
            try
            {
                meteorConfig = LoadConfig(api);

                if(meteorConfig == null)
                {
                    GenerateConfig(api);
                    meteorConfig = LoadConfig(api);
                }
                else
                {
                    GenerateConfig(api, meteorConfig);
                }
            }
            catch
            {
                GenerateConfig(api);
                meteorConfig = LoadConfig(api);
            }
        }
        private static MeteorConfig LoadConfig(ICoreServerAPI api)
        {
            return api.LoadModConfig<MeteorConfig>("MeteoricExpansionConfig.json");
        }

        //-- Generate new config with default settings --//
        private static void GenerateConfig(ICoreServerAPI api)
        {
            api.StoreModConfig<MeteorConfig>(new MeteorConfig(), "MeteoricExpansionConfig.json");
        }

        //-- Generate config from previous settings and implement current version config changes --//
        private static void GenerateConfig(ICoreServerAPI api, MeteorConfig previousConfig)
        {
            api.StoreModConfig<MeteorConfig>(new MeteorConfig(previousConfig), "MeteoricExpansionConfig.json");
        }
        public static string SelectRandomMeteor()
        {
            int randIndex = rand.Next(0, meteorCodes.Length);

            return meteorCodes[randIndex]; 
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
        public static Vec3d InvertVector(Vec3d vectorToInvert)
        {
            return new Vec3d(-vectorToInvert.X, -vectorToInvert.Y, -vectorToInvert.Z);
        }
    }
}
