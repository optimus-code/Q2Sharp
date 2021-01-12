using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Qcommon
{
    public sealed class cmd_function_t
    {
        public cmd_function_t next = null;
        public string name = null;
        public xcommand_t function;
    }
}