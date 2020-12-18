using System;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MeteoricExpansion
{
    class MeteorMotion : EntityBehavior
    {
        ILoadedSound meteorIdleSound;

        public static SimpleParticleProperties meteorParticles;

        Random rand;
        EntityPos meteorTransforms = new EntityPos();

        Vec3f randRotation;
        Vec3f randTranslation;

        //-- ignoredRotationAxist: 0 = X, 1 = Y, Z = 2 --//
        //-- Ignoring an axis makes the meteors appear to tumble through the sky more naturally --//
        private int ignoredRotationAxis;

        //-- 0 = False, 1 = True --//
        private int isMovingSouth, isMovingEast;

        private readonly int minRotation = 200;
        private readonly int maxRotation = 1600;

        public override string PropertyName()
        {
            return "meteormotion";
        }

        public MeteorMotion(Entity entity) : base(entity)
        {

        }

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            meteorTransforms = entity.ServerPos;

            rand = new Random((int)this.entity.EntityId);                                               //-- Rand uses the entity ID as a seed so that the client and server can be properly synced --//

            randRotation = new Vec3f();
            randTranslation = new Vec3f();

            SetRandomValues();
            DetermineMeteorRotation();
            DetermineMeteorTranslation();

            meteorParticles = new SimpleParticleProperties(1, 1, 1, new Vec3d(), new Vec3d(), new Vec3f(), new Vec3f());

            this.entity.Pos.SetFrom(this.entity.ServerPos);

            if(this.entity.Api.Side == EnumAppSide.Client)
            {
                //-- Creating an idle sound here allows me to control when the sound starts and stops, whereas the JSON idle sound property would continue to play the sound after entity death --//
                meteorIdleSound = ((IClientWorldAccessor)this.entity.Api.World).LoadSound(new SoundParams()
                {
                    Location = new AssetLocation("meteoricexpansion", "sounds/effect/meteor_sizzle"),
                    ShouldLoop = true,
                    Position = this.entity.ServerPos.XYZFloat,
                    DisposeOnFinish = false,
                    Volume = 0.8f,
                    Range = 256,
                });

                meteorIdleSound.Start();
            }
        }
        public override void OnEntityDespawn(EntityDespawnReason despawn)
        {
            if(this.entity.Api.Side == EnumAppSide.Client)
            {
                if (despawn.reason == EnumDespawnReason.OutOfRange)
                    meteorIdleSound.FadeOutAndStop(5.0f);
                else
                    meteorIdleSound.FadeOutAndStop(1.0f);
            }
        }

        public override void OnGameTick(float deltaTime)
        {
            base.OnGameTick(deltaTime);

            this.entity.ServerPos.SetFrom(CalculateEntityTransform(deltaTime));
            this.entity.Pos.SetFrom(this.entity.ServerPos);

            SpawnMeteorParticles();
        }

        private EntityPos CalculateEntityTransform(float deltaTime)
        {
            meteorTransforms.Pitch = CalculateMeteorPitch();
            meteorTransforms.Roll = CalculateMeteorRoll();
            meteorTransforms.Yaw = CalculateMeteorYaw();

            meteorTransforms.X += randTranslation.X * deltaTime;
            meteorTransforms.Y += randTranslation.Y * deltaTime;
            meteorTransforms.Z += randTranslation.Z * deltaTime;

            return meteorTransforms;
        }
        private float CalculateMeteorPitch()
        {
            if (randRotation.X > 0)
                return (entity.World.ElapsedMilliseconds / randRotation.X) % GameMath.TWOPI;
            else
                return 0;
        }
        private float CalculateMeteorRoll()
        {
            if (randRotation.Y > 0)
                return (entity.World.ElapsedMilliseconds / randRotation.Y) % GameMath.TWOPI;
            else
                return 0;
        }
        private float CalculateMeteorYaw()
        {
            if (randRotation.Z > 0)
                return (entity.World.ElapsedMilliseconds / randRotation.Z) % GameMath.TWOPI;
            else
                return 0;
        }
        private void SetRandomValues()
        {
            ignoredRotationAxis = rand.Next(0, 3);

            isMovingSouth = rand.Next(0, 2);
            isMovingEast = rand.Next(0, 2);
        }

        //-- Determine the rotation of the meteor --//
        private void DetermineMeteorRotation()
        {
            switch (ignoredRotationAxis)
            {
                case 0:
                    randRotation.X = 0;
                    randRotation.Y = rand.Next(minRotation, maxRotation);
                    randRotation.Z = rand.Next(minRotation, maxRotation);
                    break;
                case 1:
                    randRotation.X = rand.Next(minRotation, maxRotation);
                    randRotation.Y = 0;
                    randRotation.Z = rand.Next(minRotation, maxRotation);
                    break;
                case 2:
                    randRotation.X = rand.Next(minRotation, maxRotation);
                    randRotation.Y = rand.Next(minRotation, maxRotation);
                    randRotation.Z = 0;
                    break;
                default:
                    break;
            }
        }

        //-- Determine the speed and direction of the meteor --//
        private void DetermineMeteorTranslation()
        {
            int horizontalSpeed = rand.Next(20, 50);

            randTranslation.X = rand.Next(0, horizontalSpeed);
            randTranslation.Y = -rand.Next(0, horizontalSpeed / 2);
            randTranslation.Z = horizontalSpeed - randTranslation.X;

            if (isMovingEast != 0)
                randTranslation.X *= -1;

            if (isMovingSouth != 0)
                randTranslation.Z *= -1;
        }

        private void SpawnMeteorParticles()
        {
            #region Meteor Particles Options
            meteorParticles.MinPos = this.entity.Pos.XYZ + new Vec3d(-this.entity.Properties.Client.Size / 2, -this.entity.Properties.Client.Size / 2, -this.entity.Properties.Client.Size / 2);
            meteorParticles.AddPos = new Vec3d(this.entity.Properties.Client.Size, this.entity.Properties.Client.Size, this.entity.Properties.Client.Size);

            meteorParticles.MinVelocity = new Vec3f(randTranslation.X * 0.1f, 0, randTranslation.Z * 0.1f);

            meteorParticles.GravityEffect = -0.01f;
            meteorParticles.WindAffected = true;

            meteorParticles.MinSize = 1.0f;
            meteorParticles.MaxSize = 5.0f;
            meteorParticles.SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEAR, -2);

            meteorParticles.MinQuantity = 4;
            meteorParticles.AddQuantity = 25;

            meteorParticles.LifeLength = 1.5f;
            meteorParticles.addLifeLength = 0.5f;

            meteorParticles.ShouldDieInLiquid = true;

            meteorParticles.Color = ColorUtil.ColorFromRgba(255, 255, 255, rand.Next(100, 255));
            meteorParticles.OpacityEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);
            meteorParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, rand.Next(0, 150));
            meteorParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, rand.Next(150, 255));
            meteorParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);

            meteorParticles.VertexFlags = rand.Next(150, 255);

            meteorParticles.ParticleModel = EnumParticleModel.Quad;

            #endregion

            this.entity.World.SpawnParticles(meteorParticles);
        }
    }
}
