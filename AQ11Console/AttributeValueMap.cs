using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public class AttributeValueMap
    {
        public string name { get; private set; }
        public AttributeType type { get; private set; }
        public List<string> values { get; private set; }

        public AttributeValueMap(string name, AttributeType type)
        {
            this.name = name;
            this.type = type;
            values = new List<string>();
        }
    }
}
