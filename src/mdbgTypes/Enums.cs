using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Tools.Mdbg
{
     //Print(bool debuggerVars = false, bool canDoFunceval = false, int expandDepth = -1)

    public enum PrintDebugVarsOptions
    {
        Print_debug_vars,
        Do_not_print_debug_vars
    }

    public enum TestName
    {
        Ask_for_name,
        Empty
    }

    public enum LoggingAction
    {
        Append,
        Overwrite
    }
}
