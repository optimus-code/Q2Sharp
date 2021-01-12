using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Qcommon
{
    public class TestMD4
    {
        public static void Main(string[] args)
        {
            Test("");
            Test("a");
            Test("abc");
            Test("abcdefghijklmnopqrstuvwxyz");
            Test("hi");
            MD4 md4 = new MD4();
            byte[] data = new[]{(byte)0x71, (byte)0xa9, (byte)0x05, (byte)0xce, (byte)0x8d, (byte)0x75, (byte)0x28, (byte)0xc8, (byte)0xba, (byte)0x97, (byte)0x45, (byte)0xe9, (byte)0x8a, (byte)0xe0, (byte)0x37, (byte)0xbd, (byte)0x6c, (byte)0x6d, (byte)0x67, (byte)0x4a, (byte)0x21};
            System.Diagnostics.Debug.WriteLine("checksum=" + MD4.Com_BlockChecksum(data, 21));
        }

        public static void Test(string s)
        {
            MD4 md4 = new MD4();
            md4.EngineUpdate(s.GetBytes(), 0, s.Length);
            System.Diagnostics.Debug.WriteLine("\\\"" + s + "\\\"");
            System.out_renamed.Print(Lib.HexDump(md4.EngineDigest(), 16, false));
        }
    }
}