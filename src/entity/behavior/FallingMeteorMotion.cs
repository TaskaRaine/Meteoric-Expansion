﻿using MeteoricExpansion.Utility;
using System;
using System.Threading.Tasks;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MeteoricExpansion.Entities.Behaviors
{
    class EntityBehaviorFallingMeteorMotion : EntityBehaviorMeteorMotionBase
    {
        private AssetLocation[] MeteorSounds { get; } = {
            new AssetLocation("meteoricexpansion", "sounds/effect/meteor_rumble_fade1_double"),
            new AssetLocation("meteoricexpansion", "sounds/effect/meteor_rumble_fade2_double"),
            new AssetLocation("meteoricexpansion", "sounds/effect/meteor_rumble_fade3_double")
        };

        private int IdleSoundLengthInMilliseconds { get; } = 2000;
        private long IdleSoundStartTime { get; set; } = 0;
        private int RotationAxisToIgnore { get; set; }

        public override string PropertyName()
        {
            return "fallingmeteormotion";
        }

        public EntityBehaviorFallingMeteorMotion(Entity entity) : base(entity)
        {
            
        }

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            RotationAxisToIgnore = Rand.Next(0, 3);

            DetermineMeteorTranslation(HorizontalSpeed, VerticalSpeed);
            DetermineMeteorRotation(200, 1600);

            InitializeMeteorParticles();

            //-- The meteor direction is stored as an attribute and used to calculate meteorite placement and shrapnel, since entity.PreviousServerPos is never assigned --//
            entity.Attributes.SetVec3i("direction", new Vec3i((int)(RandomTranslation.X + .5f), (int)(RandomTranslation.Y + .5f), (int)(RandomTranslation.Z + .5f)));
        }
        public override void OnGameTick(float deltaTime)
        {
            CalculateEntityTransforms(deltaTime);

            base.OnGameTick(deltaTime);

            UpdateMeteorParticles();
            SpawnMeteorParticles();

            if(entity.Api.Side == EnumAppSide.Server)
            {
                if ((IdleSoundStartTime + IdleSoundLengthInMilliseconds) / 2 < entity.World.ElapsedMilliseconds)
                {
                    entity.World.PlaySoundAt(MeteorSounds[Rand.Next(0, MeteorSounds.Length)], entity, null, true, 512, 0.62f);

                    IdleSoundStartTime = entity.World.ElapsedMilliseconds;
                }
            }
        }
        public void DetermineMeteorTranslation(MinMaxTuple horizontalSpeed, MinMaxTuple verticalSpeed)
        {
            int randomHorizontalSpeed = Rand.Next(horizontalSpeed.Min, horizontalSpeed.Max);
            int randomVerticalSpeed = Rand.Next(verticalSpeed.Min, verticalSpeed.Max);

            RandomTranslation.X = Rand.Next(0, randomHorizontalSpeed);
            RandomTranslation.Y = -randomVerticalSpeed;
            RandomTranslation.Z = randomHorizontalSpeed - RandomTranslation.X;

            IsMovingSouth = Convert.ToBoolean(Rand.Next(0, 2));
            IsMovingEast = Convert.ToBoolean(Rand.Next(0, 2));

            if (IsMovingEast != false)
                RandomTranslation.X *= -1;

            if (IsMovingSouth != false)
                RandomTranslation.Z *= -1;
        }
        protected void DetermineMeteorRotation(int minRotation, int maxRotation)
        {
            switch (RotationAxisToIgnore)
            {
                case 0:
                    RandomRotation.X = 0;
                    RandomRotation.Y = Rand.Next(minRotation, maxRotation);
                    RandomRotation.Z = Rand.Next(minRotation, maxRotation);
                    break;
                case 1:
                    RandomRotation.X = Rand.Next(minRotation, maxRotation);
                    RandomRotation.Y = 0;
                    RandomRotation.Z = Rand.Next(minRotation, maxRotation);
                    break;
                case 2:
                    RandomRotation.X = Rand.Next(minRotation, maxRotation);
                    RandomRotation.Y = Rand.Next(minRotation, maxRotation);
                    RandomRotation.Z = 0;
                    break;
                default:
                    break;
            }
        }
        protected override void CalculateEntityTransforms(float deltaTime)
        {
            EntityTransforms.Pitch = CalculateMeteorPitch();
            EntityTransforms.Roll = CalculateMeteorRoll();
            EntityTransforms.Yaw = CalculateMeteorYaw();

            EntityTransforms.X += RandomTranslation.X * deltaTime;
            EntityTransforms.Y += RandomTranslation.Y * deltaTime;
            EntityTransforms.Z += RandomTranslation.Z * deltaTime;
        }
        private float CalculateMeteorPitch()
        {
            if (RandomRotation.X > 0)
                return (entity.World.ElapsedMilliseconds / RandomRotation.X) % GameMath.TWOPI;
            else
                return 0;
        }
        private float CalculateMeteorRoll()
        {
            if (RandomRotation.Y > 0)
                return (entity.World.ElapsedMilliseconds / RandomRotation.Y) % GameMath.TWOPI;
            else
                return 0;
        }
        private float CalculateMeteorYaw()
        {
            if (RandomRotation.Z > 0)
                return (entity.World.ElapsedMilliseconds / RandomRotation.Z) % GameMath.TWOPI;
            else
                return 0;
        }

        protected override void InitializeMeteorParticles()
        {
            #region Meteor Particles Options
            MeteorParticles.GravityEffect = -0.01f;
            MeteorParticles.WindAffected = true;

            MeteorParticles.MinSize = 1.0f;
            MeteorParticles.MaxSize = 5.0f;
            MeteorParticles.SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEAR, -2);

            MeteorParticles.MinQuantity = 4;
            MeteorParticles.AddQuantity = 25;

            MeteorParticles.LifeLength = 1.5f;
            MeteorParticles.addLifeLength = 0.5f;

            MeteorParticles.ShouldDieInLiquid = true;

            MeteorParticles.Color = ColorUtil.ColorFromRgba(255, 255, 255, Rand.Next(100, 255));
            MeteorParticles.OpacityEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);
            MeteorParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, Rand.Next(0, 150));
            MeteorParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, Rand.Next(150, 255));
            MeteorParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);

            MeteorParticles.VertexFlags = Rand.Next(150, 255);

            MeteorParticles.ParticleModel = EnumParticleModel.Quad;

            #endregion
            UpdateMeteorParticles();
        }
        protected override void UpdateMeteorParticles()
        {
            MeteorParticles.MinPos = entity.Pos.XYZ + new Vec3d(-entity.Properties.Client.Size / 2, -entity.Properties.Client.Size / 2, -entity.Properties.Client.Size / 2);
            MeteorParticles.AddPos = new Vec3d(entity.Properties.Client.Size, entity.Properties.Client.Size, entity.Properties.Client.Size);

            MeteorParticles.MinVelocity = new Vec3f(RandomTranslation.X * 0.1f, 0, RandomTranslation.Z * 0.1f);
        }
    }
}
