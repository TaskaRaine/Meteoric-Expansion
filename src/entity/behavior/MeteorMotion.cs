using System;
using System.Threading.Tasks;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MeteoricExpansion
{
    class EntityBehaviorMeteorMotion : EntityBehavior
    {
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

        private AssetLocation meteorSound1 = new AssetLocation("meteoricexpansion", "sounds/effect/meteor_rumble_fade1_double");
        private AssetLocation meteorSound2 = new AssetLocation("meteoricexpansion", "sounds/effect/meteor_rumble_fade2_double");
        private AssetLocation meteorSound3 = new AssetLocation("meteoricexpansion", "sounds/effect/meteor_rumble_fade3_double");

        private int idleSoundLengthInMilliseconds = 2000;
        private long idleSoundStartTime = 0;

        public override string PropertyName()
        {
            return "meteormotion";
        }

        public EntityBehaviorMeteorMotion(Entity entity) : base(entity)
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
        }
        public override void OnGameTick(float deltaTime)
        {
            base.OnGameTick(deltaTime);

            this.entity.ServerPos.SetFrom(CalculateEntityTransform(deltaTime));
            this.entity.Pos.SetFrom(this.entity.ServerPos);

            SpawnMeteorParticles();

            if(this.entity.Api.Side == EnumAppSide.Server)
            {
                if ((this.idleSoundStartTime + idleSoundLengthInMilliseconds) / 2 < this.entity.World.ElapsedMilliseconds)
                {
                    int nextSound = rand.Next(0, 3);

                    if(nextSound == 0)
                        this.entity.World.PlaySoundAt(meteorSound1, this.entity, null, true, 512, 0.62f);
                    else if(nextSound == 1)
                        this.entity.World.PlaySoundAt(meteorSound2, this.entity, null, true, 512, 0.62f);
                    else
                        this.entity.World.PlaySoundAt(meteorSound3, this.entity, null, true, 512, 0.62f);

                    this.idleSoundStartTime = this.entity.World.ElapsedMilliseconds;
                }
            }
        }
        public void SetVerticalSpeed(float speed)
        {
            this.randTranslation.Y = -Math.Abs(speed);
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
