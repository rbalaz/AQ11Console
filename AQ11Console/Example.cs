using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public class Example
    {
        public List<Attribute> attributes { get; private set; }

        public Example(List<Attribute> attributes)
        {
            this.attributes = attributes;
        }
    }
}
