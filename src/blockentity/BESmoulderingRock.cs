using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace MeteoricExpansion.BlockEntities
{
    class BESmoulderingRock: BlockEntity
    {
        long startedCoolingAt;
        long randomCoolingTime;
        long coolAtTime = -1;

        long coolingCallback;
        long particlesListener;

        SimpleParticleProperties smokeParticles;
        SimpleParticleProperties fireCubeParticles;

        ILoadedSound smoulderSound;

        Random rand;

        ~BESmoulderingRock()
        {
            if(smoulderSound != null)
                smoulderSound.Dispose();
        }
        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);

            rand = new Random(this.Pos.X + this.Pos.Y + this.Pos.Z);

            if (api.Side == EnumAppSide.Server)
            {
                if (coolAtTime == -1)
                {
                    startedCoolingAt = api.World.ElapsedMilliseconds;
                    randomCoolingTime = rand.Next(MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(MeteoricExpansionHelpers.GetMinSmoulderTime()), MeteoricExpansionHelpers.ConvertMinutesToMilliseconds(MeteoricExpansionHelpers.GetMaxSmoulderTime()));

                    coolingCallback = api.World.RegisterCallback(CoolBlock, (int)randomCoolingTime);
                }
                else
                {
                    coolingCallback = api.World.RegisterCallback(CoolBlock, (int)coolAtTime);
                }
            }
            else
            {
                InitializeSmokeParticles();
                InitializeFireParticles();

                smoulderSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams()
                {
                    Location = new AssetLocation("meteoricexpansion", "sounds/block/smoulder.ogg"),
                    ShouldLoop = false,
                    Position = Pos.ToVec3f(),
                    Range = 32.0f,
                    DisposeOnFinish = false,
                    Volume = 1.0f
                });

                particlesListener = api.World.RegisterGameTickListener(SpawnParticles, 1);
            }
        }
        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
        {
            base.FromTreeAttributes(tree, worldAccessForResolve);

            coolAtTime = tree.GetLong("coolAtTime", -1);
        }
        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);

            tree.SetLong("coolAtTime", (startedCoolingAt + randomCoolingTime) - Api.World.ElapsedMilliseconds);
        }

        public override void OnBlockRemoved()
        {
            base.OnBlockRemoved();

            if (Api.Side == EnumAppSide.Server)
                Api.World.UnregisterCallback(coolingCallback);
            else
            {
                Api.World.UnregisterGameTickListener(particlesListener);

                if(smoulderSound != null)
                {
                    smoulderSound.Stop();
                    smoulderSound.Dispose();
                }
            }
        }
        public override void OnBlockUnloaded()
        {
            base.OnBlockUnloaded();

            if (Api.Side == EnumAppSide.Server)
                Api.World.UnregisterCallback(coolingCallback);
            else
            {
                Api.World.UnregisterGameTickListener(particlesListener);

                if(smoulderSound != null)
                {
                    smoulderSound.Stop();
                    smoulderSound.Dispose();
                }
            }
                
        }
        private void InitializeSmokeParticles()
        {
            smokeParticles = new SimpleParticleProperties
            {
                MinPos = new Vec3d(Pos.X, Pos.Y, Pos.Z),
                AddPos = new Vec3d(1, 1, 1),

                MinSize = 0.3f,
                MaxSize = 1.5f,
                SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARINCREASE, 0.5f),

                MinQuantity = 0.15f,

                LifeLength = 0.6f,
                addLifeLength = 2.5f,

                ShouldDieInLiquid = true,

                WithTerrainCollision = true,

                Color = ColorUtil.ColorFromRgba(10, 10, 12, 255),
                RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARINCREASE, 50),
                GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARINCREASE, 50),
                BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARINCREASE, 52),
                OpacityEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255),

                GravityEffect = -0.1f,
                WindAffected = true,
                WindAffectednes = 0.7f,

                RandomVelocityChange = true,

                ParticleModel = EnumParticleModel.Quad
            };
        }
        private void InitializeFireParticles()
        {
           fireCubeParticles = new SimpleParticleProperties
            {
                MinPos = new Vec3d(Pos.X, Pos.Y, Pos.Z),
                AddPos = new Vec3d(1, 1, 1),

                MinSize = 0.2f,
                MaxSize = 0.5f,
                SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 0.2f),

                Color = ColorUtil.ColorFromRgba(255, 255, 255, 255),
                OpacityEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255),
                BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, rand.Next(0, 150)),
                GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, rand.Next(150, 255)),
                RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 255),

                MinQuantity = 0.05f,

                LifeLength = 0.4f,
                addLifeLength = 1.0f,

                ShouldDieInLiquid = true,

                WithTerrainCollision = true,

                MinVelocity = new Vec3f(-2.5f, -1.5f, -2.5f),
                AddVelocity = new Vec3f(5f, 5f, 5f),

                GravityEffect = 0.4f,
                VertexFlags = rand.Next(100, 150),

                ParticleModel = EnumParticleModel.Cube
            };
        }
        private void SpawnParticles(float dt)
        {
            Api.World.SpawnParticles(smokeParticles);
            Api.World.SpawnParticles(fireCubeParticles);

            if(!smoulderSound.IsPlaying)
            {
                int soundRand = rand.Next(0, 100);

                if (soundRand <= 1)
                    smoulderSound.Start();
            }
        }
        private void CoolBlock(float deltaTime)
        {
            AssetLocation cooledRock;

            if(Block.Code.BeginsWith("meteoricexpansion", "meteoricmetallicrock"))
            {
                string rockType = Block.FirstCodePart();
                string rockMineralGrade = Block.FirstCodePart(1);
                string rockMineral = Block.FirstCodePart(2);
                string rockStoneType = Block.FirstCodePart(3);

                cooledRock = new AssetLocation("meteoricexpansion", rockType + "-" + rockMineralGrade + "-" + rockMineral + "-" + rockStoneType + "-cooled");
            }
            else
            {
                string rockType = Block.FirstCodePart();
                string rockStoneType = Block.FirstCodePart(1);

                cooledRock = new AssetLocation("meteoricexpansion", rockType + "-" + rockStoneType + "-cooled");
            }

            Api.World.BlockAccessor.SetBlock(Api.World.GetBlock(cooledRock).Id, Pos);
            Api.World.BlockAccessor.RemoveBlockEntity(Pos);
        }
    
    }
}
