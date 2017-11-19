namespace AQ11Console
{
    public class Constant : LogicalArgument
    {
        // Represents a constant in a logical formula with a given value
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
            // Tests if the parameter argument is of the same type and
            // contains the same value
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
