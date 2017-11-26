using System.Collections.Generic;

namespace AQ11Console
{
    public class Constant : LogicalArgument
    {
        // Represents a constant in a logical formula with a given value
        public List<string> valueSet { get; private set; }

        public Constant(List<string> valueSet)
        {
            this.valueSet = valueSet;
        }

        public Constant(string value)
        {
            valueSet = new List<string>();
            valueSet.Add(value);
        }

        public string toString()
        {
            if (valueSet.Count == 1)
                return valueSet[0];
            string buildString = "{ ";
            for (int i = 0; i < valueSet.Count; i++)
            {
                buildString += valueSet[i];
                if (i < valueSet.Count - 1)
                    buildString += ", ";
            }
            buildString += " }";
            return buildString;
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
                foreach (string value in castArgument.valueSet)
                {
                    if (valueSet.Contains(value) == false)
                        return false;
                }
                return true;
            }
        }
    }
}
