using System.Collections.Generic;

namespace AQ11Console
{
    public class Disjunction : LogicalArgument
    {
        // Wrapper object representing disjunction of a set of logical arguments
        public List<LogicalArgument> arguments { get; private set; }

        public Disjunction(List<LogicalArgument> arguments)
        {
            this.arguments = arguments;
        }

        public string toString()
        {
            string output = "(";
            for (int i = 0; i < arguments.Count; i++)
            {
                if (i < arguments.Count - 1)
                    output = string.Concat(output, arguments[i].toString() + " OR ");
                else
                    output = string.Concat(output, arguments[i].toString());                     
            }
            output = string.Concat(output, ")");

            return output;
        }

        public bool isEqual(LogicalArgument argument)
        {
            // Tests if every aspect of the parameter argument matches
            // its counterpart in this object, recursively testing all the
            // way down to variables and constants
            if (argument.GetType() != GetType())
            {
                return false;
            }
            else
            {
                Disjunction castArgument = (Disjunction)argument;
                if (arguments.Count != castArgument.arguments.Count)
                    return false;
                for (int i = 0; i < arguments.Count; i++)
                {
                    if (arguments[i].isEqual(castArgument.arguments[i]) == false)
                        return false; 
                }
                return true;
            }
        }
    }
}
