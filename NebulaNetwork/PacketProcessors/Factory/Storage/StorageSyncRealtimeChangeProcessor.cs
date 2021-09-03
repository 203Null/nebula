﻿using NebulaAPI;
using NebulaModel.Networking;
using NebulaModel.Packets;
using NebulaModel.Packets.Factory;
using NebulaWorld;

namespace NebulaNetwork.PacketProcessors.Factory.Storage
{
    [RegisterPacketProcessor]
    internal class StorageSyncRealtimeChangeProcessor : PacketProcessor<StorageSyncRealtimeChangePacket>
    {
        public override void ProcessPacket(StorageSyncRealtimeChangePacket packet, NebulaConnection conn)
        {
            StorageComponent storage = null;
            StorageComponent[] pool = GameMain.galaxy.PlanetById(packet.PlanetId)?.factory?.factoryStorage?.storagePool;
            if (pool != null && packet.StorageIndex != -1 && packet.StorageIndex < pool.Length)
            {
                storage = pool[packet.StorageIndex];
            }

            if (storage != null)
            {
                using (Multiplayer.Session.Storage.IsIncomingRequest.On())
                {
                    int itemId = packet.ItemId;
                    int count = packet.Count;
                    if (packet.StorageEvent == StorageSyncRealtimeChangeEvent.AddItem2)
                    {
                        storage.AddItem(itemId, count, packet.StartIndex, packet.Length);
                    }
                    else if (packet.StorageEvent == StorageSyncRealtimeChangeEvent.AddItemStacked)
                    {
                        storage.AddItemStacked(itemId, count);
                    }
                    else if (packet.StorageEvent == StorageSyncRealtimeChangeEvent.TakeItemFromGrid)
                    {
                        storage.TakeItemFromGrid(packet.Length, ref itemId, ref count);
                    }

                    if (IsHost)
                    {
                        StorageSyncManager.SendToOtherPlayersOnTheSamePlanet(conn, packet, packet.PlanetId);
                    }
                }
            }
        }
    }
}