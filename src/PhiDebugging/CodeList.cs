using System;
using System.Collections.Generic;

namespace PhiDebugging
{
    public class CodeList
    {
        public CodeList()
        {
            Lines = new Dictionary<Tuple<LineNumber, ExistingFileName>, PhiMatrix>();
        }

        public Dictionary<Tuple<LineNumber, ExistingFileName>, PhiMatrix> Lines { get; }

        public void Increment(LineNumber line, ExistingFileName file, TestAction action, TestResult result)
        {
            var lineItem = Tuple.Create(line, file);
            if (!Lines.ContainsKey(lineItem))
                Lines.Add(lineItem, PhiMatrix.Create());

            Lines[lineItem].Increment(action, result);
        }
    }
}