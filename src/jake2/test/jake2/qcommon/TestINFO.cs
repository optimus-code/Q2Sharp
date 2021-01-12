using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Qcommon
{
    public class TestINFO
    {
        public static void Main(string[] args)
        {
            string test = "\\\\key1\\\\value 1\\\\key 2 \\\\value2\\\\key3\\\\ v a l u e 3\\\\key4\\\\val ue 4";
            Info.Print(test);
            test = Info.Info_RemoveKey(test, "key1");
            Info.Print(test);
        }
    }
}