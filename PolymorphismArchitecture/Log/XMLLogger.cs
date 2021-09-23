using System;
using System.Text;
using System.Xml;

namespace PolymorphismArchitecture.Log
{
    class XmlLogger : ILog
    {
        private readonly XmlWriter _writer;

        public XmlLogger(string path)
        {
            _writer = new XmlTextWriter("yaya.xml",Encoding.Unicode);
        }
        public void Print(string line)
        {
            
        }
    }
}
