using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PhiDebugging
{
    public class CodeList
    {
        public CodeList()
        {
            Lines = new Dictionary<Tuple<LineNumber, FileName, CodeLine>, PhiMatrix>();
        }

        public Dictionary<Tuple<LineNumber, FileName, CodeLine>, PhiMatrix> Lines { get; }

        public void Increment(LineNumber line, FileName file, CodeLine codeLine, TestAction action, TestResult result)
        {
            var lineItem = Tuple.Create(line, file, codeLine);
            if (!Lines.ContainsKey(lineItem))
                Lines.Add(lineItem, new PhiMatrix());

            Lines[lineItem].Increment(action, result);

            Console.WriteLine($"{Regex.Match(file.Value, $@"[^\\]+$").Value} | {line} | {Lines[lineItem].Phi}");
        }

        public List<Tuple<LineNumber, FileName, CodeLine, PhiMatrix>> Order_lines()
        {
            var lines = Lines.Keys
                .Select(k => Tuple.Create(k.Item1, k.Item2, k.Item3, Lines[k]))
                .OrderBy(l => l.Item4.Phi)
                .ToList();

            orderedLines = lines;
            return lines;
        }

        private List<Tuple<LineNumber, FileName, CodeLine, PhiMatrix>> orderedLines;

        public List<Tuple<LineNumber, FileName, CodeLine, PhiMatrix>> OrderedLines
        {
            get
            {
                if (orderedLines == null)
                    Order_lines();

                return orderedLines;
            }
        }
    }
}