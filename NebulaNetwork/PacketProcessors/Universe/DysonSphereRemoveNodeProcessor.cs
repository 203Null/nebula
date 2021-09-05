﻿using NebulaAPI;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Universe;
using NebulaWorld;

namespace NebulaNetwork.PacketProcessors.Universe
{
    [RegisterPacketProcessor]
    internal class DysonSphereRemoveNodeProcessor : PacketProcessor<DysonSphereRemoveNodePacket>
    {
        private readonly IPlayerManager playerManager;

        public DysonSphereRemoveNodeProcessor()
        {
            playerManager = Multiplayer.Session.Network.PlayerManager;
        }

        public override void ProcessPacket(DysonSphereRemoveNodePacket packet, NebulaConnection conn)
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
                using (Multiplayer.Session.DysonSpheres.IsIncomingRequest.On())
                {
                    DysonSphereLayer dsl = GameMain.data.dysonSpheres[packet.StarIndex]?.GetLayer(packet.LayerId);
                    if (dsl != null)
                    {
                        int num = 0;
                        DysonNode dysonNode = dsl.nodePool[packet.NodeId];
                        //Remove all frames that are part of the node
                        while (dysonNode.frames.Count > 0)
                        {
                            dsl.RemoveDysonFrame(dysonNode.frames[0].id);
                            if (num++ > 4096)
                            {
                                Assert.CannotBeReached();
                                break;
                            }
                        }
                        //Remove all shells that are part of the node
                        while (dysonNode.shells.Count > 0)
                        {
                            dsl.RemoveDysonShell(dysonNode.shells[0].id);
                            if (num++ > 4096)
                            {
                                Assert.CannotBeReached();
                                break;
                            }
                        }
                        dsl.RemoveDysonNode(packet.NodeId);
                    }
                }
            }
        }
    }
}
