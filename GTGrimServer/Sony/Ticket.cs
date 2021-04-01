using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;

using Syroot.BinaryData;
namespace GTGrimServer.Sony
{
    public class Ticket
    {
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }

        public byte[] Signature { get; set; }
        public ulong IssuedDate { get; set; }
        public ulong ExpiryDate { get; set; }
        public ulong UserId { get; set; }
        public string OnlineId { get; set; }
        public byte[] Region { get; set; }
        public string Domain { get; set; }
        public byte[] ServiceID { get; set; }

        public static Ticket FromBuffer(byte[] buffer)
        {
            using var ms = new MemoryStream(buffer);
            return FromStream(ms);
        }

        public static Ticket FromStream(Stream stream)
        {
            var ticket = new Ticket();
            var bs = new BinaryStream(stream);
            bs.ByteConverter = ByteConverter.Big;

            ticket.VersionMajor = bs.ReadByte() >> 4;
            ticket.VersionMinor = bs.ReadByte();

            bs.Position += 4;

            int headerSize = bs.ReadInt32();
            bs.ReadInt32(); // Blob thing

            bs.ReadInt16();

            ticket.Signature = (byte[])ReadNext(bs);
            ReadNext(bs);
            ticket.IssuedDate = (ulong)ReadNext(bs);
            ticket.ExpiryDate = (ulong)ReadNext(bs);
            ticket.UserId = (ulong)ReadNext(bs);
            ticket.OnlineId = (string)ReadNext(bs);
            ticket.Region = (byte[])ReadNext(bs);
            ticket.Domain = (string)ReadNext(bs);
            ticket.ServiceID = (byte[])ReadNext(bs);

            return ticket;
        }

        private static object ReadNext(BinaryStream bs)
        {
            TicketFieldType type = (TicketFieldType)bs.ReadInt16();
            short dataLen = bs.ReadInt16();

            switch (type)
            {
                case TicketFieldType.Empty:
                    return null;
                case TicketFieldType.UInt32:
                    return bs.ReadUInt32();
                case TicketFieldType.UInt64:
                    return bs.ReadUInt64();
                case TicketFieldType.String:
                    return bs.ReadString(dataLen);
                case TicketFieldType.Timestamp:
                    return bs.ReadUInt64();
                case TicketFieldType.Binary:
                    return bs.ReadBytes(dataLen);
                default:
                    return null;
            }
        }
    }

    public enum TicketFieldType
    {
        Empty = 0,
        UInt32 = 1,
        UInt64 = 2,
        String = 4,
        Timestamp = 7,
        Binary = 8,
    }
}
