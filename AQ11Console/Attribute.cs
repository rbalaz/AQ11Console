using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public class Attribute
    {
        public AttributeType type { get; private set; }
        public string value { get; set; }
        public string name { get; private set; }

        public Attribute(AttributeType type, string name, string value)
        {
            this.value = value;
            this.name = name;
            this.type = type;
        }

        public Attribute(AttributeType type, string name)
        {
            this.name = name;
            this.type = type;
        }

        public Attribute cloneValuelessAttributeWithValue(string value)
        {
            Attribute clone = new Attribute(type, name, value);
            return clone;
        }
    }
}
