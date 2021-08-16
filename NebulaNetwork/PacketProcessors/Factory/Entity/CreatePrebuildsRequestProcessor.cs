﻿using NebulaAPI;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Factory;
using NebulaWorld.Factory;
using FactoryManager = NebulaWorld.Factory.FactoryManager;

namespace NebulaNetwork.PacketProcessors.Factory.Entity
{
    [RegisterPacketProcessor]
    class CreatePrebuildsRequestProcessor : PacketProcessor<CreatePrebuildsRequest>
    {
        public override void ProcessPacket(CreatePrebuildsRequest packet, NebulaConnection conn)
        {
            using (FactoryManager.Instance.IsIncomingRequest.On())
            {
                BuildToolManager.CreatePrebuildsRequest(packet);
            }
        }
    }
}
