using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasTest
{
    class ScpiCommand
    {
        public CommandView Command;
        public MeasInstrument Instrument;

        public ScpiCommand(MeasInstrument instrument, CommandView command)
        {
            Command = command;
            Instrument = instrument;
        }
    }
}
