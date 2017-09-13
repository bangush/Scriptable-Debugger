using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Debugging.MdbgEngine
{
    public static class PrintTypes
    {
        public static Dictionary<string, PrintInfo> PrintableTypes { get; } = new Dictionary<string, PrintInfo>()
        {
            { "System.Text.StringBuilder", new PrintInfo("System.Text.StringBuilder", 2, (data) => StringBuilderPrint.Print(data)) }
        };
    }

    public struct PrintInfo
    {
        public string Type { get; }
        public int Level { get; }
        public Func<ComplexDataStructure, string> Print { get; }

        public PrintInfo(string type, int level, Func<ComplexDataStructure, string> print)
        {
            Type = type;
            Level = level;
            Print = print;
        }
    }

    public static class StringBuilderPrint
    {
        public static string Print(ComplexDataStructure data)
        {
            return $"\"{((string[])data.Parameters["m_ChunkChars"]).Aggregate("", (agg, c) => $"{agg}{c}").Trim('\0')}\"";
        }
    }
}