using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MeasTest
{
    class MeasInstrument
    {
        public readonly List<CommandView> Commands;
        public readonly string Name;
        public string Address;
        public bool IsConnected { get; private set; }

        public MeasInstrument(StreamReader reader)
        {
            
        }
    }
}
