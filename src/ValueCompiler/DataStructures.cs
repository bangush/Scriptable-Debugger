using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Debugging.MdbgEngine
{
    public struct ComplexDataStructure
    {
        public string Type { get; }

        public Dictionary <string, object> Parameters { get; }

        public ComplexDataStructure(string type, Dictionary<string, object> parameters = null)
        {
            Type = type;
            Parameters = parameters == null ? new Dictionary<string, object>() : new Dictionary<string, object>(parameters);
        }
    }    
}
