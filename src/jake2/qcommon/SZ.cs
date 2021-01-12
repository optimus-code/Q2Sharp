using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Qcommon
{
    public sealed class SZ
    {
        public static void Clear(sizebuf_t buf)
        {
            buf.Clear();
        }

        public static void Init(sizebuf_t buf, byte[] data, int length)
        {
            buf.readcount = 0;
            buf.data = data;
            buf.maxsize = length;
            buf.cursize = 0;
            buf.allowoverflow = buf.overflowed = false;
        }

        public static int GetSpace(sizebuf_t buf, int length)
        {
            int oldsize;
            if (buf.cursize + length > buf.maxsize)
            {
                if (!buf.allowoverflow)
                    Com.Error(Defines.ERR_FATAL, "SZ_GetSpace: overflow without allowoverflow set");
                if (length > buf.maxsize)
                    Com.Error(Defines.ERR_FATAL, "SZ_GetSpace: " + length + " is > full buffer size");
                Com.Printf("SZ_GetSpace: overflow\\n");
                Clear(buf);
                buf.overflowed = true;
            }

            oldsize = buf.cursize;
            buf.cursize += length;
            return oldsize;
        }

        public static void Write(sizebuf_t buf, byte[] data, int length)
        {
            System.Array.Copy(data, 0, buf.data, GetSpace(buf, length), length);
        }

        public static void Write(sizebuf_t buf, byte[] data, int offset, int length)
        {
            System.Array.Copy( data, offset, buf.data, GetSpace(buf, length), length);
        }

        public static void Write(sizebuf_t buf, byte[] data)
        {
            int length = data.Length;
            System.Array.Copy( data, 0, buf.data, GetSpace(buf, length), length);
        }

        public static void Print(sizebuf_t buf, string data)
        {
            Com.Dprintln("SZ.print():<" + data + ">");
            int length = data.Length;
            byte[] str = Lib.StringToBytes(data);
            if (buf.cursize != 0)
            {
                if (buf.data[buf.cursize - 1] != 0)
                {
                    System.Array.Copy( str, 0, buf.data, GetSpace(buf, length + 1), length);
                }
                else
                {
                    System.Array.Copy( str, 0, buf.data, GetSpace(buf, length) - 1, length);
                }
            }
            else
                System.Array.Copy( str, 0, buf.data, GetSpace(buf, length), length);
            buf.data[buf.cursize - 1] = 0;
        }
    }
}