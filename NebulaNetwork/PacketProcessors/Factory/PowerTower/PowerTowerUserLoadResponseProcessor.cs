﻿#region

using NebulaAPI.Packets;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Factory.PowerTower;
using NebulaWorld;

#endregion

namespace NebulaNetwork.PacketProcessors.Factory.PowerTower;

[RegisterPacketProcessor]
internal class PowerTowerUserLoadResponseProcessor : PacketProcessor<PowerTowerUserLoadingResponse>
{
    protected override void ProcessPacket(PowerTowerUserLoadingResponse packet, NebulaConnection conn)
    {
        var factory = GameMain.galaxy.PlanetById(packet.PlanetId)?.factory;
        if (factory is not { powerSystem: not null })
        {
            return;
        }
        var pNet = factory.powerSystem.netPool[packet.NetId];

        if (packet.Charging)
        {
            Multiplayer.Session.PowerTowers.AddExtraDemand(packet.PlanetId, packet.NetId, packet.NodeId,
                packet.PowerAmount);
            if (IsClient)
            {
                if (Multiplayer.Session.PowerTowers.DidRequest(packet.PlanetId, packet.NetId, packet.NodeId))
                {
                    var baseDemand = factory.powerSystem.nodePool[packet.NodeId].workEnergyPerTick -
                                     factory.powerSystem.nodePool[packet.NodeId].idleEnergyPerTick;
                    var mult = factory.powerSystem.networkServes[packet.NetId];
                    Multiplayer.Session.PowerTowers.PlayerChargeAmount += (int)(mult * baseDemand);
                }
            }
        }
        else
        {
            Multiplayer.Session.PowerTowers.RemExtraDemand(packet.PlanetId, packet.NetId, packet.NodeId);
        }

        if (IsHost)
        {
            Multiplayer.Session.Network.SendPacketToStar(new PowerTowerUserLoadingResponse(packet.PlanetId,
                    packet.NetId,
                    packet.NodeId,
                    packet.PowerAmount,
                    pNet.energyCapacity,
                    pNet.energyRequired,
                    pNet.energyServed,
                    pNet.energyAccumulated,
                    pNet.energyExchanged,
                    packet.Charging),
                GameMain.galaxy.PlanetById(packet.PlanetId).star.id);
        }
        else
        {
            pNet.energyCapacity = packet.EnergyCapacity;
            pNet.energyRequired = packet.EnergyRequired;
            pNet.energyAccumulated = packet.EnergyAccumulated;
            pNet.energyExchanged = packet.EnergyExchanged;
            pNet.energyServed = packet.EnergyServed;
        }
    }
}
