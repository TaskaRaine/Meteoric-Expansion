using MeteoricExpansion.Entities.Behaviors;
using Vintagestory.API.Common;

namespace MeteoricExpansion.Entities
{
    class RegisterBehaviours: ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            api.RegisterEntityBehaviorClass("FallingMeteorMotion", typeof(EntityBehaviorFallingMeteorMotion));
            api.RegisterEntityBehaviorClass("ShowerMeteorMotion", typeof(EntityBehaviorShowerMeteorMotion));
            api.RegisterEntityBehaviorClass("MeteorExplosion", typeof(EntityBehaviorMeteorExplosion));
        }
    }
}
