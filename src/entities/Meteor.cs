using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace MeteoricExpansion
{
    class EntityMeteor : Entity
    {
        MeteorProperties meteorProperties;

        public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
        {
            base.Initialize(properties, api, InChunkIndex3d);

            meteorProperties = new MeteorProperties(MeteoricExpansionHelpers.GetRand())
            {
                metalType = this.FirstCodePart(1),
                rockType = this.FirstCodePart(2)
            };

            properties.Client.Size *= meteorProperties.scaleMultiplier;
            properties.HitBoxSize *= meteorProperties.scaleMultiplier;
        }
    }
}
