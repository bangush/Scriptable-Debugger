using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace PhiDebugging
{
    public static class PhiCmd
    {
        private static List<TestData> LoadTests(IEnumerable<LogLine> log_lines)
        {
            var tests = new List<TestData>();
            var testData = new TestData();

            foreach(var line in log_lines.Where(l => !string.IsNullOrWhiteSpace(l.Value)))
            {
                if (line.Value.StartsWith("L;"))
                {
                    var line_elements = Regex.Match(line.Value, @"(L;\s)([^;]+;\s)([^;]+;\s)([^;]+;\s)(.+)");
                    testData.Lines.Add(
                        new Tuple<LineNumber, FileName, CodeLine>(
                            new LineNumber(int.Parse(line_elements.Groups[4].Value.Trim().TrimEnd(';'))),
                            new FileName(line_elements.Groups[2].Value.Trim().TrimEnd(';')),
                            new CodeLine(line_elements.Groups[5].Value)));
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
                    var test_result = Regex.Match(line.Value, @"(TestEnd;\s)(.)");
                    switch(test_result.Groups[2].Value)
                    {
                        case "p":
                            testData.Result = TestResult.Pass;
                            break;
                        case "f":
                            testData.Result = TestResult.Fail;
                            break;
                        default:
                            Debug.Assert(false, "Tests can only have 'p'(pass) or 'f'(fail) result");
                            break;
                    }

                    tests.Add(testData);
                    testData = new TestData();
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

        public static void Analise_Log(string log_file, string analysis_log)
        {
            var log_lines = GetLines(new ExistingFileName(log_file));
            var tests = LoadTests(log_lines);

            Console.WriteLine($"{tests.Count} tests found.");

            var code_list = new CodeList();

            foreach (var test in tests)
            {
                foreach (var line in test.Lines)
                    code_list.Increment(line.Item1, line.Item2, line.Item3, TestAction.Covered, test.Result);
            }

            code_list.Order_lines();

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("------- Ordered Lines -------");
            Console.ResetColor();

            
            using (StreamWriter streamWriter = File.CreateText(analysis_log))
            {
                foreach (var line in code_list.OrderedLines)
                {
                    streamWriter.WriteLine($"{line.Item1.Value}; {line.Item2.Value}; {line.Item4.Phi}; {line.Item3.Value}");
                    Console.WriteLine($"{Regex.Match(line.Item2.Value, $@"[^\\]+$").Value} | {line.Item1.Value} | {line.Item4.Phi}");
                }
            }                
        }
    }
}