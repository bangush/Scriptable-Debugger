using System.IO;

namespace Microsoft.Samples.Tools.Mdbg
{
    public static class Log
    {
        public static string LogFilePath;
        public static bool WriteToLog = true;
        private static LogBuffer _buffer;

        /// <summary>
        /// Line number, FiletPath, Function Name
        /// </summary>
        public static LocationState LocationState { get; set; }

        public static void StartBuffer()
        {
            _buffer = new LogBuffer(() => WriteThisToLog(_buffer.Content));
        }

        public static void StopBuffer()
        {
            WriteThisToLog(_buffer.Content);
            _buffer = null;
        }
        public static void WriteThisToLog(LocationState locationState)
        {
            if (locationState.File != null && locationState.LineNumber > -1)
                LocationState = locationState;

            if (locationState != null && WriteToLog && locationState.ProcessState == ProcessStateEnum.Running && locationState.FileName != string.Empty)
            {
                var text = $"L; {locationState.File}; {locationState.Function}; {locationState.LineNumber}; {locationState.Code}";

                if (_buffer != null)
                    _buffer.Line(text);
                else
                    File.AppendAllLines(LogFilePath, new string[] { text });
            }
        }

        public static void WriteThisToLog(string buffer)
        {
            if (WriteToLog)
            {
                File.AppendAllText(LogFilePath, buffer.ToString());
            }
        }

        public static string WriteThisToLog(string variableName = null, string variableValue = null)
        {
            if (WriteToLog)
            {
                var text = $"V; {LocationState.File}; {LocationState.Function}; {LocationState.LineNumber}; {variableName}; \"{variableValue}\"";

                if (_buffer != null)
                    _buffer.Line(text);
                else
                    File.AppendAllLines(LogFilePath, new string[] { text });
            }

            return variableValue;
        }
    }
}