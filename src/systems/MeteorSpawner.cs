using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace MeteoricExpansion
{
    class MeteorSpawner: ModSystem
    {
        ICoreServerAPI serverAPI;

        int tickIntervalInSeconds = 2000;

        //-- In Minutes --//
        int minMeteorSpawnTime = 1;
        int maxMeteorSpawnTime = 2;

        double nextMeteorSpawn;

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);
            serverAPI = api;

            nextMeteorSpawn = MeteoricExpansionHelpers.GetRand().Next(minMeteorSpawnTime, maxMeteorSpawnTime) + MeteoricExpansionHelpers.GetRand().NextDouble();

            api.Event.RegisterGameTickListener(onMeteorTick, tickIntervalInSeconds);
        }

        //-- Spawn a meteor made with random rock and metals above the first online player every number of seconds as determined by tickIntervalInSeconds --//
        //-- Eventually spawns will happen between the minMeteorSpawnTime and maxMeteorSpawnTime --// 
        private void onMeteorTick(float deltaTime)
        {
            if(serverAPI.World.AllOnlinePlayers.Length > 0)
            {
                string rockType = MeteoricExpansionHelpers.SelectRandomRock();
                string metalType = MeteoricExpansionHelpers.SelectRandomMetal();
                string indexType = MeteoricExpansionHelpers.SelectRandomIndex();

                string meteorCode = "meteor-" + metalType + "-" + rockType + "-" + indexType;

                EntityProperties entityType = serverAPI.World.GetEntityType(new AssetLocation("meteoricexpansion", meteorCode));
                Entity entity = serverAPI.World.ClassRegistry.CreateEntity(entityType);

                EntityPos entityPos = new EntityPos(serverAPI.World.AllOnlinePlayers[0].Entity.ServerPos.X, serverAPI.WorldManager.MapSizeY, serverAPI.World.AllOnlinePlayers[0].Entity.ServerPos.Z);

                entity.ServerPos.SetPos(entityPos);
                entity.Pos.SetFrom(entity.ServerPos);

                serverAPI.World.SpawnEntity(entity);
            }
        }
    }
}
