﻿using NebulaAPI;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Universe;
using NebulaWorld;

namespace NebulaNetwork.PacketProcessors.Universe
{
    [RegisterPacketProcessor]
    internal class DysonSwarmRemoveOrbitProcessor : PacketProcessor<DysonSwarmRemoveOrbitPacket>
    {
        private readonly IPlayerManager playerManager;

        public DysonSwarmRemoveOrbitProcessor()
        {
            playerManager = Multiplayer.Session.Network.PlayerManager;
        }

        public override void ProcessPacket(DysonSwarmRemoveOrbitPacket packet, NebulaConnection conn)
        {
            bool valid = true;
            if (IsHost)
            {
                INebulaPlayer player = playerManager.GetPlayer(conn);
                if (player != null)
                {
                    playerManager.SendPacketToOtherPlayers(packet, player);
                }
                else
                {
                    valid = false;
                }
            }

            if (valid)
            {
                using (Multiplayer.Session.DysonSpheres.IncomingDysonSwarmPacket.On())
                {
                    GameMain.data.dysonSpheres[packet.StarIndex]?.swarm?.RemoveOrbit(packet.OrbitId);
                }
            }
        }
    }
}
