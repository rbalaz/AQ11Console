using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public class Equality: LogicalArgument
    {
        public LogicalArgument firstArgument { get; private set; }
        public LogicalArgument secondArgument { get; private set; }

        public Equality(LogicalArgument firstArgument, LogicalArgument secondArgument)
        {
            this.firstArgument = firstArgument;
            this.secondArgument = secondArgument;
        }

        public string toString()
        {
            return "(" + firstArgument.toString() + "=" + secondArgument.toString() + ")";
        }

        public bool isEqual(LogicalArgument argument)
        {
            if (argument.GetType() != GetType())
            {
                return false;
            }
            else
            {
                Equality castArgument = (Equality)argument;
                if (firstArgument.isEqual(castArgument.firstArgument) == false)
                    return false;
                else if (secondArgument.isEqual(castArgument.secondArgument) == false)
                    return false;
                else
                    return true;
            }
        }
    }
}
