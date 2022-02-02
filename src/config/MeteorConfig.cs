namespace MeteoricExpansion
{
    class MeteorConfig
    {
        public bool Destructive = false;
        public bool ClaimsProtected = true;
        public int MinimumMinutesBetweenMeteorSpawns = 10;
        public int MaximumMinutesBetweenMeteorSpawns = 30;
        public int MinimumSpawnDistanceInChunks = 1;
        public int MaximumSpawnDistanceInChunks = 6;
        public int MinimumMeteorLifespanInSeconds = 2;
        public int MaximumMeteorLifespanInSeconds = 10;
        public int MinimumCraterSmoulderTimeInMinutes = 2;
        public int MaximumCraterSmoulderTimeInMinutes = 10;

        public MeteorConfig()
        {

        }
        public  MeteorConfig(MeteorConfig previousConfig) 
        {
            Destructive = previousConfig.Destructive;
            ClaimsProtected = previousConfig.ClaimsProtected;
            MinimumMinutesBetweenMeteorSpawns = previousConfig.MinimumMinutesBetweenMeteorSpawns;
            MaximumMinutesBetweenMeteorSpawns = previousConfig.MaximumMinutesBetweenMeteorSpawns;
            MinimumSpawnDistanceInChunks = previousConfig.MinimumSpawnDistanceInChunks;
            MaximumSpawnDistanceInChunks = previousConfig.MaximumSpawnDistanceInChunks;
            MinimumMeteorLifespanInSeconds = previousConfig.MinimumMeteorLifespanInSeconds;
            MaximumMeteorLifespanInSeconds = previousConfig.MaximumMeteorLifespanInSeconds;
            MinimumCraterSmoulderTimeInMinutes = previousConfig.MinimumCraterSmoulderTimeInMinutes;
            MaximumCraterSmoulderTimeInMinutes = previousConfig.MaximumCraterSmoulderTimeInMinutes;
        }
    }
}
