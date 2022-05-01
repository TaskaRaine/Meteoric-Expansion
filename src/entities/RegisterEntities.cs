using MeteoricExpansion.Entities;
using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace MeteoricExpansion
{
    class RegisterEntities : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            api.RegisterEntity("EntityFallingMeteor", typeof(EntityFallingMeteor));
            api.RegisterEntity("EntityShowerMeteor", typeof(EntityShowerMeteor));
        }
    }
}
