namespace MeteoricExpansion
{
    class MeteoricExpansionConfig
    {
        public string FallingMeteorConfigOptions = "The Following Options Affect Falling Meteors";
        public bool DisableFallingMeteors = false;
        public bool Destructive = false;
        public bool ClaimsProtected = true;
        public int MinimumMeteorHorizontalSpeed = 20;
        public int MaximumMeteorHorizontalSpeed = 50;
        public int MinimumMeteorVerticalSpeed = 2;
        public int MaximumMeteorVerticalSpeed = 20;
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
        public int MinimumShowerHorizontalSpeed = 60;
        public int MaximumShowerHorizontalSpeed = 100;
        public int MinimumShowerVerticalSpeed = 0;
        public int MaximumShowerVerticalSpeed = 0;
        public int MinimumMinutesBetweenShowers = 5;
        public int MaximumMinutesBetweenShowers = 45;
        public int MinimumShowerSpawnDistanceInChunks = 0;
        public int MaximumShowerSpawnDistanceInChunks = 6;
        public int MinimumShowerDurationInMinutes = 2;
        public int MaximumShowerDurationInMinutes = 5;
        public int MaxMeteorsPerShower = 100;

        public MeteoricExpansionConfig()
        {

        }
        public MeteoricExpansionConfig(MeteoricExpansionConfig previousConfig) 
        {
            //-- The following options affect falling meteors --//
            DisableFallingMeteors = previousConfig.DisableFallingMeteors;
            Destructive = previousConfig.Destructive;
            ClaimsProtected = previousConfig.ClaimsProtected;
            MinimumMeteorHorizontalSpeed = previousConfig.MinimumMeteorHorizontalSpeed;
            MaximumMeteorHorizontalSpeed = previousConfig.MaximumMeteorHorizontalSpeed;
            MinimumMeteorVerticalSpeed = previousConfig.MinimumMeteorVerticalSpeed;
            MaximumMeteorVerticalSpeed = previousConfig.MaximumMeteorVerticalSpeed;
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
            MinimumShowerHorizontalSpeed = previousConfig.MinimumShowerHorizontalSpeed;
            MaximumShowerHorizontalSpeed = previousConfig.MaximumShowerHorizontalSpeed;
            MinimumShowerVerticalSpeed = previousConfig.MinimumShowerVerticalSpeed;
            MaximumShowerVerticalSpeed = previousConfig.MaximumShowerVerticalSpeed;
            MinimumShowerSpawnDistanceInChunks = previousConfig.MinimumShowerSpawnDistanceInChunks;
            MaximumShowerSpawnDistanceInChunks = previousConfig.MaximumShowerSpawnDistanceInChunks;
            MinimumShowerDurationInMinutes = previousConfig.MinimumShowerDurationInMinutes;
            MaximumShowerDurationInMinutes = previousConfig.MaximumShowerDurationInMinutes;
            MaxMeteorsPerShower = previousConfig.MaxMeteorsPerShower;
        }
    }
}
