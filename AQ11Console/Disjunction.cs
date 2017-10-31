﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public class Disjunction : LogicalArgument
    {
        public List<LogicalArgument> arguments { get; private set; }

        public Disjunction(List<LogicalArgument> arguments)
        {
            this.arguments = arguments;
        }

        public string toString()
        {
            string output = "(";
            for (int i = 0; i < arguments.Count; i++)
            {
                if (i < arguments.Count - 1)
                    output = string.Concat(output, arguments[i].toString() + " v ");
                else
                    output = string.Concat(output, arguments[i].toString());                     
            }
            output = string.Concat(output, ")");

            return output;
        }

        public bool isEqual(LogicalArgument argument)
        {
            if (argument.GetType() != GetType())
            {
                return false;
            }
            else
            {
                Disjunction castArgument = (Disjunction)argument;
                if (arguments.Count != castArgument.arguments.Count)
                    return false;
                for (int i = 0; i < arguments.Count; i++)
                {
                    if (arguments[i].isEqual(castArgument.arguments[i]) == false)
                        return false; 
                }
                return true;
            }
        }
    }
}
