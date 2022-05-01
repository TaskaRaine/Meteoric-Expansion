using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace MeteoricExpansion.Entities.Behaviors
{
    abstract class EntityBehaviorMeteorMotionBase : EntityBehavior
    {
        protected SimpleParticleProperties MeteorParticles { get; set; } = new SimpleParticleProperties();
        protected Random Rand { get; set; }
        protected EntityPos EntityTransforms { get; set; }
        protected Vec3f RandomTranslation { get; set; } = new Vec3f();
        protected Vec3f RandomRotation { get; set; } = new Vec3f();

        protected bool IsMovingSouth { get; set; }
        protected bool IsMovingEast { get; set; }

        private int RotationAxisToIgnore { get; set; }


        public override string PropertyName()
        {
            return "meteormotion";
        }
        public EntityBehaviorMeteorMotionBase(Entity entity) : base(entity)
        {

        }
        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            InitializeRandomValues();

            EntityTransforms = entity.ServerPos;
            entity.Pos.SetFrom(entity.ServerPos);
        }
        protected void InitializeRandomValues()
        {
            Rand = new Random((int)entity.EntityId);

            IsMovingSouth = Convert.ToBoolean(Rand.Next(0, 2));
            IsMovingEast = Convert.ToBoolean(Rand.Next(0, 2));

            RotationAxisToIgnore = Rand.Next(0, 3);
        }
        public override void OnGameTick(float deltaTime)
        {
            base.OnGameTick(deltaTime);

            entity.ServerPos.SetFrom(EntityTransforms);
            entity.Pos.SetFrom(entity.ServerPos);
        }
        /// <summary>
        /// Default Values: 
        /// Horizontal(15, 50)
        /// Vertical(2, 20)
        /// </summary>
        /// <param name="horizontalSpeed">X = Min, Y = Max</param>
        /// <param name="verticalSpeed">X = Min, Y = Max</param>
        protected void DetermineMeteorTranslation(Vec2i horizontalSpeed, Vec2i verticalSpeed)
        {
            int randomHorizontalSpeed = Rand.Next(horizontalSpeed.X, horizontalSpeed.Y);
            int randomVerticalSpeed = Rand.Next(verticalSpeed.X, verticalSpeed.Y);

            RandomTranslation.X = Rand.Next(0, randomHorizontalSpeed);
            RandomTranslation.Y = -randomVerticalSpeed;
            RandomTranslation.Z = randomHorizontalSpeed - RandomTranslation.X;

            if (IsMovingEast != false)
                RandomTranslation.X *= -1;

            if (IsMovingSouth != false)
                RandomTranslation.Z *= -1;
        }
        /// <summary>
        /// Default Values: (200, 1600)
        /// </summary>
        /// <param name="minRotation"></param>
        /// <param name="maxRotation"></param>
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
        protected abstract void CalculateEntityTransforms(float deltaTime);
        protected abstract void InitializeMeteorParticles();
        protected abstract void UpdateMeteorParticles();
        protected void SpawnMeteorParticles()
        {
            this.entity.World.SpawnParticles(MeteorParticles);
        }
    }
}
