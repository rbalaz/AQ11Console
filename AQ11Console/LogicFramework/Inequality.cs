namespace AQ11Console
{
    public class Inequality : LogicalArgument
    {
        // Wrapper object representing an inequality of a variable and constant
        // First argument must be a variable
        public LogicalArgument firstArgument { get; private set; }
        // Second argument must be a constant
        public LogicalArgument secondArgument { get; private set; }

        public Inequality(LogicalArgument firstArgument, LogicalArgument secondArgument)
        {
            this.firstArgument = firstArgument;
            this.secondArgument = secondArgument;
        }

        public string toString()
        {
            return "(" + firstArgument.toString() + " ≠ " + secondArgument.toString() + ")";
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
                Inequality castArgument = (Inequality)argument;
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
