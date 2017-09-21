using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PhiDebugging
{
    public static class PhiCmd
    {
        private static List<TestData> LoadTests(ExistingFileName log_file)
        {
            var tests = new List<TestData>();

            var lines = new List<CodeLine>();
            foreach(var line in GetLines(log_file).Where(l => !string.IsNullOrWhiteSpace(l.Value)))
            {
                if (line.Value.StartsWith("L;"))
                {
                    //L; C:\debugger\Calculator-master\Calculator-master\Source\LoreSoft.Calculator\CalculatorForm.cs; LoreSoft.Calculator.CalculatorForm.Eval; 88;                 answer = _eval.Evaluate(input).ToString();
                    var line_elements = Regex.Matches(line.Value, @"(L;\s)([^;]+;\s)([^;]+;\s)([^;]+;\s)(.+)");
                }
                else if (line.Value.StartsWith("V;"))
                {
                    continue;
                }
                else if (line.Value.StartsWith("TestName;"))
                {
                    continue;
                }
                else if(line.Value.StartsWith("TestEnd;"))
                {

                }
                else
                    Debug.Assert(false, "The log contains an invalid line");
            }

            return tests;
        }

        private static IEnumerable<LogLine> GetLines(ExistingFileName log_file)
        {
            using (FileStream fileStream = File.Open(log_file.Value, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bufferedStream = new BufferedStream(fileStream))
            using (StreamReader streamReader = new StreamReader(bufferedStream))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    yield return new LogLine(line);
                }
            }
        }

        public static void AnalizeLog(string logFile, string analysisLog)
        {
        }
    }
}