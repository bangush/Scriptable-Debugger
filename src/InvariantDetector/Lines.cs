using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvariantDetector
{
    public class Lines
    {
        public string File_1 { get; set; }

        public int Line_1 { get; set; }

        public string File_2 { get; set; }
        
        public int Line_2 { get; set; }

        public void To(string file, int line)
        {
            File_2 = file;
            Line_2 = line;
        }
    }
}
