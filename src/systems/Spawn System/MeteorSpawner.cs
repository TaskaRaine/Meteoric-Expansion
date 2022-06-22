using System;
using Vintagestory.API.Server;
using MeteoricExpansion.Utility;
using MeteoricExpansion.Entities;

namespace MeteoricExpansion.Systems
{
    class MeteorSpawner: SpawnerBase
    {
        protected override Type EntityTypeToSpawn { get; set; }
        protected override int MinSpawnTime { get; set; }
        protected override int MaxSpawnTime { get; set; }
        protected override int MinSpawnDistance { get; set; }
        protected override int MaxSpawnDistance { get; set; }
        protected override double NextSpawn { get; set; }
        private bool ConfigDisableFallingMeteors { get; set; }

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);

            Initialize(api);
        }
        public override void Dispose()
        {
            if (ConfigDisableFallingMeteors == false)
            {
                base.Dispose();
            }
        }
        protected override void Initialize(ICoreServerAPI sApi)
        {
            ConfigDisableFallingMeteors = sApi.World.Config.GetBool("DisableFallingMeteors");

            if(ConfigDisableFallingMeteors == false)
            {
                base.Initialize(sApi);

                EntityTypeToSpawn = typeof(EntityFallingMeteor);

                SpawnTickListener = ServerAPI.Event.RegisterGameTickListener(OnSpawnerTick, SPAWNER_TICK_INTERVAL);

                MinSpawnTime = ServerAPI.World.Config.GetInt("MinimumMinutesBetweenMeteorSpawns");
                MaxSpawnTime = ServerAPI.World.Config.GetInt("MaximumMinutesBetweenMeteorSpawns");

                MinSpawnDistance = ServerAPI.World.Config.GetInt("MinimumSpawnDistanceInChunks");
                MaxSpawnDistance = ServerAPI.World.Config.GetInt("MaximumSpawnDistanceInChunks");

                NextSpawn = SpawnerRand.Next(MinSpawnTime, MaxSpawnTime) + SpawnerRand.NextDouble();
                NextSpawn = MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(NextSpawn);

                MeteoricExpansionHelpers.InitializeHelpers(ServerAPI.World.Seed);
            }
        }
        //-- Spawn a meteor made with random rock and metals above the first online player every number of seconds as determined by tickIntervalInSeconds --//
        //-- Eventually spawns will happen between the minMeteorSpawnTime and maxMeteorSpawnTime --// 
        protected override void OnSpawnerTick(float deltaTime)
        {
            if(ServerAPI.World.ElapsedMilliseconds - TimeSinceSpawn > NextSpawn)
            {
                if (ServerAPI.World.AllOnlinePlayers.Length > 0)
                {
                    //ServerAPI.World.SpawnEntity(GenerateEntity());
                }

                NextSpawn = SpawnerRand.Next(MinSpawnTime, MaxSpawnTime) + SpawnerRand.NextDouble();
                NextSpawn = MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(NextSpawn);

                TimeSinceSpawn = this.ServerAPI.World.ElapsedMilliseconds;
            }
        }
    }
}
