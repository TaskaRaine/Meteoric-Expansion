using MeteoricExpansion.Entities.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace MeteoricExpansion.Entities
{
    class EntityShowerMeteor: MeteorBase
    {
        protected override int MinimumScale { get; set; } = 0;
        protected override int MaximumScale { get; set; } = 1;

        public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
        {
            base.Initialize(properties, api, InChunkIndex3d);
        }
    }
}
