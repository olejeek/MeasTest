using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeasTest
{
    public class CommandView
    {
        public readonly string Description;
        public readonly string CommandText;
        public readonly bool ValueHave;
        public readonly bool UnitsHave;
        public readonly bool Responsed;

        public CommandView (string source)
        {
            Description = "Test Command";
            CommandText = "TEST?";
            ValueHave = false;
            UnitsHave = false;
        }
    }
}
