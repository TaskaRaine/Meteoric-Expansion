using Vintagestory.API.Common;

namespace MeteoricExpansion.Entities
{
    class RegisterBehaviours: ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            api.RegisterEntityBehaviorClass("MeteorMotion", typeof(EntityBehaviorMeteorMotion));
            api.RegisterEntityBehaviorClass("MeteorExplosion", typeof(EntityBehaviorMeteorExplosion));
        }
    }
}
