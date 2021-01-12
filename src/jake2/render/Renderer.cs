using Jake2.Client;
using Jake2.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jake2.Render
{
    public class Renderer
    {
        static IRenderAPI fastRenderer = new Fast.Misc();
        static IRenderAPI basicRenderer = new Basic.Misc();
        static List<IRef> drivers = new List<IRef>(3);

        public static void Register(IRef impl)
        {
            if (impl == null)
            {
                throw new ArgumentException("Ref implementation can't be null");
            }

            if (!drivers.Contains(impl))
            {
                drivers.Add(impl);
            }
        }

        public static Irefexport_t GetDriver(string driverName)
        {
            return GetDriver(driverName, true);
        }

        public static Irefexport_t GetDriver(string driverName, bool fast)
        {
            IRef driver = null;
            int count = drivers.Count;
            for (int i = 0; i < count; i++)
            {
                driver = (IRef)drivers[i];
                if (driver.GetName().Equals(driverName))
                {
                    return driver.GetRefAPI((fast) ? fastRenderer : basicRenderer);
                }
            }

            return null;
        }

        public static string GetDefaultName()
        {
            return (drivers.Count == 0) ? null : ((IRef)drivers.First()).GetName();
        }

        public static string GetPreferedName()
        {
            return (drivers.Count == 0) ? null : ((IRef)drivers.Last()).GetName();
        }

        public static String[] GetDriverNames()
        {
            if (drivers.Count == 0)
                return null;
            int count = drivers.Count;
            String[] names = new string[count];
            for (int i = 0; i < count; i++)
            {
                names[i] = ((IRef)drivers[i]).GetName();
            }

            return names;
        }
    }
}