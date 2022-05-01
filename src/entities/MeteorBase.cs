﻿using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace MeteoricExpansion.Entities
{
    abstract class MeteorBase: Entity
    {
        public override byte[] LightHsv { get; set; } = { 4, 4, 31 };

        protected Random rand;

        protected float CurrentScale { get; set; }
        protected virtual int MinimumScale { get; set; } = 1;
        protected virtual int MaximumScale { get; set; } = 3;

        protected float CurrentLifespan { get; set; }
        protected int MinimumLifespan { get; set; }
        protected int MaximumLifespan { get; set; }

        protected long MeteorSpawnTime { get; set; }
        protected long ElapsedLifetime { get; set; }

        public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
        {
            base.Initialize(properties, api, InChunkIndex3d);

            rand = new Random((int)this.EntityId);

            CurrentScale = (float)rand.NextDouble() + rand.Next(MinimumScale, MaximumScale);

            properties.Client.Size *= CurrentScale;
            properties.CollisionBoxSize *= CurrentScale;

            if (api.Side == EnumAppSide.Server)
            {
                MinimumLifespan = api.World.Config.GetInt("MinimumMeteorLifespanInSeconds");
                MaximumLifespan = api.World.Config.GetInt("MaximumMeteorLifespanInSeconds");

                MeteorSpawnTime = api.World.ElapsedMilliseconds;
                CurrentLifespan = (long)(rand.Next(MinimumLifespan, MaximumLifespan) * CurrentScale + rand.NextDouble()) * 1000;
            }
        }
    }
}
