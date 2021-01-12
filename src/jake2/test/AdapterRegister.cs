using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp
{
    public class AdapterRegister
    {
        static int id = 0;
        class t0
        {
            public int myid = id++;
            public virtual string Test()
            {
                return ("t0, id = " + myid);
            }
        }

        class t1 : t0
        {
            public override string Test()
            {
                return ("t1, id = " + myid);
            }
        }

        class t2 : t0
        {
            public virtual string Test(int x)
            {
                return ("t2, id = " + myid);
            }
        }

        public static void Main(String[] args)
        {
            System.Diagnostics.Debug.WriteLine("hello world.");
            t1 t1 = new t1();
            t2 t2 = new t2();
            t2 t3 = new t2();
            System.Diagnostics.Debug.WriteLine(t1.Test());
            System.Diagnostics.Debug.WriteLine(t2.Test());
            System.Diagnostics.Debug.WriteLine(t2.Test(5));
            System.Diagnostics.Debug.WriteLine(t3.Test(5));
            System.Diagnostics.Debug.WriteLine("good bye world.");
        }
    }
}