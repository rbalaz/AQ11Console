using System;
using System.Collections.Generic;

namespace AQ11Console
{
    public class Example
    {
        // Represents an example object containing data about attributes and class
        public List<Attribute> attributes { get; private set; }

        public string groupClass { get; private set; }

        public Example(List<Attribute> attributes, string groupClass)
        {
            this.attributes = attributes;
            this.groupClass = groupClass;
        }

        public void printExample()
        {
            string toPrint = "Example: ";
            foreach (Attribute atr in attributes)
                toPrint += "| " + atr.name + " = " + atr.value + " ";
            toPrint += "| Class = " + groupClass;

            Console.WriteLine(toPrint);
        }
    }
}
