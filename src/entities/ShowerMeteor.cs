using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeteoricExpansion.Entities
{
    class EntityShowerMeteor: MeteorBase
    {
        protected override int MinimumScale { get; set; } = 0;
        protected override int MaximumScale { get; set; } = 1;
    }
}
