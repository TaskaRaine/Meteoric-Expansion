using MeteoricExpansion.Entities;
using MeteoricExpansion.Entities.Behaviors;
using MeteoricExpansion.Utility;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MeteoricExpansion.Systems
{
    class ShowerSpawner : SpawnerBase
    {
        protected override Type EntityTypeToSpawn { get; set; }
        protected override int MinSpawnTime { get; set; }
        protected override int MaxSpawnTime { get; set; }
        protected override int MinSpawnDistance { get; set; }
        protected override int MaxSpawnDistance { get; set; }
        protected override double NextSpawn { get; set; }

        protected int MinShowerSpawnTime { get; set; }
        protected int MaxShowerSpawnTime { get; set; }
        protected double NextShower { get; set; }
        protected int MinShowerDuration { get; set; }
        protected int MaxShowerDuration { get; set; }
        protected Vec3d ShowerTranslation { get; set; }

        protected long[] ShowerCallbacks { get; set; }

        private string MeteorCode { get; set; }
        private bool ConfigDisableShowers { get; set; }

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);

            Initialize(api);
        }
        public override void Dispose()
        {
            if(ConfigDisableShowers == false)
            {
                base.Dispose();

                UnregisterShowerCallbacks();
            }
        }
        protected override void Initialize(ICoreServerAPI sApi)
        {
            ConfigDisableShowers = sApi.World.Config.GetBool("DisableShowers");

            if (ConfigDisableShowers == false)
            {
                base.Initialize(sApi);

                EntityTypeToSpawn = typeof(EntityShowerMeteor);

                SpawnTickListener = ServerAPI.Event.RegisterGameTickListener(OnSpawnerTick, SPAWNER_TICK_INTERVAL);

                MinShowerSpawnTime = ServerAPI.World.Config.GetInt("MinimumMinutesBetweenShowers");
                MaxShowerSpawnTime = ServerAPI.World.Config.GetInt("MaximumMinutesBetweenShowers");

                MinSpawnDistance = ServerAPI.World.Config.GetInt("MinimumShowerSpawnDistanceInChunks");
                MaxSpawnDistance = ServerAPI.World.Config.GetInt("MaximumShowerSpawnDistanceInChunks");

                NextShower = SpawnerRand.Next(MinShowerSpawnTime, MaxShowerSpawnTime) + SpawnerRand.NextDouble();
                NextShower = MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(NextShower);
            }
        }
        protected override void OnSpawnerTick(float deltaTime)
        {
            if (ServerAPI.World.ElapsedMilliseconds - TimeSinceSpawn > NextShower)
            {
                if (ServerAPI.World.AllOnlinePlayers.Length > 0)
                {
                    MeteorCode = GetRandomEntityCode();
                    ShowerTranslation = DetermineTranslation();

                    int numShowerMeteors = SpawnerRand.Next(0, ServerAPI.World.Config.GetInt("MaxMeteorsPerShower"));
                    int showerTime = SpawnerRand.Next(ServerAPI.World.Config.GetInt("MinimumShowerDurationInMinutes"), ServerAPI.World.Config.GetInt("MaximumShowerDurationInMinutes"));

                    UnregisterShowerCallbacks();

                    ShowerCallbacks = new long[numShowerMeteors];

                    for(int i = 0; i < ShowerCallbacks.Length; i++)
                    {
                        double offsetTime = SpawnerRand.NextDouble();

                        ShowerCallbacks[i] = ServerAPI.Event.RegisterCallback(SpawnShowerMeteor, (int)(MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(SpawnerRand.Next(0, showerTime)) + MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(offsetTime)));
                    }

                    NextSpawn = SpawnerRand.Next(MinShowerSpawnTime, MaxShowerSpawnTime) + SpawnerRand.NextDouble();
                    NextSpawn = MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(NextSpawn);

                    TimeSinceSpawn = ServerAPI.World.ElapsedMilliseconds + MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(showerTime);
                }
            }
        }
        private void SpawnShowerMeteor(float deltaTime)
        {
            if (ServerAPI.World.AllOnlinePlayers.Length > 0)
            {
                EntityShowerMeteor meteor = (EntityShowerMeteor)GenerateEntity(MeteorCode);

                ServerAPI.World.SpawnEntity(meteor);

                meteor.GetBehavior<EntityBehaviorShowerMeteorMotion>().DetermineMeteorTranslation(ShowerTranslation, (int)TimeSinceSpawn);
            }
        }

        private void UnregisterShowerCallbacks()
        {
            if (ShowerCallbacks != null)
                foreach (long callback in ShowerCallbacks)
                {
                    ServerAPI.Event.UnregisterCallback(callback);
                }
        }
    }
}
