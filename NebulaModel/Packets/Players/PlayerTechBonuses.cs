﻿using NebulaAPI;

namespace NebulaModel.Packets.Players
{
    public class PlayerTechBonuses : IPlayerTechBonuses
    {
        public double coreEnergyCap { get; set; }
        public double corePowerGen { get; set; }
        public double reactorPowerGen { get; set; }
        public double walkPower { get; set; }
        public double jumpEnergy { get; set; }
        public double thrustPowerPerAcc { get; set; }
        public double warpKeepingPowerPerSpeed { get; set; }
        public double warpStartPowerPerSpeed { get; set; }
        public double miningPower { get; set; }
        public double replicatePower { get; set; }
        public double researchPower { get; set; }
        public double droneEjectEnergy { get; set; }
        public double droneEnergyPerMeter { get; set; }
        public int coreLevel { get; set; }
        public int thrusterLevel { get; set; }
        public float miningSpeed { get; set; }
        public float replicateSpeed { get; set; }
        public float walkSpeed { get; set; }
        public float jumpSpeed { get; set; }
        public float maxSailSpeed { get; set; }
        public float maxWarpSpeed { get; set; }
        public float buildArea { get; set; }
        public int droneCount { get; set; }
        public float droneSpeed { get; set; }
        public int droneMovement { get; set; }
        public int inventorySize { get; set; }

        public PlayerTechBonuses() { }

        public PlayerTechBonuses(Mecha source)
        {
            coreEnergyCap = source.coreEnergyCap;
            corePowerGen = source.corePowerGen;
            reactorPowerGen = source.reactorPowerGen;
            walkPower = source.walkPower;
            jumpEnergy = source.jumpEnergy;
            thrustPowerPerAcc = source.thrustPowerPerAcc;
            warpKeepingPowerPerSpeed = source.warpKeepingPowerPerSpeed;
            warpStartPowerPerSpeed = source.warpStartPowerPerSpeed;
            miningPower = source.miningPower;
            replicatePower = source.replicatePower;
            researchPower = source.researchPower;
            droneEjectEnergy = source.droneEjectEnergy;
            droneEnergyPerMeter = source.droneEnergyPerMeter;
            coreLevel = source.coreLevel;
            thrusterLevel = source.thrusterLevel;
            miningSpeed = source.miningSpeed;
            replicateSpeed = source.replicateSpeed;
            walkSpeed = source.walkSpeed;
            jumpSpeed = source.jumpSpeed;
            maxSailSpeed = source.maxSailSpeed;
            maxWarpSpeed = source.maxWarpSpeed;
            buildArea = source.buildArea;
            droneCount = source.droneCount;
            droneSpeed = source.droneSpeed;
            droneMovement = source.droneMovement;
            inventorySize = source.player.package.size;
        }

        public void UpdateMech(Mecha destination)
        {
            destination.coreEnergyCap = coreEnergyCap;
            destination.corePowerGen = corePowerGen;
            destination.reactorPowerGen = reactorPowerGen;
            destination.walkPower = walkPower;
            destination.jumpEnergy = jumpEnergy;
            destination.thrustPowerPerAcc = thrustPowerPerAcc;
            destination.warpKeepingPowerPerSpeed = warpKeepingPowerPerSpeed;
            destination.warpStartPowerPerSpeed = warpStartPowerPerSpeed;
            destination.miningPower = miningPower;
            destination.replicatePower = replicatePower;
            destination.researchPower = researchPower;
            destination.droneEjectEnergy = droneEjectEnergy;
            destination.droneEnergyPerMeter = droneEnergyPerMeter;
            destination.coreLevel = coreLevel;
            destination.thrusterLevel = thrusterLevel;
            destination.miningSpeed = miningSpeed;
            destination.replicateSpeed = replicateSpeed;
            destination.walkSpeed = walkSpeed;
            destination.jumpSpeed = jumpSpeed;
            destination.maxSailSpeed = maxSailSpeed;
            destination.maxWarpSpeed = maxWarpSpeed;
            destination.buildArea = buildArea;
            destination.droneCount = droneCount;
            destination.droneSpeed = droneSpeed;
            destination.droneMovement = droneMovement;
            if (inventorySize > destination.player.package.size)
            {
                destination.player.package.SetSize(inventorySize);
            }
        }

        public void Serialize(INetDataWriter writer)
        {
            writer.Put(coreEnergyCap);
            writer.Put(corePowerGen);
            writer.Put(reactorPowerGen);
            writer.Put(walkPower);
            writer.Put(jumpEnergy);
            writer.Put(thrustPowerPerAcc);
            writer.Put(warpKeepingPowerPerSpeed);
            writer.Put(warpStartPowerPerSpeed);
            writer.Put(miningPower);
            writer.Put(replicatePower);
            writer.Put(researchPower);
            writer.Put(droneEjectEnergy);
            writer.Put(droneEnergyPerMeter);
            writer.Put(coreLevel);
            writer.Put(thrusterLevel);
            writer.Put(miningSpeed);
            writer.Put(replicateSpeed);
            writer.Put(walkSpeed);
            writer.Put(jumpSpeed);
            writer.Put(maxSailSpeed);
            writer.Put(maxWarpSpeed);
            writer.Put(buildArea);
            writer.Put(droneCount);
            writer.Put(droneSpeed);
            writer.Put(droneMovement);
            writer.Put(inventorySize);
        }

        public void Deserialize(INetDataReader reader)
        {
            coreEnergyCap = reader.GetDouble();
            corePowerGen = reader.GetDouble();
            reactorPowerGen = reader.GetDouble();
            walkPower = reader.GetDouble();
            jumpEnergy = reader.GetDouble();
            thrustPowerPerAcc = reader.GetDouble();
            warpKeepingPowerPerSpeed = reader.GetDouble();
            warpStartPowerPerSpeed = reader.GetDouble();
            miningPower = reader.GetDouble();
            replicatePower = reader.GetDouble();
            researchPower = reader.GetDouble();
            droneEjectEnergy = reader.GetDouble();
            droneEnergyPerMeter = reader.GetDouble();
            coreLevel = reader.GetInt();
            thrusterLevel = reader.GetInt();
            miningSpeed = reader.GetFloat();
            replicateSpeed = reader.GetFloat();
            walkSpeed = reader.GetFloat();
            jumpSpeed = reader.GetFloat();
            maxSailSpeed = reader.GetFloat();
            maxWarpSpeed = reader.GetFloat();
            buildArea = reader.GetFloat();
            droneCount = reader.GetInt();
            droneSpeed = reader.GetFloat();
            droneMovement = reader.GetInt();
            inventorySize = reader.GetInt();
        }
    }
}
