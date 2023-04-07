
using MeteoricExpansion.Utility;
using System;
using System.Collections.Generic;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace MeteoricExpansion
{
    class EntityBehaviorMeteorExplosion : EntityBehavior
    {
        ICoreAPI sidedAPI;
        ICoreServerAPI serverAPI;
        IBlockAccessor blockAccessor;

        public SimpleParticleProperties solidExplosionParticles, quadExplosionParticles;

        Random explosionRand;
        DamageSource meteorDamageSource;

        private int explosionRadius;
        private int crackedCraterRadius = 1;
        private int injuryRadius;
        private float injuryBaseDamage = 5.0f;

        private double explosionRadiusModifier = 1.5;
        private double itemStackVelocityModifier = 1.0;
        private double injuryDamageModifier = 1.0;

        private int minMeteorDrops = 5;
        private int maxMeteorDrops = 16;

        //-- Percent Chances --//
        private int terrainDropChance = 5;
        private int meteorImpactResourceChance = 75;
        private int metalResourceChance = 33;
        private int ashBlockChance = 20;

        private bool fillWithLiquid = false;
        private int fillHeight;
        AssetLocation liquidAsset;

        private List<BlockPos> positionsChanged = new List<BlockPos>();

        public override string PropertyName()
        {
            return "meteorexplosion";
        }
        public EntityBehaviorMeteorExplosion(Entity entity) : base(entity)
        {

        }

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            sidedAPI = this.entity.Api;

            explosionRand = new Random();

            quadExplosionParticles = new SimpleParticleProperties(1, 1, 1, new Vec3d(), new Vec3d(), new Vec3f(), new Vec3f());
            solidExplosionParticles = new SimpleParticleProperties(1, 1, 1, new Vec3d(), new Vec3d(), new Vec3f(), new Vec3f());

            if (sidedAPI.Side == EnumAppSide.Server)
            {
                serverAPI = (ICoreServerAPI)sidedAPI;

                meteorDamageSource = new DamageSource
                {
                    Source = EnumDamageSource.Unknown,
                };
            }
        }
        public override void OnEntityDespawn(EntityDespawnData despawn)
        {
            base.OnEntityDespawn(despawn);

            explosionRadius = (int)((this.entity.Properties.Client.Size + 0.5f) * explosionRadiusModifier);

            if(sidedAPI.Side == EnumAppSide.Server)
            {
                injuryRadius = explosionRadius * 2;
                injuryDamageModifier = this.entity.Properties.Client.Size * injuryDamageModifier;
            }
            if (despawn != null)
            {
                if(despawn.Reason != EnumDespawnReason.OutOfRange)
                {
                    SpawnExplosionParticles();

                    if(despawn.DamageSourceForDeath != null)
                        switch (despawn.DamageSourceForDeath.Type)
                        {
                            case EnumDamageType.Fire:
                                ExplodeInAir();
                                break;
                            case EnumDamageType.Gravity:
                                ExplodeOnLand();
                                break;
                            default:
                                break;
                        }
                }
            }

        }
        private void ExplodeInAir()
        {
            if (sidedAPI.Side == EnumAppSide.Server)
            {
                this.entity.World.PlaySoundAt(new AssetLocation("meteoricexpansion", "sounds/effect/air_meteor_explosion_layered"), this.entity, null, true, 512, 1.0f);

                if (serverAPI.World.Config.GetBool("Destructive") == true)
                {
                    InjureEntities(this.entity.ServerPos.XYZ);
                }
            }
        }
        private void ExplodeOnLand()
        {
            if (sidedAPI.Side == EnumAppSide.Server)
            {
                this.entity.World.PlaySoundAt(new AssetLocation("meteoricexpansion", "sounds/effect/land_meteor_explosion_layered"), this.entity, null, true, 512, 1.0f);

                //-- entity.PreviousServerPos is never set in VS 1.17 for some reason! Therefore, the direction vector is stored in an attribute on the entity and used instead of calculating it from currentPos - previousPos --//

                Vec3d currentPos = this.entity.ServerPos.XYZ;

                Vec3i meteorDirectionInt = entity.Attributes.GetVec3i("direction", Vec3i.Zero);
                Vec3d meteorDirection = new Vec3d(meteorDirectionInt.X, meteorDirectionInt.Y, meteorDirectionInt.Z).Normalize();

                Vec3d shrapnelDirection = MeteoricExpansionHelpers.InvertVector(meteorDirection);

                if (serverAPI.World.Config.GetBool("Destructive") == true)
                {
                    CreateCrater(meteorDirection, shrapnelDirection);
                    InjureEntities(currentPos);
                }

                //-- The meteor has a chance to 'break apart' on impact and throw out its own drops on impact, amount based on its size --//
                foreach (BlockDropItemStack itemStack in entity.Properties.Drops)
                    for (int i = 0; i < (int)(entity.Properties.Client.Size * explosionRand.Next(minMeteorDrops, maxMeteorDrops)); i++)
                    {
                        entity.World.SpawnItemEntity(itemStack.GetNextItemStack(), entity.ServerPos.XYZ, GetNewItemStackVector(shrapnelDirection, itemStackVelocityModifier));
                    }
            }
        }

        //-- Creates a spherical crater where the meteor made contact with the ground --//
        private void CreateCrater(Vec3d meteorDirection, Vec3d shrapnelDirection)
        {
            List<BlockPos> ashPositions = new List<BlockPos>();
            
            blockAccessor = serverAPI.World.GetBlockAccessorBulkUpdate(true, true, false);

            Vec3i centerPos = this.entity.ServerPos.XYZInt;

            BlockPos craterPos = new BlockPos();

            //-- Initial scan to see if the meteor crater should fill with liquid instead of air --//
            blockAccessor.SearchFluidBlocks(new BlockPos(centerPos.X - explosionRadius, centerPos.Y - explosionRadius, centerPos.Z - explosionRadius), 
                new BlockPos(centerPos.X + explosionRadius, centerPos.Y + explosionRadius, centerPos.Z + explosionRadius), (block, bPos) =>
                {
                    if(block.DrawType == EnumDrawType.Liquid)
                    {
                        liquidAsset = new AssetLocation(block.Code.Domain, block.Code.Path);

                        fillWithLiquid = true;
                        fillHeight = bPos.Y;

                        return true;
                    }

                    return false;
                });
            
            
            //-- Scans every block in a cube determined by the explosion radius and determines whether that block fits within the explosion sphere --//
            blockAccessor.WalkBlocks(new BlockPos(centerPos.X - explosionRadius - crackedCraterRadius, centerPos.Y - explosionRadius - crackedCraterRadius, centerPos.Z - explosionRadius - crackedCraterRadius),
                new BlockPos(centerPos.X + explosionRadius + crackedCraterRadius, centerPos.Y + explosionRadius + crackedCraterRadius, centerPos.Z + explosionRadius + crackedCraterRadius), (block, xPos, yPos, zPos) =>
                {
                    BlockPos blockPos = new BlockPos(xPos, yPos, zPos);

                    if (!IsPositionClaimed(blockPos))
                    {
                        float distanceToCenter = blockPos.DistanceTo(centerPos.ToBlockPos());

                        if (distanceToCenter < explosionRadius)
                        {
                            ExplodeBlock(block, blockPos, shrapnelDirection);

                            if (fillWithLiquid == false)
                            {
                                if (centerPos.Y - blockPos.Y == explosionRadius - 1)
                                {
                                    if (explosionRand.Next(0, 100) < ashBlockChance)
                                        ashPositions.Add(blockPos);
                                }
                            }
                        }
                        else if (distanceToCenter < explosionRadius + crackedCraterRadius + 0.1)
                        {
                            if (block.Code.Equals(new AssetLocation("game", "rock-" + block.Code.EndVariant())))
                            {
                                blockAccessor.SetBlock(serverAPI.World.GetBlock(new AssetLocation("game", "crackedrock-" + block.Code.EndVariant())).Id, blockPos);
                            }
                        }
                    }
                });
            blockAccessor.Commit();

            PlaceMeteor(meteorDirection, shrapnelDirection, centerPos, explosionRadius);
            PlaceAsh(ashPositions);

            blockAccessor.Commit();
        }

        private void ExplodeBlock(Block blockToExplode, BlockPos explosionPos, Vec3d shrapnelDirection)
        {
            AssetLocation blockCode = blockToExplode.Code;

            //-- Explosions do not destroy water, mantle or air blocks --//
            switch (blockCode.Path)
            {
                case "boilingwater-still-7":
                case "saltwater-still-7":
                case "water-still-7":
                case "mantle":
                case "air":
                    break;
                default:
                    //-- If a block being destroyed is an inventory, then throw all the contents of it on the ground. Otherwise, for terrain blocks, only spawn items based on a % chance --//
                    if (blockToExplode is BlockGenericTypedContainer || blockToExplode is BlockShelf || blockToExplode is BlockDisplayCase || blockToExplode is BlockMoldRack)
                    {
                        BlockEntityContainer entityContainer = blockAccessor.GetBlockEntity(explosionPos) as BlockEntityContainer;

                        foreach (ItemSlot slot in entityContainer.Inventory)
                        {
                            if (!slot.Empty)
                                entity.World.SpawnItemEntity(slot.Itemstack, explosionPos.ToVec3d(), GetNewItemStackVector(shrapnelDirection, itemStackVelocityModifier));
                        }
                    }
                    else if (blockToExplode is BlockToolRack)
                    {
                        BlockEntityToolrack blockEntityToolrack = blockAccessor.GetBlockEntity(explosionPos) as BlockEntityToolrack;

                        foreach (ItemSlot slot in blockEntityToolrack.inventory)
                        {
                            if (!slot.Empty)
                                entity.World.SpawnItemEntity(slot.Itemstack, explosionPos.ToVec3d(), GetNewItemStackVector(shrapnelDirection, itemStackVelocityModifier));
                        }
                    }
                    else
                    {
                        foreach (BlockDropItemStack itemStack in blockAccessor.GetBlock(explosionPos, BlockLayersAccess.SolidBlocks).Drops)
                            if (explosionRand.Next(0, 101) < terrainDropChance)
                            {
                                entity.World.SpawnItemEntity(itemStack.GetNextItemStack(), explosionPos.ToVec3d(), GetNewItemStackVector(shrapnelDirection, itemStackVelocityModifier));
                            }
                    }

                    if (fillWithLiquid == false)
                        blockAccessor.SetBlock(0, explosionPos);
                    else
                    {
                        if (explosionPos.Y <= fillHeight)
                            blockAccessor.SetBlock(serverAPI.WorldManager.GetBlockId(liquidAsset), explosionPos);
                        else
                            blockAccessor.SetBlock(0, explosionPos);
                    }

                    blockAccessor.TriggerNeighbourBlockUpdate(explosionPos);
                    break;
            }
        }
        //-- Do damage to any entities within the vicinity of the explosion --//
        private void InjureEntities(Vec3d explosionPos)
        {
            injuryRadius = explosionRadius * 2;
            injuryDamageModifier = this.entity.Properties.Client.Size * injuryDamageModifier;

            Entity[] affectedEntities = this.entity.World.GetEntitiesAround(this.entity.ServerPos.XYZ, injuryRadius, injuryRadius);

            if (affectedEntities.Length > 0)
            {
                foreach (Entity entity in affectedEntities)
                {
                    entity.ReceiveDamage(meteorDamageSource, injuryBaseDamage * this.entity.Properties.Client.Size);
                }
            }
        }

        //-- Get a randomized velocity vector for the items that are being thrown out from the impact --//
        private Vec3d GetNewItemStackVector(Vec3d shrapnelDirection, double velocityModifier)
        {
            Vec3d itemStackVector = new Vec3d
            {
                X = shrapnelDirection.X * explosionRand.NextDouble() * velocityModifier,
                Y = shrapnelDirection.Y * explosionRand.NextDouble() * velocityModifier,
                Z = shrapnelDirection.Z * explosionRand.NextDouble() * velocityModifier
            };

            return itemStackVector;
        }
        private void SpawnExplosionParticles()
        {
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
            quadExplosionParticles.Async = true;
            #endregion

            if (sidedAPI.Side == EnumAppSide.Server)
            {
                #region Cuboid Particle Options
                Vec3f velocityRand = new Vec3f((float)(explosionRand.Next(0, 5) + explosionRand.NextDouble()), (float)(explosionRand.Next(0, 5) + explosionRand.NextDouble()), (float)(explosionRand.Next(0, 5) + explosionRand.NextDouble()));

                solidExplosionParticles.MinPos = this.entity.Pos.XYZ + new Vec3d(-this.entity.Properties.Client.Size / 2, -this.entity.Properties.Client.Size / 2, -this.entity.Properties.Client.Size / 2);
                solidExplosionParticles.AddPos = new Vec3d(this.entity.Properties.Client.Size, this.entity.Properties.Client.Size, this.entity.Properties.Client.Size);

                solidExplosionParticles.MinVelocity = new Vec3f(-velocityRand.X, -velocityRand.Y, -velocityRand.Z);

                solidExplosionParticles.AddVelocity = velocityRand * 2;

                solidExplosionParticles.GravityEffect = 0.1f;

                solidExplosionParticles.MinSize = 1.0f;
                solidExplosionParticles.MaxSize = 3.0f;
                solidExplosionParticles.SizeEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARNULLIFY, -2.5f);

                solidExplosionParticles.MinQuantity = 100;
                solidExplosionParticles.AddQuantity = 50;

                solidExplosionParticles.LifeLength = 2.0f;
                solidExplosionParticles.addLifeLength = 2.5f;

                solidExplosionParticles.ShouldDieInLiquid = true;

                solidExplosionParticles.WithTerrainCollision = true;

                solidExplosionParticles.Color = ColorUtil.ColorFromRgba(255, 255, 255, 255);

                solidExplosionParticles.BlueEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, explosionRand.Next(0, 150));
                solidExplosionParticles.GreenEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, explosionRand.Next(150, 255));
                solidExplosionParticles.RedEvolve = new EvolvingNatFloat(EnumTransformFunction.LINEARREDUCE, 250);

                solidExplosionParticles.VertexFlags = explosionRand.Next(150, 255);
                solidExplosionParticles.Async = true;

                #endregion

                this.entity.World.SpawnParticles(solidExplosionParticles);
            }

            this.entity.World.SpawnParticles(quadExplosionParticles);

        }
        private void PlaceMeteor(Vec3d direction, Vec3d shrapnelDirection, Vec3i explosionPos, int explosionRadius)
        {
            Vec3i resourceLocation = new Vec3i(explosionPos.X + (int)(direction.X * (explosionRadius + 1)), explosionPos.Y + (int)(direction.Y * (explosionRadius + 1)), explosionPos.Z + (int)(direction.Z * (explosionRadius + 1)));
            
            string metalType = this.entity.FirstCodePart(1);
            string stoneType = this.entity.FirstCodePart(2);

            int meteorRemainsSize = (int)(this.entity.Properties.CollisionBoxSize.X + 0.5f) / 2;
            int metalBlockID = 0;
            int stoneBlockID = 0;

            string[] richnesses = { "poor", "medium", "rich", "bountiful" };

            string metalBlockCode = "";
            string stoneBlockCode = "";

            //-- Searches for the best possible ore of the meteor/stone type to be used as the meteor resource --//
            for(int i = 3; i >= 0 && metalBlockID == 0; i--)
            {
                metalBlockCode = "meteoricmetallicrock-" + richnesses[i] + "-" + metalType + "-" + stoneType;

                if (fillWithLiquid == false)
                {
                    metalBlockCode += "-smouldering";
                }
                else
                { 
                    metalBlockCode += "-cooled";
                }

                AssetLocation assetLocation = new AssetLocation("meteoricexpansion", metalBlockCode);

                if (serverAPI.World.GetBlock(assetLocation) != null)
                {
                    metalBlockID = serverAPI.World.GetBlock(assetLocation).BlockId;

                    break;
                }
            }

            stoneBlockCode = "meteoricrock-" + stoneType;

            if (fillWithLiquid == false)
            {
                stoneBlockCode += "-smouldering";
            }
            else
            {
                stoneBlockCode += "-cooled";
            }

            stoneBlockID = serverAPI.WorldManager.GetBlockId(new AssetLocation("meteoricexpansion", stoneBlockCode));

            blockAccessor.WalkBlocks(new BlockPos(resourceLocation.X - meteorRemainsSize - crackedCraterRadius, resourceLocation.Y - meteorRemainsSize - crackedCraterRadius, resourceLocation.Z - meteorRemainsSize - crackedCraterRadius),
                new BlockPos(resourceLocation.X + meteorRemainsSize + crackedCraterRadius, resourceLocation.Y + meteorRemainsSize + crackedCraterRadius, resourceLocation.Z + meteorRemainsSize + crackedCraterRadius), (block, xPos, yPos, zPos) =>
                {
                    BlockPos blockPos = new BlockPos(xPos, yPos, zPos);

                    if (!IsPositionClaimed(blockPos))
                    {
                        //-- Modifies rock around the deposit to be cracked rock --//
                        if (blockPos.DistanceTo(resourceLocation.AsBlockPos) > meteorRemainsSize)
                        {
                            if (block.Code.Equals(new AssetLocation("game", "rock-" + block.Code.EndVariant())))
                            {
                                blockAccessor.SetBlock(serverAPI.World.GetBlock(new AssetLocation("game", "crackedrock-" + block.Code.EndVariant())).Id, blockPos);
                            }
                        }
                        else
                        {
                            PlaceMeteorResources(blockPos, metalBlockID, stoneBlockID);
                        }
                    }
                }, false);
        }
        private void PlaceMeteorResources(BlockPos bpos, int metalBlockID, int stoneBlockID)
        {
            int placeMeteorBlockRand = explosionRand.Next(0, 101);
            switch (placeMeteorBlockRand < meteorImpactResourceChance)
            {
                case true:
                    int metalBlockRand = explosionRand.Next(0, 101);

                    switch (metalBlockRand < metalResourceChance)
                    {
                        case true:
                            blockAccessor.SetBlock(metalBlockID, bpos);
                            break;
                        case false:
                            blockAccessor.SetBlock(stoneBlockID, bpos);
                            break;
                    }

                    positionsChanged.Add(bpos.Copy());
                    break;
                case false:
                    break;
            }
        }
        private void PlaceAsh(List<BlockPos> ashPositions)
        {
            if (ashPositions.Count == 0)
                return;

            AssetLocation[] ashBlocks = new AssetLocation[] 
            {
                new AssetLocation("meteoricexpansion", "rockashpile-small"),
                new AssetLocation("meteoricexpansion", "rockashpile-medium"),
                new AssetLocation("meteoricexpansion", "rockashpile-large")
            };

            int ashID;

            foreach(BlockPos position in ashPositions)
            {
                ashID = serverAPI.World.GetBlock(ashBlocks[explosionRand.Next(0, 3)]).Id;

                blockAccessor.SetBlock(ashID, position);
            }
        }
        private bool IsPositionClaimed(BlockPos blockPos)
        {
            if (serverAPI.World.Config.GetBool("ClaimsProtected") == true)
                foreach (LandClaim claim in serverAPI.World.Claims.All)
                {
                    if (claim.PositionInside(blockPos))
                        return true;
                }

            return false;
        }
    }
}
