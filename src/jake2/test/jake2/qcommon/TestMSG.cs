using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Qcommon
{
    public class TestMSG : MSG
    {
        public static void Main(String[] args)
        {
            byte[] buf_data = new byte[Defines.MAX_MSGLEN];
            sizebuf_t buf = new sizebuf_t();
            SZ.Init(buf, buf_data, Defines.MAX_MSGLEN);
            MSG.WriteInt(buf, 0x80000000);
            MSG.WriteInt(buf, 0x12345678);
            MSG.WriteInt(buf, 0x7fffffff);
            MSG.WriteInt(buf, 0xffffffff);
            MSG.WriteByte(buf, 1);
            MSG.WriteByte(buf, 2);
            MSG.WriteByte(buf, 3);
            MSG.WriteByte(buf, 4);
            SZ.Print(buf, "[einz]\\n");
            SZ.Print(buf, "[zwei]...");
            MSG.WriteByte(buf, 0xfe);
            MSG.WriteByte(buf, 4);
            MSG.WriteShort(buf, 32766);
            MSG.WriteShort(buf, 16384);
            MSG.WriteShort(buf, -32768);
            MSG.WriteFloat(buf, (float)Math.PI);
            System.Diagnostics.Debug.WriteLine("Read:" + Integer.ToHexString(MSG.ReadLong(buf)));
            System.Diagnostics.Debug.WriteLine("Read:" + Integer.ToHexString(MSG.ReadLong(buf)));
            System.Diagnostics.Debug.WriteLine("Read:" + Integer.ToHexString(MSG.ReadLong(buf)));
            System.Diagnostics.Debug.WriteLine("Read:" + Integer.ToHexString(MSG.ReadLong(buf)));
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadByte(buf));
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadByte(buf));
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadByte(buf));
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadByte(buf));
            System.Diagnostics.Debug.WriteLine("Read:<" + MSG.ReadString(buf) + ">");
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadByte(buf));
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadByte(buf));
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadShort(buf));
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadShort(buf));
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadShort(buf));
            System.Diagnostics.Debug.WriteLine("Read:" + MSG.ReadFloat(buf));
        }
    }
}