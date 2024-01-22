using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace MeteoricExpansion.Utility
{
    class ModConfig
    {
        private MeteoricExpansionConfig config;

        public void ReadConfig(ICoreAPI api)
        {
            try
            {
                config = LoadConfig(api);

                if (config == null)
                {
                    GenerateConfig(api);
                    config = LoadConfig(api);
                }
                else
                {
                    GenerateConfig(api, config);
                }
            }
            catch
            {
                GenerateConfig(api);
                config = LoadConfig(api);
            }

            api.World.Config.SetBool("Destructive", config.Destructive);
            api.World.Config.SetBool("ClaimsProtected", config.ClaimsProtected);
            api.World.Config.SetBool("DisableFallingMeteors", config.DisableFallingMeteors);
            api.World.Config.SetBool("DisableShowers", config.DisableShowers);
            api.World.Config.SetDouble("FallingMeteorSize", config.FallingMeteorSize);
            api.World.Config.SetInt("FallingMeteorSizeVariance", config.FallingMeteorSizeVariance);
            api.World.Config.SetInt("MinimumMeteorHorizontalSpeed", config.MinimumMeteorHorizontalSpeed);
            api.World.Config.SetInt("MaximumMeteorHorizontalSpeed", config.MaximumMeteorHorizontalSpeed);
            api.World.Config.SetInt("MinimumMeteorVerticalSpeed", config.MinimumMeteorVerticalSpeed);
            api.World.Config.SetInt("MaximumMeteorVerticalSpeed", config.MaximumMeteorVerticalSpeed);
            api.World.Config.SetInt("MinimumMinutesBetweenMeteorSpawns", config.MinimumMinutesBetweenMeteorSpawns);
            api.World.Config.SetInt("MaximumMinutesBetweenMeteorSpawns", config.MaximumMinutesBetweenMeteorSpawns);
            api.World.Config.SetInt("MinimumSpawnDistanceInChunks", config.MinimumSpawnDistanceInChunks);
            api.World.Config.SetInt("MaximumSpawnDistanceInChunks", config.MaximumSpawnDistanceInChunks);
            api.World.Config.SetInt("MinimumMeteorLifespanInSeconds", config.MinimumMeteorLifespanInSeconds);
            api.World.Config.SetInt("MaximumMeteorLifespanInSeconds", config.MaximumMeteorLifespanInSeconds);
            api.World.Config.SetInt("MinimumCraterSmoulderTimeInMinutes", config.MinimumCraterSmoulderTimeInMinutes);
            api.World.Config.SetInt("MaximumCraterSmoulderTimeInMinutes", config.MaximumCraterSmoulderTimeInMinutes);
            api.World.Config.SetDouble("CraterSizeMultiplier", config.CraterSizeMultiplier);
            api.World.Config.SetInt("MinimumShowerHorizontalSpeed", config.MinimumShowerHorizontalSpeed);
            api.World.Config.SetInt("MaximumShowerHorizontalSpeed", config.MaximumShowerHorizontalSpeed);
            api.World.Config.SetInt("MinimumShowerVerticalSpeed", config.MinimumShowerVerticalSpeed);
            api.World.Config.SetInt("MaximumShowerVerticalSpeed", config.MaximumShowerVerticalSpeed);
            api.World.Config.SetInt("MinimumMinutesBetweenShowers", config.MinimumMinutesBetweenShowers);
            api.World.Config.SetInt("MaximumMinutesBetweenShowers", config.MaximumMinutesBetweenShowers);
            api.World.Config.SetInt("MinimumShowerSpawnDistanceInChunks", config.MinimumShowerSpawnDistanceInChunks);
            api.World.Config.SetInt("MaximumShowerSpawnDistanceInChunks", config.MaximumShowerSpawnDistanceInChunks);
            api.World.Config.SetInt("MinimumShowerDurationInMinutes", config.MinimumShowerDurationInMinutes);
            api.World.Config.SetInt("MaximumShowerDurationInMinutes", config.MaximumShowerDurationInMinutes);
            api.World.Config.SetInt("MaxMeteorsPerShower", config.MaxMeteorsPerShower);
        }
        private MeteoricExpansionConfig LoadConfig(ICoreAPI api)
        {
            return api.LoadModConfig<MeteoricExpansionConfig>("MeteoricExpansionConfig.json");
        }
        private void GenerateConfig(ICoreAPI api)
        {
            api.StoreModConfig<MeteoricExpansionConfig>(new MeteoricExpansionConfig(), "MeteoricExpansionConfig.json");
        }
        private void GenerateConfig(ICoreAPI api, MeteoricExpansionConfig previousConfig)
        {
            api.StoreModConfig<MeteoricExpansionConfig>(new MeteoricExpansionConfig(previousConfig), "MeteoricExpansionConfig.json");
        }
    }
}
