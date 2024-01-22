namespace MeteoricExpansion
{
    class MeteoricExpansionConfig
    {
        public string FallingMeteorConfigOptions = "The Following Options Affect Falling Meteors";
        public bool DisableFallingMeteors = false;
        public bool Destructive = false;
        public bool ClaimsProtected = true;
        public double FallingMeteorSize = 2;
        public int FallingMeteorSizeVariance = 3;
        public int MinimumMeteorHorizontalSpeed = 25;
        public int MaximumMeteorHorizontalSpeed = 60;
        public int MinimumMeteorVerticalSpeed = 20;
        public int MaximumMeteorVerticalSpeed = 45;
        public int MinimumMinutesBetweenMeteorSpawns = 10;
        public int MaximumMinutesBetweenMeteorSpawns = 30;
        public int MinimumSpawnDistanceInChunks = 1;
        public int MaximumSpawnDistanceInChunks = 6;
        public int MinimumMeteorLifespanInSeconds = 3;
        public int MaximumMeteorLifespanInSeconds = 10;
        public int MinimumCraterSmoulderTimeInMinutes = 2;
        public int MaximumCraterSmoulderTimeInMinutes = 10;
        public double CraterSizeMultiplier = 1.75;

        public string ShowerConfigOptions = "The Following Options Affect Meteor Showers";
        public bool DisableShowers = false;
        public int MinimumShowerHorizontalSpeed = 80;
        public int MaximumShowerHorizontalSpeed = 120;
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
            FallingMeteorSize = previousConfig.FallingMeteorSize;
            FallingMeteorSizeVariance = previousConfig.FallingMeteorSizeVariance;
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
            CraterSizeMultiplier = previousConfig.CraterSizeMultiplier;

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
