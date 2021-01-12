using Jake2.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Render
{
    public interface IRef
    {
        Irefexport_t GetRefAPI( IRenderAPI renderer );
        string GetName();
    }
}