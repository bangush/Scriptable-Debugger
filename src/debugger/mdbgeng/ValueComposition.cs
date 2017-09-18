using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Debugging.MdbgEngine
{
    public class ValueComposition
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public List<ValueComposition> Components { get; set; }

        public ValueComposition()
        {
            Components = new List<ValueComposition>();
        }
    }
}
