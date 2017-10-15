using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public class Constant : LogicalArgument
    {
        public string value { get; private set; }

        public Constant(string value)
        {
            this.value = value;
        }

        public string toString()
        {
            return value;
        }

        public bool isEqual(LogicalArgument argument)
        {
            if (argument.GetType() != GetType())
            {
                return false;
            }
            else
            {
                Constant castArgument = (Constant)argument;
                if (value == castArgument.value)
                    return true;
                else
                    return false;
            }
        }
    }
}
