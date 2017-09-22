using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhiDebugging
{
    public class TestData
    {
        public List<Tuple<LineNumber, FileName, CodeLine>> Lines { get; } = new List<Tuple<LineNumber, FileName, CodeLine>>();

        public TestResult Result { get; set; }
    }
}
