using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InvariantDetector
{
    public interface NonNullable<T>
    {
        T Value { get; }
    }

    public enum Nulled
    {
        IsNull,
        IsValue
    }

    public class VariableValueNullable : Nullable<string>
    {
        public Nulled Valid { get; }

        public string Value { get; }

        public VariableValueNullable(string value)
        {
            Value = value != null ? value : throw new ArgumentOutOfRangeException();
            Valid = Nulled.IsValue;
        }

        public VariableValueNullable()
        {
            Value = null;
            Valid = Nulled.IsNull;
        }

        public override string ToString()
        {
            return Valid == Nulled.IsValue ? Value : "Null";
        }
    }

    public struct LineNumber : NonNullable<int>
    {
        public int Value { get; }

        public LineNumber(int line)
        {
            Value = line > 0 ? line : throw new ArgumentOutOfRangeException();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public struct FileName : NonNullable<string>
    {
        public string Value { get; }

        public FileName(string name)
        {
            Value = Validation.FileName(name);
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public struct VariableName : NonNullable<string>
    {
        public string Value { get; }

        public VariableName(string name)
        {
            Value = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentOutOfRangeException();
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public struct VariableValue : NonNullable<string>
    {
        public string Value { get; }

        public VariableValue(string value)
        {
            Value = value != null ? value : throw new ArgumentOutOfRangeException();
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public interface Nullable<T>
    {
        Nulled Valid { get; }

        T Value { get; }
    }


    public enum ValidationState
    {
        Valid,
        Invalid
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
}
