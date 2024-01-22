using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace MeteoricExpansion.Renderers
{
    public class MeteorIlluminationRenderer : IRenderer
    {
        public double RenderOrder => 0.35;
        public int RenderRange => 9999;

        private WeatherSystemClient WeatherSystemClient { get; set; }

        private ICoreClientAPI Capi { get; set; } 
        private AmbientModifier MeteorAmbient { get; set; } = new AmbientModifier().EnsurePopulated();
        private AmbientModifier NormalAmbient { get; set; } = new AmbientModifier().EnsurePopulated();

        private double FlashDurationMS { get; set; } = 1000;
        private long StartTimeSeconds { get; set; }

        public MeteorIlluminationRenderer(ICoreAPI api) 
        {
            StartTimeSeconds = api.World.Calendar.ElapsedSeconds;

            if(api.Side == EnumAppSide.Client)
            {
                WeatherSystemClient = Capi.ModLoader.GetModSystem<WeatherSystemBase>() as WeatherSystemClient;

                NormalAmbient = WeatherSystemClient.BlendedWeatherData.Ambient;
                Capi.Ambient.CurrentModifiers.Add("meteorambient", MeteorAmbient);
            }
        }
        public void Dispose()
        {
            Capi.Ambient.CurrentModifiers.Remove("meteorambient");

            AmbientModifier sunGlowAmb = Capi.Ambient.CurrentModifiers["sunglow"];
            sunGlowAmb.FogColor.Weight = NormalAmbient.FogColor.Weight;
            sunGlowAmb.AmbientColor.Weight = NormalAmbient.AmbientColor.Weight;
        }

        public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
        {
            WeatherDataSnapshot weatherData = WeatherSystemClient.BlendedWeatherData;

            MeteorAmbient.CloudBrightness.Value = 1;
            MeteorAmbient.FogBrightness.Value = 1;

            MeteorAmbient.CloudBrightness.Weight = 1;
            MeteorAmbient.FogBrightness.Weight = 1;

            MeteorAmbient.SceneBrightness.Weight = 1;
            MeteorAmbient.SceneBrightness.Weight = 1;
        }
    }
}
