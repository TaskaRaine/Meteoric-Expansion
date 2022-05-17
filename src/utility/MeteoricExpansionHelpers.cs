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

namespace MeteoricExpansion.Utility
{
    public class MinMaxTuple
    {
        private int _min;
        private int _max;

        public int Min { get { return _min; } set { _min = value; } }
        public int Max { get { return _max; } set { _max = value; } }
    }
    public static class MeteoricExpansionHelpers
    {
        private static Random rand;

        public static void InitializeHelpers(int seed)
        {
            if(rand == null)
                rand = new Random(seed);
        }
        public static Random GetRand()
        {
            return rand;
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
