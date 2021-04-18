using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Buffers.Binary;
using System.IO;

using Syroot.BinaryData;
using System.Text;
namespace GTGrimServer.Sony
{
    public class NPTicket
    {
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }

        public byte[] Signature { get; set; }
        public ulong IssuedDate { get; set; }
        public ulong ExpiryDate { get; set; }
        public ulong UserId { get; set; }
        public string OnlineId { get; set; }
        public string Region { get; set; }
        public NPLanguage Language { get; set; }

        public string Domain { get; set; }
        public byte[] ServiceID { get; set; }

        public static NPTicket FromBuffer(byte[] buffer)
        {
            using var ms = new MemoryStream(buffer);
            return FromStream(ms);
        }

        public static NPTicket FromStream(Stream stream)
        {
            var ticket = new NPTicket();
            var bs = new BinaryStream(stream);
            bs.ByteConverter = ByteConverter.Big;

            ticket.VersionMajor = bs.ReadByte() >> 4;
            ticket.VersionMinor = bs.ReadByte();

            if (ticket.VersionMajor >= 4)
                bs.Position = 0x10;
            else
                bs.Position = 0x0C;

            ticket.Signature = (byte[])ReadNext(bs);
            ReadNext(bs);
            ticket.IssuedDate = (ulong)ReadNext(bs);
            ticket.ExpiryDate = (ulong)ReadNext(bs);
            ticket.UserId = (ulong)ReadNext(bs);
            ticket.OnlineId = (string)ReadNext(bs);

            byte[] localeData = (byte[])ReadNext(bs);
            ticket.Region = Encoding.UTF8.GetString(localeData.AsSpan(0, 2));
            ticket.Language = (NPLanguage)BinaryPrimitives.ReadInt16BigEndian(localeData.AsSpan()[2..]);

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
                    return bs.ReadString(dataLen).TrimEnd('\0');
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

    public enum NPLanguage
    {
        Japanese,
        English,
        French,
        Spanish,
        German,
        Italian,
        Norwegian,
        Portugese,
        Russian,
        Korean,
        Chinese
    }
}
