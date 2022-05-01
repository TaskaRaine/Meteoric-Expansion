using Vintagestory.API.Common;

namespace MeteoricExpansion.Utility
{
    class RegisterConfig : ModSystem
    {
        ModConfig config = new ModConfig();

        public override void StartPre(ICoreAPI api)
        {
            base.StartPre(api);

            config.ReadConfig(api);
        }
        public override void Dispose()
        {
            base.Dispose();

            config = null;
        }
    }
}
