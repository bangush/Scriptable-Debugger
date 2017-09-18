using System.IO;

namespace Microsoft.Samples.Tools.Mdbg
{
    public enum ProcessStateEnum
    {
        Stop,
        Running
    }

    public class LocationState
    {
        public LocationState(ProcessStateEnum processState, string file = "", int lineNumber = -1, string code = "", string message = "", string function = "")
        {
            this.ProcessState = processState;
            this.File = file;
            this.FileName = Path.GetFileName(file);
            this.LineNumber = lineNumber;
            this.Code = code;
            this.Message = message;
            this.Function = function;
        }

        public string Code { get; }

        public string File { get; }

        public string FileName { get; }

        public int LineNumber { get; }

        public string Message { get; }

        public ProcessStateEnum ProcessState { get; }

        public string Function { get; }
    }
}