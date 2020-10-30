using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace MeteoricExpansion
{
    class MeteorSpawner: ModSystem
    {
        //-- Spawn Time In Minutes --//
        public int minMeteorSpawnTime = 0;
        public int maxMeteorSpawnTime = 1;

        private ICoreServerAPI serverAPI;
        private Random spawnerRand;

        private int spawnerTickIntervalInSeconds = 5000;

        private double nextMeteorSpawn;
        private long timeSinceSpawn;

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);
            serverAPI = api;

            InitializeSpawner();

            api.Event.RegisterGameTickListener(onSpawnerTick, spawnerTickIntervalInSeconds);
        }

        private void InitializeSpawner()
        {
            spawnerRand = new Random(serverAPI.World.Seed);
            timeSinceSpawn = serverAPI.World.ElapsedMilliseconds;
            nextMeteorSpawn = spawnerRand.Next(minMeteorSpawnTime, maxMeteorSpawnTime) + spawnerRand.NextDouble();
            nextMeteorSpawn = MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(nextMeteorSpawn);
        }

        //-- Spawn a meteor made with random rock and metals above the first online player every number of seconds as determined by tickIntervalInSeconds --//
        //-- Eventually spawns will happen between the minMeteorSpawnTime and maxMeteorSpawnTime --// 
        private void onSpawnerTick(float deltaTime)
        {
            if(this.serverAPI.World.ElapsedMilliseconds - timeSinceSpawn > nextMeteorSpawn)
            {
                if (serverAPI.World.AllOnlinePlayers.Length > 0)
                {
                    string meteorCode = "meteor-" + MeteoricExpansionHelpers.SelectRandomMetal() + "-" + MeteoricExpansionHelpers.SelectRandomRock() + "-" + MeteoricExpansionHelpers.SelectRandomIndex();

                    int playerToSpawnAt = GetSinglePlayer(serverAPI.World.AllOnlinePlayers);

                    serverAPI.World.SpawnEntity(InitEntity(meteorCode, playerToSpawnAt));
                }

                nextMeteorSpawn = spawnerRand.Next(minMeteorSpawnTime, maxMeteorSpawnTime) + spawnerRand.NextDouble();
                nextMeteorSpawn = MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(nextMeteorSpawn);

                timeSinceSpawn = this.serverAPI.World.ElapsedMilliseconds;
            }
        }

        //-- Fetches the entity type, assigns the entity to an object, sets its position on server and client, then returns the entity object --//
        private Entity InitEntity(string meteorCode, int atPlayer)
        {
            EntityProperties entityType = serverAPI.World.GetEntityType(new AssetLocation("meteoricexpansion", meteorCode));
            Entity entity = serverAPI.World.ClassRegistry.CreateEntity(entityType);

            EntityPos entityPos = new EntityPos(serverAPI.World.AllOnlinePlayers[atPlayer].Entity.ServerPos.X + GetSpawnOffset(), serverAPI.WorldManager.MapSizeY - 10, serverAPI.World.AllOnlinePlayers[atPlayer].Entity.ServerPos.Z + GetSpawnOffset());

            entity.ServerPos.SetPos(entityPos);
            entity.Pos.SetFrom(entity.ServerPos);

            return entity;
        }

        //-- Retreives a single player index from the list of provided players --//
        private int GetSinglePlayer(IPlayer[] players)
        {
            return spawnerRand.Next(0, players.Length);
        }

        //-- Offsets meteor spawn position so that it does not spawn directly above the player --//
        private double GetSpawnOffset()
        {
            int negativeRand = spawnerRand.Next(0, 1);
            double spawnOffset = spawnerRand.Next(this.serverAPI.WorldManager.ChunkSize, this.serverAPI.WorldManager.ChunkSize * 2) + spawnerRand.NextDouble();
            
            if (negativeRand == 0)
                return -spawnOffset;

            return spawnOffset;
        }
    }
}
