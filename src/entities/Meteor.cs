using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace MeteoricExpansion
{
    class EntityMeteor : Entity
    {
        MeteorProperties meteorProperties;

        float spawnTimeInMilliseconds;
        float elapsedTimeInMilliseconds;
        float maxEntityLifeSpan;

        byte[] lightHsv = new byte[] { 4, 4, 31 };

        public override byte[] LightHsv
        {
            get
            {
                return lightHsv;
            }
        }

        public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
        {
            base.Initialize(properties, api, InChunkIndex3d);

            if (api.Side == EnumAppSide.Server)
            {
                meteorProperties = new MeteorProperties(MeteoricExpansionHelpers.GetRand())
                {
                    metalType = this.FirstCodePart(1),
                    rockType = this.FirstCodePart(2)
                };
                properties.Client.Size *= meteorProperties.scaleMultiplier;
                properties.HitBoxSize *= meteorProperties.scaleMultiplier;

                spawnTimeInMilliseconds = api.World.ElapsedMilliseconds;
                maxEntityLifeSpan = meteorProperties.lifespan;

                properties.Attributes["lightHsv"].AsObject<byte[]>(new byte[] { 4, 4, 31 });
            }
        }
        public override void OnGameTick(float deltaTime)
        {
            base.OnGameTick(deltaTime);

            if (this.Api.Side == EnumAppSide.Server)
            {
                elapsedTimeInMilliseconds = this.Api.World.ElapsedMilliseconds;

                if(elapsedTimeInMilliseconds - spawnTimeInMilliseconds > maxEntityLifeSpan && maxEntityLifeSpan > 0)
                {
                    this.Die();
                }
            }
        }
    }
}
