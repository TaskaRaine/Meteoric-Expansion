using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MeteoricExpansion
{
    class EntityMeteor : Entity
    {
        private Random rand;

        private float currentScale;
        private int minMeteorScale = 1;
        private int maxMeteorScale = 3;

        private float spawnTimeInMilliseconds;
        private float elapsedTimeInMilliseconds;

        private float currentLifespan;
        private int minMeteorLifespan;
        private int maxMeteorLifespan;

        private byte[] lightHsv = new byte[] { 4, 4, 31 };

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

            rand = new Random((int)this.EntityId);

            currentScale = (float)rand.NextDouble() + rand.Next(minMeteorScale, maxMeteorScale);

            properties.Client.Size *= currentScale;
            properties.CollisionBoxSize *= currentScale;

            if (api.Side == EnumAppSide.Server)
            {
                minMeteorLifespan = MeteoricExpansionHelpers.GetMinLifespan();
                maxMeteorLifespan = MeteoricExpansionHelpers.GetMaxLifespan();

                spawnTimeInMilliseconds = api.World.ElapsedMilliseconds;
                currentLifespan = (float)(rand.Next(minMeteorLifespan, maxMeteorLifespan) * currentScale + rand.NextDouble()) * 1000;

                properties.Attributes["lightHsv"].AsObject<byte[]>(new byte[] { 4, 4, 31 });
            }
        }
        public override void OnGameTick(float deltaTime)
        {
            base.OnGameTick(deltaTime);

            if (this.Api.Side == EnumAppSide.Server)
            {
                elapsedTimeInMilliseconds = this.Api.World.ElapsedMilliseconds;

                if(elapsedTimeInMilliseconds - spawnTimeInMilliseconds > currentLifespan)
                {
                    //-- Combusted, in this case, represents the meteor exploding in mid-air --//
                    this.Die(EnumDespawnReason.Combusted);
                }
                else if(this.World.CollisionTester.IsColliding(this.World.BlockAccessor, this.CollisionBox, this.ServerPos.XYZ, false))
                {
                    EnumBlockMaterial material = this.World.BlockAccessor.GetBlock(this.ServerPos.XYZ.AsBlockPos).BlockMaterial;

                    //-- Don't kill the entity on plant matter, causing the meteor to explode. It just looks silly... --//
                    if(material != EnumBlockMaterial.Plant && material != EnumBlockMaterial.Wood && material != EnumBlockMaterial.Leaves && material != EnumBlockMaterial.Air)
                    {
                        //-- Death represents the meteor colliding with terrain and creating a crater --//
                        this.Die(EnumDespawnReason.Death);
                    }
                }
            }
        }
    }
}
