using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace MeteoricExpansion.EntityRenderers
{
    class MeteorRenderer : EntityShapeRenderer
    {
        public MeteorRenderer(Entity entity, ICoreClientAPI api) : base(entity, api)
        {
            this.renderRange = 512;
        }
    }
}
