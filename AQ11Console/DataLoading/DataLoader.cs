using System;
using System.Collections.Generic;
using System.IO;

namespace AQ11Console
{
    public class DataLoader
    {
        // Represents a loader object responsible for loading training set from a given file
        
        // Exception thrown when file was not found
        class FileNotFoundException : Exception { };
        // Exception thrown when file format is not supported by the loader
        class InvalidDataFileException : Exception { };

        private string fileName;

        public DataLoader(string fileName)
        {
            this.fileName = fileName;
        }

        public List<Example> loadData()
        {
            // Loader supports .csv file with this metadata:
            // Attributes:
            // Name_of_the_attribute,type_of_the_attribute
            // Examples:
            // attribute_value,...,class_value
            if (File.Exists(fileName) == false)
                throw new FileNotFoundException();

            FileStream reader = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            StreamReader stream = new StreamReader(reader);

            string line = stream.ReadLine();
            if (line.Contains("Attributes") == false)
                throw new InvalidDataFileException();

            List<Attribute> attributes = new List<Attribute>();
            line = stream.ReadLine();
            while (line.Contains("Examples") == false)
            {
                string[] attributeData = line.Split(',');
                AttributeType type = attributeData[1] == "Nominal" ? AttributeType.NOMINAL : AttributeType.NUMERIC;
                Attribute attribute = new Attribute(type, attributeData[0]);
                attributes.Add(attribute);
                line = stream.ReadLine();
            }

            line = stream.ReadLine();
            List<Example> examples = new List<Example>();
            while (line != null)
            {
                string[] exampleData = line.Split(',');
                List<Attribute> initializedAttributes = new List<Attribute>();
                for (int i = 0; i < exampleData.Length - 1; i++)
                {
                    Attribute attribute = attributes[i].cloneValuelessAttributeWithValue(exampleData[i]);
                    initializedAttributes.Add(attribute);
                }
                Example example = new Example(initializedAttributes, exampleData[exampleData.Length - 1]);
                examples.Add(example);
                line = stream.ReadLine();
            }

            return examples;
        }
    }
}
