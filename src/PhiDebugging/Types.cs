using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PhiDebugging
{

    public enum ValidationState
    {
        Valid,
        Invalid
    }

    public enum Nulled
    {
        IsNull,
        IsValue
    }

    public static class Validation
    {
        public static int IntIsNatural(int value)
        {
            return value >= 0 ? value : throw new ArgumentOutOfRangeException();
        }

        public static string FileExists(string path)
        {
            return File.Exists(path) ? path : throw new FileNotFoundException();
        }

        public static string FileName(string path)
        {
            return !string.IsNullOrWhiteSpace(path) ? path : throw new ArgumentNullException();
        }    
        
        public static string NotNull(string str)
        {
            return str != null ? str : throw new ArgumentNullException();
        }
    }

    public interface Nullable<T>
    {
        Nulled Valid { get; }

        T Value { get; }
    }

    public interface NonNullable<T>
    {
        T Value { get; }
    }

    public struct IntCount : NonNullable<int>
    {
        public int Value { get; }

        public IntCount(int startingCount)
        {
            Value = Validation.IntIsNatural(startingCount);
        }

        public IntCount Increment()
        {
            return new IntCount(Value + 1);
        }
    }

    public struct LineNumber : NonNullable<int>
    {
        public int Value { get; }

        public LineNumber(int number)
        {
            Value = Validation.IntIsNatural(number);
        }
    }

    public struct ExistingFileName : NonNullable<string>
    {
        public string Value { get; }

        public ExistingFileName(string name)
        {
            Value = Validation.FileExists(name);
        }
    }

    public struct FileName :NonNullable<string>
    {
        public string Value { get; }

        public FileName(string name)
        {
            Value = Validation.FileName(name);
        }
    }

    public struct CodeLine : NonNullable<string>
    {
        public string Value { get; }

        public CodeLine(string code)
        {
            Value = Validation.NotNull(code);
        }
    }

    public struct LogLine : NonNullable<string>
    {
        public string Value { get; }

        public LogLine(string line)
        {
            Value = Validation.NotNull(line);
        }
    }
}
