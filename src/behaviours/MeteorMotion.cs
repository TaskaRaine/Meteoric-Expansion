using System;
using Vintagestory.API;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace MeteoricExpansion
{
    class MeteorMotion : EntityBehavior
    {
        Random rand = new Random();
        EntityPos meteorTransforms = new EntityPos();

        Vec3f randRotation;
        Vec3f randTranslation;

        //-- ignoredRotationAxist: 0 = X, 1 = Y, Z = 2 --//
        //-- Ignoring an axis makes the meteors appear to tumble through the sky more naturally --//
        private int ignoredRotationAxis;

        //-- 0 = False, 1 = True --//
        private int isMovingSouth, isMovingEast;

        private int maxSpeed;

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
            meteorTransforms = entity.Pos;

            randRotation = new Vec3f();
            randTranslation = new Vec3f();

            SetRandomValues();
            DetermineMeteorRotation();
            DetermineMeteorTranslation();

            this.entity.ServerPos = CalculateEntityRotation();
        }

        public override void OnGameTick(float deltaTime)
        {
            base.OnGameTick(deltaTime);

            this.entity.ServerPos = CalculateEntityTransform(deltaTime);
        }
        private EntityPos CalculateEntityRotation()
        {
            meteorTransforms.Pitch = CalculateMeteorPitch();
            meteorTransforms.Roll = CalculateMeteorRoll();
            meteorTransforms.Yaw = CalculateMeteorYaw();

            return meteorTransforms;
        }

        private EntityPos CalculateEntityTransform(float deltaTime)
        {
            meteorTransforms = CalculateEntityRotation();

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

            maxSpeed = rand.Next(20, 50);
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
            randTranslation.X = rand.Next(0, maxSpeed);
            randTranslation.Y = 0;
            randTranslation.Z = maxSpeed - randTranslation.X;

            if (isMovingEast != 0)
                randTranslation.X *= -1;

            if (isMovingSouth != 0)
                randTranslation.Z *= -1;
        }
    }
}
