using MeteoricExpansion.Entities;
using MeteoricExpansion.Entities.Behaviors;
using MeteoricExpansion.Utility;
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
            api.RegisterCommand("fallingmeteor", "Spawns a meteor for testing purposes.", "",
            (IServerPlayer player, int groupId, CmdArgs args) =>
            {
                EntityProperties entityType = api.World.GetEntityType(new AssetLocation("meteoricexpansion", "meteor-bismuthinite-andesite"));
                EntityFallingMeteor entity = (EntityFallingMeteor)api.World.ClassRegistry.CreateEntity(entityType);
                EntityPos entityPos = new EntityPos(player.Entity.ServerPos.X, api.WorldManager.MapSizeY - 10, player.Entity.ServerPos.Z);

                entity.ServerPos.SetPos(entityPos);
                entity.Pos.SetFrom(entity.ServerPos);

                api.World.SpawnEntity(entity);

                System.Diagnostics.Debug.WriteLine("Spawned at: " + entity.ServerPos);
                System.Diagnostics.Debug.WriteLine("Player at: " + player.Entity.ServerPos);
                
            }, Privilege.controlserver);
            //-- Registers a command that will spawn a random meteor 10 blocks above the player --//
            api.RegisterCommand("showermeteor", "Spawns a meteor for testing purposes.", "",
            (IServerPlayer player, int groupId, CmdArgs args) =>
            {
                EntityProperties entityType = api.World.GetEntityType(new AssetLocation("meteoricexpansion", "showermeteor-" + args[0]));
                EntityShowerMeteor entity = (EntityShowerMeteor)api.World.ClassRegistry.CreateEntity(entityType);
                EntityPos entityPos = new EntityPos(player.Entity.ServerPos.X, api.WorldManager.MapSizeY - 10, player.Entity.ServerPos.Z);

                entity.ServerPos.SetPos(entityPos);
                entity.Pos.SetFrom(entity.ServerPos);

                api.World.SpawnEntity(entity);

                entity.GetBehavior<EntityBehaviorShowerMeteorMotion>().SetMeteorTranslation(new Vec2f(20, 20));

                System.Diagnostics.Debug.WriteLine("Spawned at: " + entity.ServerPos);
                System.Diagnostics.Debug.WriteLine("Player at: " + player.Entity.ServerPos);

            }, Privilege.controlserver);

            api.RegisterCommand("testcrater", "Makes a crater below the player.", "",
                (IServerPlayer player, int groupId, CmdArgs args) =>
                {
                    IWorldAccessor world = player.Entity.World;
                    IBlockAccessor blockAccessor = world.GetBlockAccessorBulkUpdate(true, true);

                    Vec3i centerPos = new Vec3i((int)player.Entity.ServerPos.X, (int)player.Entity.ServerPos.Y - 1, (int)player.Entity.ServerPos.Z);
                    //BlockPos blockPos = new BlockPos();
                    int craterRadius = 3;

                    blockAccessor.WalkBlocks(new BlockPos(centerPos.X - craterRadius, centerPos.Y - craterRadius, centerPos.Z - craterRadius),
                        new BlockPos(centerPos.X + craterRadius, centerPos.Y + craterRadius, centerPos.Z + craterRadius), (block, xPos, yPos, zPos) => {
                            BlockPos blockPos = new BlockPos(xPos, yPos, zPos);

                            if (blockPos.DistanceTo(centerPos.ToBlockPos()) < craterRadius)
                            {
                                blockAccessor.SetBlock(0, blockPos);
                                blockAccessor.TriggerNeighbourBlockUpdate(blockPos);
                            }
                        }, false);

                    blockAccessor.Commit();
                }, Privilege.controlserver);
            */
        }
    }
}
