using MeteoricExpansion.Utility;
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
        protected MinMaxTuple HorizontalSpeed { get; set; } = new MinMaxTuple();
        protected MinMaxTuple VerticalSpeed { get; set; } = new MinMaxTuple();
        protected Vec3f RandomTranslation { get; set; } = new Vec3f();
        protected Vec3f RandomRotation { get; set; } = new Vec3f();

        protected bool IsMovingSouth { get; set; }
        protected bool IsMovingEast { get; set; }


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

            HorizontalSpeed.Min = attributes["horizontalSpeed"]["min"].AsInt();
            HorizontalSpeed.Max = attributes["horizontalSpeed"]["max"].AsInt();

            VerticalSpeed.Min = attributes["verticalSpeed"]["min"].AsInt();
            VerticalSpeed.Max = attributes["verticalSpeed"]["max"].AsInt();

            EntityTransforms = entity.ServerPos;
            entity.Pos.SetFrom(entity.ServerPos);
        }
        protected void InitializeRandomValues()
        {
            Rand = new Random((int)entity.EntityId);
        }
        public override void OnGameTick(float deltaTime)
        {
            base.OnGameTick(deltaTime);

            entity.ServerPos.SetFrom(EntityTransforms);
            entity.Pos.SetFrom(entity.ServerPos);
        }
        protected abstract void CalculateEntityTransforms(float deltaTime);
        protected abstract void InitializeMeteorParticles();
        /// <summary>
        /// Used to update meteor particles each tick. Particle position, velocity, etc. 
        /// </summary>
        protected abstract void UpdateMeteorParticles();
        protected void SpawnMeteorParticles()
        {
            this.entity.World.SpawnParticles(MeteorParticles);
        }
    }
}
