﻿using LiteNetLib;
using LiteNetLib.Utils;
using NebulaModel.Networking;
using NebulaModel.Packets.Session;
using NebulaModel.Utils;
using NebulaWorld;
using UnityEngine;

namespace NebulaClient.MonoBehaviours
{
    public class MultiplayerClientSession : MonoBehaviour, INetworkProvider
    {
        public static MultiplayerClientSession Instance { get; protected set; }

        private NetManager client;
        private NebulaConnection serverConnection;

        public NetPacketProcessor PacketProcessor { get; protected set; }
        public bool IsConnected { get; protected set; }

        private void Awake()
        {
            Instance = this;
        }
        
        public void Connect(string ip, int port)
        {
            EventBasedNetListener listener = new EventBasedNetListener();
            listener.PeerConnectedEvent += OnPeerConnected;
            listener.PeerDisconnectedEvent += OnPeerDisconnected;
            listener.NetworkReceiveEvent += OnNetworkReceive;

            client = new NetManager(listener)
            {
                AutoRecycle = true,
            };

            PacketProcessor = new NetPacketProcessor();
            LiteNetLibUtils.RegisterAllPacketNestedTypes(PacketProcessor);
            LiteNetLibUtils.RegisterAllPacketProcessorsInCallingAssembly(PacketProcessor);

            client.Start();
            client.Connect(ip, port, "nebula");

            LocalPlayer.IsMasterClient = false;
            LocalPlayer.SetNetworkProvider(this);
            SimulatedWorld.Initialize();
        }

        public void Disconnect()
        {
            IsConnected = false;
            client.Stop();
        }

        public void Update()
        {
            client?.PollEvents();
        }

        public void SendPacket<T>(T packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableOrdered) where T : class, new()
        {
            serverConnection?.SendPacket(packet, deliveryMethod);
        }

        private void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            PacketProcessor.ReadAllPackets(reader, new NebulaConnection(peer, PacketProcessor));
        }

        private void OnPeerConnected(NetPeer peer)
        {
            serverConnection = new NebulaConnection(peer, PacketProcessor);
            IsConnected = true;
            SendPacket(new HandshakeRequest());
        }

        private void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            IsConnected = false;
            serverConnection = null;

            /*
            UIMessageBox.Show(
                "Connection Lost",
                $"You have been disconnect of the server.\nReason{disconnectInfo.Reason}",
                "Quit",
                "Reconnect",
                0,
                new UIMessageBox.Response(() =>
                {
                    // MultiplayerSession.instance.LeaveGame();
                }),
                new UIMessageBox.Response(() =>
                {
                    // MultiplayerSession.instance.TryToReconnect();
                }));
            */
        }
    }
}
