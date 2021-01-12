using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Qcommon
{
    public class TestLoadMap
    {
        public static void Main(String[] args)
        {
            Com.DPrintf("hello!\\n");
            FS.InitFilesystem();
            CM.CM_LoadMap("maps/base1.bsp", true, new int {0});
        }
    }
}