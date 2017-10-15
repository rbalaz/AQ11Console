using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public class Variable : LogicalArgument
    {
        public string name { get; private set; }

        public Variable(string name)
        {
            this.name = name;
        }

        public string toString()
        {
            return name;
        }

        public bool isEqual(LogicalArgument argument)
        {
            if (argument.GetType() != GetType())
            {
                return false;
            }
            else
            {
                Variable castArgument = (Variable)argument;
                if (name == castArgument.name)
                    return true;
                else
                    return false;
            }
        }
    }
}
