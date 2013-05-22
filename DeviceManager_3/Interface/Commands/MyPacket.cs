using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeviceManager
{
    public class MyPacket
    {
        public byte fl;
        public byte ladd;
        public byte lpak;
        public byte cmd;
        public byte adr;
        public byte ldata;
        public byte[] data;
        public byte status;
        public byte crc;

        public bool isActive = false;
        public string NameCommand = string.Empty;
        public TypeCRC typeCRC = TypeCRC.Cycled;

        public MyPacket(TypeCRC TypeCrc)
        {
            fl = 0;
            ladd = 0;
            lpak = 0;
            cmd = 0;
            adr = 0;
            ldata = 0;
            data = null;
            status = 0;
            crc = 0;

            typeCRC = TypeCrc;
        }

        public byte[] Packet
        {
            get
            {
                int size = 8;
                if (data != null) size += data.Length;

                byte[] packet = new byte[size];

                packet[0] = fl;
                packet[1] = ladd;
                packet[2] = lpak;
                packet[3] = cmd;
                packet[4] = adr;
                packet[5] = ldata;

                if (data != null)
                {
                    Array.Copy(data, 0, packet, 6, data.Length);
                }

                packet[packet.Length - 2] = status;

                switch (typeCRC)
                {
                    case TypeCRC.Cycled:

                        ushort crcOne = Application.GetCRC(packet, typeCRC);
                        packet[packet.Length - 1] = (byte)crcOne;
                        
                        break;

                    case TypeCRC.CycledTwo:

                        ushort crcTwo = Application.GetCRC(packet, typeCRC);

                        packet[packet.Length - 1] = (byte)crcTwo;
                        crcTwo >>= 8;
                        packet[packet.Length - 2] = (byte)crcTwo;
                        break;

                    case TypeCRC.CRC16:

                        ushort crc16 = Application.GetCRC(packet, typeCRC);

                        packet[packet.Length - 1] = (byte)crc16;
                        crc16 >>= 8;
                        packet[packet.Length - 2] = (byte)crc16;
                        break;

                    default:

                        break;
                }

                return packet;
            }

            set
            {
                byte[] packet = value;

                fl = packet[0];
                ladd = packet[1];
                lpak = packet[2];
                cmd = packet[3];
                adr = packet[4];
                ldata = packet[5];

                if (lpak > 7)
                {
                    data = new byte[ldata];
                    Array.Copy(packet, 6, data, 0, ldata);
                }

                packet[packet.Length - 2] = status;
                switch (typeCRC)
                {
                    case TypeCRC.Cycled:

                        ushort crcOne = Application.GetCRC(packet, typeCRC);
                        packet[packet.Length - 1] = (byte)crcOne;
                        break;

                    case TypeCRC.CycledTwo:

                        ushort crcTwo = Application.GetCRC(packet, typeCRC);

                        packet[packet.Length - 1] = (byte)crcTwo;
                        crcTwo >>= 8;
                        packet[packet.Length - 2] = (byte)crcTwo;
                        break;

                    case TypeCRC.CRC16:

                        ushort crc16 = Application.GetCRC(packet, typeCRC);

                        packet[packet.Length - 1] = (byte)crc16;
                        crc16 >>= 8;
                        packet[packet.Length - 2] = (byte)crc16;
                        break;

                    default:

                        break;
                }
            }
        }

        public string ConvertToString()
        {
            string total = string.Empty;
            foreach (byte item in Packet)
            {
                total += string.Format("{0:x2}", item);
            }
            return total;
        }

        public void InitFromString(string packet)
        {
            try
            {
                int len = packet.Length / 2;
                byte[] m_packet = new byte[len];

                for (int i = 0; i < len; i++)
                {
                    string byt = packet.Substring(i * 2, 2);
                    m_packet[i] = (byte)(int.Parse(byt, System.Globalization.NumberStyles.AllowHexSpecifier));
                }
                Packet = m_packet;
            }
            catch (Exception) { }
        }

        public void CorrectCRC()
        {
            Packet = Packet;
        }
    }
}