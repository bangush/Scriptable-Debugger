using System;
using System.Text;

namespace Microsoft.Samples.Tools.Mdbg
{
    public class LogBuffer
    {
        private StringBuilder _buffer = new StringBuilder();
        private int _lineCount = 0;
        private Action _onBufferFull;

        public LogBuffer(Action onBufferFull)
        {
            _onBufferFull = onBufferFull;
        }

        public string Content
        {
            get => _buffer.ToString();
        }

        public int Limit { get; set; } = 100;

        public void Line(string line)
        {
            _buffer.AppendLine(line);
            _lineCount++;

            if (_lineCount >= Limit)
            {
                _onBufferFull?.Invoke();
                _buffer.Clear();
            }
        }
    }
}