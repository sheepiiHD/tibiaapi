﻿using System.Collections.Generic;
using Tibia.Constants;

namespace Tibia.Packets.Incoming
{
    public class ShopInfo
    {
        public ushort ItemId { get; set; }
        public byte CountOrSubType { get; set; }
        public uint BuyPrice { get; set; }
        public uint SellPrice { get; set; }
        public string ItemName { get; set; }
        public uint Weight { get; set; }

        public ShopInfo() { }

        public ShopInfo(ushort itemId, byte subType, uint buyPrice, uint sellPrice)
        {
            ItemId = itemId;
            CountOrSubType = subType;
            BuyPrice = buyPrice;
            SellPrice = sellPrice;
        }
    }

    public class OpenNPCTradePacket : IncomingPacket
    {
        public string NPCName { get; set; }
        public List<ShopInfo> ShopList { get; set; }

        public OpenNPCTradePacket(Objects.Client c)
            : base(c)
        {
            Type = IncomingPacketType.OpenNPCTrade;
            Destination = PacketDestination.Client;
        }

        public override bool ParseMessage(NetworkMessage msg, PacketDestination destination)
        {
            int position = msg.Position;

            if (msg.GetByte() != (byte)IncomingPacketType.OpenNPCTrade)
                return false;

            Destination = destination;
            Type = IncomingPacketType.OpenNPCTrade;

            NPCName = msg.GetString();

            ushort cap = (Client.VersionNumber >= 900) ? msg.GetUInt16() : msg.GetByte();
            ShopList = new List<ShopInfo> { };

            for (int i = 0; i < cap; i++)
            {
                ShopInfo item = new ShopInfo();

                item.ItemId = msg.GetUInt16();
                item.CountOrSubType = msg.GetByte();
                item.ItemName = msg.GetString();
                item.Weight = msg.GetUInt32();
                item.BuyPrice = msg.GetUInt32();
                item.SellPrice = msg.GetUInt32();
                ShopList.Add(item);
            }

            return true;
        }

        public override void ToNetworkMessage(NetworkMessage msg)
        {
            msg.AddByte((byte)Type);
            msg.AddString(NPCName);
            if (Client.VersionNumber >= 900)
                msg.AddUInt16((ushort)ShopList.Count);
            else
                msg.AddByte((byte)ShopList.Count);

            foreach (ShopInfo i in ShopList)
            {
                msg.AddUInt16(i.ItemId);
                msg.AddByte(i.CountOrSubType);
                msg.AddString(i.ItemName);
                msg.AddUInt32(i.Weight);
                msg.AddUInt32(i.BuyPrice);
                msg.AddUInt32(i.SellPrice);
            }
        }
    }
}