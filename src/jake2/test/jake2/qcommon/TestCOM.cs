using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Qcommon
{
    public class TestCOM
    {
        public static void Main(string[] args)
        {
            string test = "testrene = \\\"ein mal eins\\\"; a=3 ";
            Com.ParseHelp ph = new ParseHelp(test);
            while (!ph.IsEof())
                System.Diagnostics.Debug.WriteLine("[" + Com.Parse(ph) + "]");
            System.Diagnostics.Debug.WriteLine("OK!");
            test = " testrene = \\\"ein mal eins\\\"; a=3";
            ph = new ParseHelp(test);
            while (!ph.IsEof())
                System.Diagnostics.Debug.WriteLine("[" + Com.Parse(ph) + "]");
            System.Diagnostics.Debug.WriteLine("OK!");
        }
    }
}