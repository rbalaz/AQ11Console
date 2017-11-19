namespace AQ11Console
{
    public class Variable : LogicalArgument
    {
        // Represents a variable in a logical formula with a given name
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
            // Tests if the parameter argument is of the same type 
            // and has the same name
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
