using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace MeteoricExpansion.Entities.Behaviors
{
    /* 
     * Meteor Colors: 
     * Sodium: Bright Yellow
     * Iron: Gold/Orange
     * Magnesium: White
     * Nitrogen: Red
     * Potassium: Violet
     * Copper: Green
     * Lead: Blue
    */
    class EntityBehaviorShowerMeteorMotion : EntityBehaviorMeteorMotionBase
    {

        public override string PropertyName()
        {
            return "showermeteormotion";
        }

        public EntityBehaviorShowerMeteorMotion(Entity entity) : base(entity)
        {

        }
        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            InitializeMeteorParticles();
        }
        public override void OnGameTick(float deltaTime)
        {
            CalculateEntityTransforms(deltaTime);

            base.OnGameTick(deltaTime);

            UpdateMeteorParticles();
            SpawnMeteorParticles();
        }
        protected override void CalculateEntityTransforms(float deltaTime)
        {
            EntityTransforms.X += RandomTranslation.X * deltaTime;
            EntityTransforms.Y = entity.World.BlockAccessor.MapSizeY;
            EntityTransforms.Z += RandomTranslation.Z * deltaTime;
        }

        protected override void InitializeMeteorParticles()
        {
            #region Meteor Particles Options
            MeteorParticles.GravityEffect = -0.01f;
            MeteorParticles.WindAffected = true;

            MeteorParticles.MinSize = .5f;
            MeteorParticles.MaxSize = 2.5f;
            MeteorParticles.SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEAR, -2);

            MeteorParticles.MinQuantity = 2;
            MeteorParticles.AddQuantity = 5;

            MeteorParticles.LifeLength = 1.5f;
            MeteorParticles.addLifeLength = 0.5f;

            MeteorParticles.ShouldDieInLiquid = true;

            DetermineTrailColour();

            MeteorParticles.VertexFlags = Rand.Next(150, 255);

            MeteorParticles.ParticleModel = EnumParticleModel.Quad;

            #endregion
            UpdateMeteorParticles();
        }

        protected override void UpdateMeteorParticles()
        {
            MeteorParticles.MinPos = entity.Pos.XYZ + new Vec3d(-1.5, -1.5, -1.5);
            MeteorParticles.AddPos = new Vec3d(1.5, 1.5, 1.5);

            MeteorParticles.MinVelocity = new Vec3f(RandomTranslation.X * 0.1f, 0, RandomTranslation.Z * 0.1f);
        }
        public void SetMeteorTranslation(Vec2f horizontalVector)
        {
            RandomTranslation.X = horizontalVector.X;
            RandomTranslation.Y = 0;
            RandomTranslation.Z = horizontalVector.Y;
        }
        private void DetermineTrailColour()
        {
            MeteorParticles.OpacityEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);

            switch (entity.LastCodePart())
            {
                //-- Bright Yellow --//
                case "sodium":
                    MeteorParticles.Color = ColorUtil.ColorFromRgba(25, 255, 255, Rand.Next(100, 255));
                    MeteorParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 5);
                    MeteorParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 5);
                    MeteorParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 20);
                    break;
                //-- Gold/Orange --//
                case "iron":
                    MeteorParticles.Color = ColorUtil.ColorFromRgba(30, 200, 255, Rand.Next(100, 255));
                    MeteorParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 20);
                    MeteorParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 20);
                    MeteorParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 5);
                    break;
                //-- White --//
                case "magnesium":
                    MeteorParticles.Color = ColorUtil.ColorFromRgba(255, 255, 255, Rand.Next(100, 255));
                    break;
                //-- Red --//
                case "nitrogen":
                    MeteorParticles.Color = ColorUtil.ColorFromRgba(155, 155, 255, Rand.Next(100, 255));
                    MeteorParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 30);
                    MeteorParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 30);
                    MeteorParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 0);
                    break;
                //-- Violet --//
                case "potassium":
                    MeteorParticles.Color = ColorUtil.ColorFromRgba(255, 200, 255, Rand.Next(100, 255));
                    MeteorParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);
                    MeteorParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 150);
                    MeteorParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, Rand.Next(0, 150));
                    break;
                //-- Green --//
                case "copper":
                    MeteorParticles.Color = ColorUtil.ColorFromRgba(200, 255, 200, Rand.Next(100, 255));
                    MeteorParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);
                    MeteorParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, Rand.Next(0, 150));
                    MeteorParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);
                    break;
                //-- Blue --//
                case "lead":
                    MeteorParticles.Color = ColorUtil.ColorFromRgba(255, 100, 50, Rand.Next(100, 255));
                    MeteorParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 5);
                    MeteorParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 5);
                    MeteorParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 20);
                    break;
                default:
                    MeteorParticles.Color = ColorUtil.ColorFromRgba(255, 255, 255, Rand.Next(100, 255));
                    MeteorParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, Rand.Next(0, 150));
                    MeteorParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, Rand.Next(150, 255));
                    MeteorParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255);

                    break;
            }
        }
    }
}
