using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace MeteoricExpansion.Blocks
{
    class RegisterBlocks: ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            api.RegisterBlockClass("MeteorRock", typeof(BlockMeteorRock));
            api.RegisterBlockClass("SmoulderingMeteorRock", typeof(BlockSmoulderingMeteorRock));
        }
    }
}
