using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace MeteoricExpansion.EntityRenderers
{
    class RegisterEntityRenderer : ModSystem
    {
        public override void StartClientSide(ICoreClientAPI api)
        {
            base.StartClientSide(api);

            api.RegisterEntityRendererClass("MeteorRenderer", typeof(MeteorRenderer));
        }
    }
}
