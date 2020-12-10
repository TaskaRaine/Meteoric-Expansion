using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace MeteoricExpansion
{
    class MeteorSpawner: ModSystem
    {
        //-- Spawn Time In Minutes --//
        public int minMeteorSpawnTime;
        public int maxMeteorSpawnTime;

        private ICoreServerAPI serverAPI;
        private Random spawnerRand;

        private int spawnerTickIntervalInMilliseconds = 5000;

        private double nextMeteorSpawn;
        private long timeSinceSpawn;

        private long firstTickListener;

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);
            serverAPI = api;

            MeteoricExpansionHelpers.ReadConfig(api);
            InitializeSpawner();

            api.Event.RegisterGameTickListener(onSpawnerTick, spawnerTickIntervalInMilliseconds);
            firstTickListener = api.Event.RegisterGameTickListener(onFirstTick, 1);
        }
        private void InitializeSpawner()
        {
            spawnerRand = new Random(serverAPI.World.Seed);
            timeSinceSpawn = serverAPI.World.ElapsedMilliseconds;

            minMeteorSpawnTime = MeteoricExpansionHelpers.GetMinSpawnTime();
            maxMeteorSpawnTime = MeteoricExpansionHelpers.GetMaxSpawnTime();

            nextMeteorSpawn = spawnerRand.Next(minMeteorSpawnTime, maxMeteorSpawnTime) + spawnerRand.NextDouble();
            nextMeteorSpawn = MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(nextMeteorSpawn);
        }
        private void onFirstTick(float deltaTime)
        {
            MeteoricExpansionHelpers.InitializeHelpers(serverAPI.World.Seed, EntityCodeArray());

            serverAPI.Event.UnregisterGameTickListener(firstTickListener);
        }
        //-- Spawn a meteor made with random rock and metals above the first online player every number of seconds as determined by tickIntervalInSeconds --//
        //-- Eventually spawns will happen between the minMeteorSpawnTime and maxMeteorSpawnTime --// 
        private void onSpawnerTick(float deltaTime)
        {
            if(this.serverAPI.World.ElapsedMilliseconds - timeSinceSpawn > nextMeteorSpawn)
            {
                if (serverAPI.World.AllOnlinePlayers.Length > 0)
                {
                    int playerToSpawnAt = GetSinglePlayer(serverAPI.World.AllOnlinePlayers);

                    serverAPI.World.SpawnEntity(InitEntity(MeteoricExpansionHelpers.SelectRandomMeteor(), playerToSpawnAt));
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
            int minSpawnOffset = MeteoricExpansionHelpers.GetMinSpawnDistance();
            int maxSpawnOffset = MeteoricExpansionHelpers.GetMaxSpawnDistance();

            int negativeRand = spawnerRand.Next(0, 1);
            double spawnOffset = spawnerRand.Next(this.serverAPI.WorldManager.ChunkSize * minSpawnOffset, this.serverAPI.WorldManager.ChunkSize * maxSpawnOffset) + spawnerRand.NextDouble();
            
            if (negativeRand == 0)
                return -spawnOffset;

            return spawnOffset;
        }
        private static bool FindMeteor(EntityProperties meteor)
        {
            if (meteor.Class == "EntityMeteor")
                return true;

            return false;
        }
        private string[] EntityCodeArray()
        {
            List<string> entityCodes = new List<string>();

            List<EntityProperties> entityList = serverAPI.World.EntityTypes.FindAll(FindMeteor);

            foreach (EntityProperties property in entityList)
            {
                entityCodes.Add(property.Code.Path);
            }

            return entityCodes.ToArray();
        }
    }
}
