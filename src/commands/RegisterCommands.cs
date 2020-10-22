using System;
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

            MeteoricExpansionHelpers.InitializeHelpers(api.World.Seed);

            //-- Registers a command that will spawn a random meteor 10 blocks above the player --//
            api.RegisterCommand("testmeteor", "Spawns a meteor for testing purposes.", "",
            (IServerPlayer player, int groupId, CmdArgs args) =>
            {
                string rockType = MeteoricExpansionHelpers.SelectRandomRock();
                string metalType = MeteoricExpansionHelpers.SelectRandomMetal();
                string indexType = MeteoricExpansionHelpers.SelectRandomIndex();

                string meteorCode = "meteor-" + metalType + "-" + rockType + "-" + indexType;

                EntityProperties entityType = api.World.GetEntityType(new AssetLocation("meteoricexpansion", meteorCode));
                Entity entity = api.World.ClassRegistry.CreateEntity(entityType);
                EntityPos entityPos = new EntityPos(player.Entity.ServerPos.X, player.Entity.ServerPos.Y + 10, player.Entity.ServerPos.Z);

                entity.ServerPos.SetPos(entityPos);
                entity.Pos.SetFrom(entity.ServerPos);

                api.World.SpawnEntity(entity);

                System.Diagnostics.Debug.WriteLine("Spawned at: " + entity.ServerPos);
                System.Diagnostics.Debug.WriteLine("Player at: " + player.Entity.ServerPos);
                
            }, Privilege.controlserver);
        }
    }
}
