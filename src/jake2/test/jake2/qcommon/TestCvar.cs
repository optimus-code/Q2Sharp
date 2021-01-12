using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Qcommon
{
    public class TestCvar
    {
        public static void Main(String[] args)
        {
            Cvar.Set("rene", "is cool.");
            Com.Printf("rene:" + Cvar.FindVar("rene").string_renamed);
        }
    }
}