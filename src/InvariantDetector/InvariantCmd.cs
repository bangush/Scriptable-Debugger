using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InvariantDetector
{
    public static class InvariantCmd
    {
        private static string _source_log;
        private static string _result_log;

        private static Lines lines;

        private static Dictionary<VariableName, List<Tuple<VariableValue, VariableValueNullable>>> variables;

        public static void Log ( string source_log, string result_log )
        {
            _source_log = source_log;
            _result_log = result_log;
        }

        public static Lines Compare(string file, int line)
        {
            lines = new Lines() { File_1 = file, Line_1 = line };
            return lines;
        }

        public static void Go()
        {
            variables = new Dictionary<VariableName, List<Tuple<VariableValue, VariableValueNullable>>>();

            var line_1_pattern = $@"(^V;\s{lines.File_1.Replace(@"\", @"\\").Replace(@".", @"\.")};\s[^;]+;\s{lines.Line_1};\s)([^;]+)(;\s)([^$]+)($)";
            var line_2_pattern = $@"(^V;\s{lines.File_2.Replace(@"\", @"\\").Replace(@".", @"\.")};\s[^;]+;\s{lines.Line_2};\s)([^;]+)(;\s)([^$]+)($)";

            foreach (string line in File.ReadLines(_source_log))
            {
                if(Regex.IsMatch(line, line_1_pattern))
                {
                    var match_1 = Regex.Match(line, line_1_pattern);
                    var variableName_1 = new VariableName(match_1.Groups[2].Value);
                    var variableValue_1 = new VariableValue(match_1.Groups[4].Value.Trim('"'));

                    if (!variables.ContainsKey(variableName_1))
                        variables.Add(
                            key: new VariableName(match_1.Groups[2].Value),
                            value: new List<Tuple<VariableValue, VariableValueNullable>>());

                    variables[variableName_1].Add(Tuple.Create(variableValue_1, new VariableValueNullable()));                    
                }
                else if (Regex.IsMatch(line, line_2_pattern))
                {
                    var match_2 = Regex.Match(line, line_2_pattern);
                    var variableName_2 = new VariableName(match_2.Groups[2].Value);
                    var variableValue_2 = new VariableValue(match_2.Groups[4].Value.Trim('"'));

                    if(variables.ContainsKey(variableName_2))
                    {
                        var variable = variables[variableName_2].LastOrDefault(tup => tup.Item2.Valid == Nulled.IsNull);
                        var index = variable == null ? -1 : variables[variableName_2].LastIndexOf(variable);
                        if (variable != null)
                        {
                            variables[variableName_2][index] = Tuple.Create(
                                variables[variableName_2][index].Item1, 
                                new VariableValueNullable(variableValue_2.Value));
                        }
                    }
                }
            }

            File.WriteAllText(_result_log, $"Comparing line {lines.File_1}.{lines.Line_1} to {lines.File_2}.{lines.Line_2}\r\n");

            foreach(var variable in variables)
            {
                var values = variable.Value.Where(v => v.Item2.Valid == Nulled.IsValue).ToList();

                var smallTotals = new List<VariableValueNullable>();

                var varients = new List<Tuple<VariableValue, VariableValueNullable>>();

                foreach (var tup in values)
                    if (tup.Item1.Value == tup.Item2.Value)
                        smallTotals.Add(new VariableValueNullable(tup.Item1.Value));
                    else
                       varients.Add(tup);

                var total = smallTotals.FirstOrDefault();
                if (total == null)
                    total = new VariableValueNullable();
                else if (varients.Count() > 0)
                {
                    total = new VariableValueNullable();
                }
                else
                {
                    foreach(var smallTotal in smallTotals)
                    {
                        if (total.Value != smallTotal.Value)
                        {
                            total = new VariableValueNullable();
                            break;
                        }
                    }
                }

                var result = new StringBuilder();
                result.AppendLine($"{variable.Key.Value}:");
                if (total.Valid == Nulled.IsValue)
                    result.AppendLine($"   always constant: [{total.Value}]");
                else
                {
                    foreach(var small in smallTotals.Distinct(new VariableComparer()))
                        result.AppendLine($"   not changed by function: [{small.Value}]");

                    foreach (var varient in varients.Distinct(new VarientComparer()))
                        result.AppendLine($"   changed from: [{varient.Item1.Value}] to: [{varient.Item2.Value}]");
                }

                File.AppendAllText(_result_log, result.ToString());
            }
        }
    }

    public class VariableComparer : IEqualityComparer<VariableValueNullable>
    {
        public bool Equals(VariableValueNullable x, VariableValueNullable y)
        {
            return x.Valid == y.Valid && x.Value == y.Value;
        }

        public int GetHashCode(VariableValueNullable obj)
        {
            return obj.Valid == Nulled.IsNull ? -1 : obj.Value.GetHashCode();
        }
    }

    public class VarientComparer : IEqualityComparer<Tuple<VariableValue, VariableValueNullable>>
    {
        public bool Equals(Tuple<VariableValue, VariableValueNullable> x, Tuple<VariableValue, VariableValueNullable> y)
        {
            return x.Item1.Value == y.Item1.Value && x.Item2.Value == y.Item2.Value;
        }

        public int GetHashCode(Tuple<VariableValue, VariableValueNullable> obj)
        { 
            return obj.Item1.Value.GetHashCode() * 31 + obj.Item2.Value.GetHashCode(); ;
        }
    }
}
