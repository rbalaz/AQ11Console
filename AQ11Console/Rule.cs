using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public class Rule
    {
        private List<Conjunction> conjuncts = new List<Conjunction>();
        private string groupClass;

        public Rule(List<Conjunction> secondLevelClausules, string groupClass)
        {
            conjuncts = secondLevelClausules;
            this.groupClass = groupClass;
        } 

        public void printRule()
        {
            string ruleString = "IF ";
            for (int i = 0; i < conjuncts.Count; i++)
            {
                ruleString = string.Concat(ruleString, "(" + conjuncts[i].toString());
                if (i < conjuncts.Count - 1)
                    ruleString = string.Concat(ruleString, ") || ");
                else
                    ruleString = string.Concat(ruleString, ") ");
            }
            ruleString = string.Concat(ruleString, "THEN " + groupClass);

            ruleString = removeUselessBrackets(ruleString);

            Console.WriteLine(ruleString);
        }

        public class BracketPair
        {
            public int startingPosition { get; private set; }
            public int endingPosition { get; private set; }
            public bool isClosed { get; set; }

            public BracketPair(int startingPosition)
            {
                this.startingPosition = startingPosition;
                isClosed = false;
            }

            public void closeBracket(int endingPosition)
            {
                this.endingPosition = endingPosition;
                isClosed = true;
            }
        }

        private string removeUselessBrackets(string rule)
        {
            List<BracketPair> openedBrackets = new List<BracketPair>();
            List<BracketPair> closedBrackets = new List<BracketPair>();
            for (int i = 0; i < rule.Length; i++)
            {
                if (rule[i] == '(')
                    openedBrackets.Add(new BracketPair(i));
                if (rule[i] == ')')
                {
                    openedBrackets[openedBrackets.Count - 1].closeBracket(i);
                    closedBrackets.Add(openedBrackets[openedBrackets.Count - 1]);
                    openedBrackets.RemoveAt(openedBrackets.Count - 1);
                }
            }

            List<BracketPair> uselessBrackets = new List<BracketPair>();
            for (int i = 0; i < closedBrackets.Count; i++)
            {
                for (int j = i + 1; j < closedBrackets.Count; j++)
                {
                    int openDistance = Math.Abs(closedBrackets[i].startingPosition - closedBrackets[j].startingPosition);
                    int closeDistance = Math.Abs(closedBrackets[i].endingPosition - closedBrackets[j].endingPosition);

                    if (openDistance == 1 && closeDistance == 1)
                    {
                        if ((uselessBrackets.IndexOf(closedBrackets[i]) == -1) &&
                            (uselessBrackets.IndexOf(closedBrackets[j]) == -1))
                        {
                            uselessBrackets.Add(closedBrackets[i]);
                        }
                    }
                }
            }

            string newRule = "";
            for (int i = 0; i < rule.Length; i++)
            {
                bool useless = false;
                foreach (BracketPair pair in uselessBrackets)
                {
                    if (i == pair.startingPosition || i == pair.endingPosition)
                        useless = true;
                }
                if(useless == false)
                    newRule += rule[i];
            }

            return newRule;
        }
    }
}
