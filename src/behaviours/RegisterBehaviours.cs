using Vintagestory.API.Common;

namespace MeteoricExpansion
{
    class RegisterBehaviours: ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            api.RegisterEntityBehaviorClass("meteormotion", typeof(MeteorMotion));
        }
    }
}
