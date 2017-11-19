using System.Collections.Generic;

namespace AQ11Console
{
    public class AttributeValueMap
    {
        // Represents a map containing data about a certain attribute:
        // Name of the attribute
        public string name { get; private set; }
        // Type of the attribute
        public AttributeType type { get; private set; }
        // All values supported by that attribute
        public List<string> values { get; private set; }

        public AttributeValueMap(string name, AttributeType type)
        {
            this.name = name;
            this.type = type;
            values = new List<string>();
        }
    }
}
