
using System;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace MeteoricExpansion
{
    class MeteorExplosion : EntityBehavior
    {
        public static SimpleParticleProperties solidExplosionParticles, quadExplosionParticles;

        Random explosionRand;

        public override string PropertyName()
        {
            return "meteorexplosion";
        }
        public MeteorExplosion(Entity entity) : base(entity)
        {
            solidExplosionParticles = new SimpleParticleProperties(1, 1, 1, new Vec3d(), new Vec3d(), new Vec3f(), new Vec3f());
            quadExplosionParticles = new SimpleParticleProperties(1, 1, 1, new Vec3d(), new Vec3d(), new Vec3f(), new Vec3f());

            explosionRand = new Random();
        }

        //-- When an entity with this behaviour attached despawns...spawn explosion particles, play an explosion sound and make it drop items that are in its drops property --//
        public override void OnEntityDespawn(EntityDespawnReason despawn)
        {
            base.OnEntityDespawn(despawn);

            SpawnExplosionParticles();

            this.entity.World.PlaySoundAt(new AssetLocation("meteoricexpansion", "sounds/effect/air_meteor_explosion"), this.entity, null, true, 256, 0.5f);

            foreach (BlockDropItemStack itemStack in entity.Properties.Drops)
                for (int i = 0; i < (int)(entity.Properties.Client.Size * 10); i++)
                {
                    entity.World.SpawnItemEntity(itemStack.ResolvedItemstack, entity.ServerPos.XYZ, MeteoricExpansionHelpers.GetRandomVelocityVectorXYZ(0, 1));
                }
        }
        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);
        }
        private void SpawnExplosionParticles()
        {
            #region Cuboid Particle Options
            Vec3f velocityRand = new Vec3f((float)(explosionRand.Next(0, 5) + explosionRand.NextDouble()), (float)(explosionRand.Next(0, 5) + explosionRand.NextDouble()), (float)(explosionRand.Next(0, 5) + explosionRand.NextDouble()));

            solidExplosionParticles.MinPos = this.entity.Pos.XYZ + new Vec3d(-this.entity.Properties.Client.Size / 2, -this.entity.Properties.Client.Size / 2, -this.entity.Properties.Client.Size / 2);
            solidExplosionParticles.AddPos = new Vec3d(this.entity.Properties.Client.Size, this.entity.Properties.Client.Size, this.entity.Properties.Client.Size);

            solidExplosionParticles.MinVelocity = new Vec3f(-velocityRand.X, -velocityRand.Y, -velocityRand.Z);

            solidExplosionParticles.AddVelocity = velocityRand * 2;

            solidExplosionParticles.GravityEffect = 0.1f;

            solidExplosionParticles.MinSize = 1.0f;
            solidExplosionParticles.MaxSize = 5.0f;
            solidExplosionParticles.SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEAR, -2);

            solidExplosionParticles.MinQuantity = 100;
            solidExplosionParticles.AddQuantity = 50;

            solidExplosionParticles.LifeLength = 5.0f;
            solidExplosionParticles.addLifeLength = 5.0f;

            solidExplosionParticles.ShouldDieInLiquid = true;

            solidExplosionParticles.WithTerrainCollision = true;

            solidExplosionParticles.Color = ColorUtil.ColorFromRgba(255, 255, 255, explosionRand.Next(100, 255));
            solidExplosionParticles.OpacityEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);
            solidExplosionParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, explosionRand.Next(0, 150));
            solidExplosionParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, explosionRand.Next(150, 255));
            solidExplosionParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 250);

            solidExplosionParticles.VertexFlags = explosionRand.Next(150, 255);

            #endregion

            #region Quad Particle Options
            Vec3f quadVelocityRand = new Vec3f((float)(explosionRand.Next(0, 2) + explosionRand.NextDouble()), (float)(explosionRand.Next(0, 2) + explosionRand.NextDouble()), (float)(explosionRand.Next(0, 2) + explosionRand.NextDouble()));

            quadExplosionParticles.MinPos = this.entity.ServerPos.XYZ + new Vec3d(-this.entity.Properties.Client.Size / 2, -this.entity.Properties.Client.Size / 2, -this.entity.Properties.Client.Size / 2);
            quadExplosionParticles.AddPos = new Vec3d(this.entity.Properties.Client.Size, this.entity.Properties.Client.Size, this.entity.Properties.Client.Size);

            quadExplosionParticles.MinVelocity = new Vec3f(-quadVelocityRand.X, -quadVelocityRand.Y, -quadVelocityRand.Z);

            quadExplosionParticles.AddVelocity = quadVelocityRand * 2;

            quadExplosionParticles.GravityEffect = 0.0f;

            quadExplosionParticles.MinSize = 1.0f;
            quadExplosionParticles.MaxSize = 10.0f;
            quadExplosionParticles.SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEAR, -2);

            quadExplosionParticles.MinQuantity = 20;
            quadExplosionParticles.AddQuantity = 25;

            quadExplosionParticles.LifeLength = 1.5f;
            quadExplosionParticles.addLifeLength = .5f;

            quadExplosionParticles.ShouldDieInLiquid = true;

            quadExplosionParticles.Color = ColorUtil.ColorFromRgba(255, 255, 255, explosionRand.Next(100, 255));
            quadExplosionParticles.OpacityEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);
            quadExplosionParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, explosionRand.Next(0, 150));
            quadExplosionParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, explosionRand.Next(150, 255));
            quadExplosionParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);

            quadExplosionParticles.VertexFlags = explosionRand.Next(150, 255);

            quadExplosionParticles.ParticleModel = EnumParticleModel.Quad;
            #endregion

            this.entity.World.SpawnParticles(solidExplosionParticles);
            this.entity.World.SpawnParticles(quadExplosionParticles);

        }
    }
}
