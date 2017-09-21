using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhiDebugging
{
    public class TestData
    {
        public List<Tuple<LineNumber, ExistingFileName, CodeLine>> Lines { get; } = new List<Tuple<LineNumber, ExistingFileName, CodeLine>>();

        public TestResult Result { get; set; }
    }
}
