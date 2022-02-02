using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace MeteoricExpansion.Blocks
{
    class BlockMeteorRock: Block
    {
        public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
        {
            if (FirstCodePart() == "meteoricmetallicrock")
            {
                string qualityPart = "ore-grade-" + FirstCodePart(1);
                string orePart = "ore-" + FirstCodePart(2);
                string rockPart = "rock-" + FirstCodePart(3);

                return
                    Lang.Get(qualityPart) + "\n" +
                    Lang.Get("ore-in-rock", Lang.Get(orePart), Lang.Get(rockPart)) + "\n" +
                    base.GetPlacedBlockInfo(world, pos, forPlayer);
            }

            return base.GetPlacedBlockInfo(world, pos, forPlayer);
        }
    }
}
