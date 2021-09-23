using System;
using System.IO;
using System.Text;
using DzNet6;

namespace PolymorphismArchitecture.Log
{
    class FileLogger : ILog
    {
        private readonly TextWriter _writer;

        public FileLogger(string path)
        {
            _writer = new StreamWriter(path,true,Encoding.Unicode);
        }
        public void Print(string line)
        {
            _writer.WriteLine(line);
            _writer.Flush();
        }
    }
}
