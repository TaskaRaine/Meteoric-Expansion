using Vintagestory.API.Common;

namespace MeteoricExpansion.BlockEntities
{
    class RegisterBlockEntities: ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            api.RegisterBlockEntityClass("BESmoulderingRock", typeof(BESmoulderingRock));
        }
    }
}
