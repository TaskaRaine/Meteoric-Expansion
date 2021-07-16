using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace MeteoricExpansion
{
    class RegisterCommands : ModSystem
    {
        public override bool ShouldLoad(EnumAppSide side)
        {
            return side == EnumAppSide.Server;
        }
        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);

            /*
            //-- Registers a command that will spawn a random meteor 10 blocks above the player --//
            api.RegisterCommand("testmeteor", "Spawns a meteor for testing purposes.", "",
            (IServerPlayer player, int groupId, CmdArgs args) =>
            {
                EntityProperties entityType = api.World.GetEntityType(new AssetLocation("meteoricexpansion", MeteoricExpansionHelpers.SelectRandomMeteor()));
                Entity entity = api.World.ClassRegistry.CreateEntity(entityType);
                EntityPos entityPos = new EntityPos(player.Entity.ServerPos.X, api.WorldManager.MapSizeY - 10, player.Entity.ServerPos.Z);

                System.Diagnostics.Debug.WriteLine(MeteoricExpansionHelpers.SelectRandomMeteor());

                entity.ServerPos.SetPos(entityPos);
                entity.Pos.SetFrom(entity.ServerPos);

                api.World.SpawnEntity(entity);

                System.Diagnostics.Debug.WriteLine("Spawned at: " + entity.ServerPos);
                System.Diagnostics.Debug.WriteLine("Player at: " + player.Entity.ServerPos);
                
            }, Privilege.controlserver);

            api.RegisterCommand("testcrater", "Makes a crater below the player.", "",
                (IServerPlayer player, int groupId, CmdArgs args) =>
                {
                    IWorldAccessor world = player.Entity.World;
                    IBlockAccessor blockAccessor = world.GetBlockAccessorBulkUpdate(true, true);

                    Vec3i centerPos = new Vec3i((int)player.Entity.ServerPos.X, (int)player.Entity.ServerPos.Y - 1, (int)player.Entity.ServerPos.Z);
                    BlockPos blockPos = new BlockPos();
                    int craterRadius = 3;

                    blockAccessor.WalkBlocks(new BlockPos(centerPos.X - craterRadius, centerPos.Y - craterRadius, centerPos.Z - craterRadius),
                        new BlockPos(centerPos.X + craterRadius, centerPos.Y + craterRadius, centerPos.Z + craterRadius), (block, bpos) => {
                            if (bpos.DistanceTo(centerPos.ToBlockPos()) < craterRadius)
                            {
                                blockAccessor.SetBlock(0, bpos);
                                blockAccessor.TriggerNeighbourBlockUpdate(bpos);
                            }
                        }, false);

                    blockAccessor.Commit();
                }, Privilege.controlserver);
            */
        }
    }
}
