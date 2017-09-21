using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Tools.Mdbg
{
    public static class Log
    {
        public static bool WriteToLog = true;

        public static string LogFilePath;

        public static StringBuilder Buffer;

        /// <summary>
        /// Line number, FiletPath, Function Name
        /// </summary>
        public static LocationState LocationState { get; set; }

        public static void WriteThisToLog(LocationState locationState)
        {
            if(locationState.File != null && locationState.LineNumber > -1)
                LocationState = locationState;

            if (locationState != null && WriteToLog && locationState.ProcessState == ProcessStateEnum.Running && locationState.FileName != string.Empty)
            {
                var text = $"L; {locationState.File}; {locationState.Function}; {locationState.LineNumber}; {locationState.Code}";

                if (Buffer != null)
                    Buffer.AppendLine(text);
                else
                    File.AppendAllLines(LogFilePath, new string[] { text });
            }
        }

        public static void WriteThisToLog(StringBuilder buffer)
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

                if (Buffer != null)
                    Buffer.AppendLine(text);
                else
                    File.AppendAllLines(LogFilePath, new string[] { text });
            }

            return variableValue;
        }
    }
}
