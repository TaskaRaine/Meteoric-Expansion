namespace MeteoricExpansion
{
    class MeteoricExpansionConfig
    {
        public string FallingMeteorConfigOptions = "The Following Options Affect Falling Meteors";
        public bool DisableFallingMeteors = false;
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

        public string ShowerConfigOptions = "The Following Options Affect Meteor Showers";
        public bool DisableShowers = false;
        public int MinimumMinutesBetweenShowers = 10;
        public int MaximumMinutesBetweenShowers = 30;
        public int MinimumShowerSpawnDistanceInChunks = 1;
        public int MaximumShowerSpawnDistanceInChunks = 6;
        public int MinimumShowerDurationInMinutes = 1;
        public int MaximumShowerDurationInMinutes = 2;
        public int MinimumShowerMeteorSpeed = 20;
        public int MaximumShowerMeteorSpeed = 60;
        public int MaxMeteorsPerShower = 25;

        public MeteoricExpansionConfig()
        {

        }
        public MeteoricExpansionConfig(MeteoricExpansionConfig previousConfig) 
        {
            //-- The following options affect falling meteors --//
            DisableFallingMeteors = previousConfig.DisableFallingMeteors;
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

            //-- The following options affect meteor showers --//
            DisableShowers = previousConfig.DisableShowers;
            MinimumMinutesBetweenShowers = previousConfig.MinimumMinutesBetweenShowers;
            MaximumMinutesBetweenShowers = previousConfig.MaximumMinutesBetweenShowers;
            MinimumShowerSpawnDistanceInChunks = previousConfig.MinimumShowerSpawnDistanceInChunks;
            MaximumShowerSpawnDistanceInChunks = previousConfig.MaximumShowerSpawnDistanceInChunks;
            MinimumShowerDurationInMinutes = previousConfig.MinimumShowerDurationInMinutes;
            MaximumShowerDurationInMinutes = previousConfig.MaximumShowerDurationInMinutes;
            MinimumShowerMeteorSpeed = previousConfig.MinimumShowerMeteorSpeed;
            MaximumShowerMeteorSpeed = previousConfig.MaximumShowerMeteorSpeed;
            MaxMeteorsPerShower = previousConfig.MaxMeteorsPerShower;
        }
    }
}
