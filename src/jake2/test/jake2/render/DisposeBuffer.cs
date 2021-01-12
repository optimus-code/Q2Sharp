using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Render
{
    public class DisposeBuffer
    {
        static int SIZE = 1024 * 1024;
        static int COUNT = 160;
        public static void Main(String[] args)
        {
            System.Diagnostics.Debug.WriteLine("DirectBuffer allocation.");
            Buffer[] buf = new Buffer[COUNT];
            Runtime run = Runtime.GetRuntime();
            System.Gc();
            for (int i = 0; i < COUNT; i++)
            {
                buf[i] = ByteBuffer.AllocateDirect(SIZE);
            }

            System.Gc();
            System.Diagnostics.Debug.WriteLine((run.TotalMemory() / 1024) + "KB heap");
            try
            {
                Thread.Sleep(10000);
            }
            catch (InterruptedException e)
            {
            }

            System.Diagnostics.Debug.WriteLine("DirectBuffer dispose.");
            for (int i = 0; i < COUNT; i++)
            {
                buf[i] = null;
            }

            System.Gc();
            System.Diagnostics.Debug.WriteLine((run.TotalMemory() / 1024) + "KB heap");
            try
            {
                Thread.Sleep(20000);
            }
            catch (InterruptedException e)
            {
            }
        }
    }
}