﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "Data1.txt";
            DataLoader loader = new DataLoader(fileName);
            List<Example> examples = loader.loadData();
            string groupClass = "T1";
            Inference inference = new Inference(examples, groupClass);
            List<LogicalArgument> rule = inference.ruleInference();
            inference.printRule(rule);
        }
    }
}