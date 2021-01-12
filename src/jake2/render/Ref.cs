using Q2Sharp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Render
{
    public interface IRef
    {
        Irefexport_t GetRefAPI( IRenderAPI renderer );
        string GetName();
    }
}