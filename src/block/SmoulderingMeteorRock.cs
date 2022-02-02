using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MeteoricExpansion.Blocks
{
    class BlockSmoulderingMeteorRock: BlockMeteorRock
    {
        const float SMOULDERING_DAMAGE = 1.0f;

        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);
        }
        public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
        {
            base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);

            if (entity == null || !entity.Alive)
                return;

            DamageSource damageSource = new DamageSource() { Source = EnumDamageSource.Block, SourceBlock = this, Type = EnumDamageType.Fire };

            entity.ReceiveDamage(damageSource, SMOULDERING_DAMAGE);
        }
    }
}
