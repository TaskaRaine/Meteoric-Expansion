using System;
using Vintagestory.API.MathTools;

namespace MeteoricExpansion
{
    class MeteorProperties
    {
        public float scaleMultiplier;
        public string metalRichness;
        public string metalType;
        public string rockType;

        public MeteorProperties(Random rand)
        {
            scaleMultiplier = (float)rand.NextDouble() + rand.Next(1, 3);
            metalRichness = DetermineRichness(rand.Next(0, 99));
        }

        //-- 10% chance for bountiful meteors --//
        //-- 20% chance for rich meteors --//
        //-- 30% chance for medium meteors --//
        //-- 40% chance for poor meteors --//
        private string DetermineRichness(int richnessRand)
        {
            if (richnessRand <= 9)
                return "bountiful";
            else if (richnessRand > 9 && richnessRand <= 29)
                return "rich";
            else if (richnessRand > 29 && richnessRand <= 59)
                return "medium";
            else
                return "poor";
        }
    }
}
