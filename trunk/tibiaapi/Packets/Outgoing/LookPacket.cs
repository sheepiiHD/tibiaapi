﻿using Tibia.Constants;

namespace Tibia.Packets.Outgoing
{
    public class LookPacket : OutgoingPacket
    {

        public Objects.Location Location { get; set; }
        public ushort SpriteId { get; set; }
        public byte StackPosition { get; set; }

        public LookPacket(Objects.Client c)
            : base(c)
        {
            Type = OutgoingPacketType.Look;
            Destination = PacketDestination.Server;
        }

        public override bool ParseMessage(NetworkMessage msg, PacketDestination destination)
        {
            if (msg.GetByte() != (byte)OutgoingPacketType.Look)
                return false;

            Destination = destination;
            Type = OutgoingPacketType.Look;

            Location = msg.GetLocation();
            SpriteId = msg.GetUInt16();
            StackPosition = msg.GetByte();

            return true;
        }

        public override void ToNetworkMessage(NetworkMessage msg)
        {
            msg.AddByte((byte)Type);

            msg.AddLocation(Location);
            msg.AddUInt16(SpriteId);
            msg.AddByte(StackPosition);
        }

        public static bool Send(Objects.Client client, Objects.Location position, ushort spriteId, byte stackPosition)
        {
            LookPacket p = new LookPacket(client);
            p.Location = position;
            p.SpriteId = spriteId;
            p.StackPosition = stackPosition;
            return p.Send();
        }
    }
}