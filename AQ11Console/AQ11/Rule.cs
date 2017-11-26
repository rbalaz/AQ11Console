using System;
using System.Collections.Generic;

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

        public string printRule()
        {
            // Builds the final E1/E2 wrapper from conjuncts loaded in the constructor
            // Returns the wrapper in a single rule form
            string ruleString = "IF ";
            for (int i = 0; i < conjuncts.Count; i++)
            {
                ruleString = string.Concat(ruleString, "(" + conjuncts[i].toString());
                if (i < conjuncts.Count - 1)
                    ruleString = string.Concat(ruleString, ") OR ");
                else
                    ruleString = string.Concat(ruleString, ") ");
            }
            ruleString = string.Concat(ruleString, "THEN " + groupClass);

            ruleString = removeUselessBrackets(ruleString);

            return ruleString;
        }

        public string[] printRules()
        {
            // Builds the final E1/E2 wrapper from conjuncts loaded in the constructor
            // Returns each rule disjunct separately
            string[] ruleStrings = new string[conjuncts.Count];
            for (int i = 0; i < conjuncts.Count; i++)
            {
                ruleStrings[i] = "IF ";
                ruleStrings[i] = string.Concat(ruleStrings[i], "(" + conjuncts[i].toString() + ") ");
                ruleStrings[i] = string.Concat(ruleStrings[i], "THEN " + groupClass);
                ruleStrings[i] = removeUselessBrackets(ruleStrings[i]);
            }

            return ruleStrings;
        }

        public bool classifyNewExample(Example example)
        {
            // Tests, if new example matches the rule definition
            bool matchValidated = true;
            foreach (Conjunction conj in conjuncts)
            {
                List<Disjunction> disjuncts = new List<Disjunction>();
                List<LogicalArgument> eqs = new List<LogicalArgument>();
                foreach (LogicalArgument arg in conj.arguments)
                {
                    if (arg.GetType().Name == "Disjunction")
                        disjuncts.Add((Disjunction)arg);
                    else
                        eqs.Add(arg);
                }
                foreach (Disjunction disj in disjuncts)
                {
                    eqs.AddRange(disj.arguments);
                }
                foreach (LogicalArgument eq in eqs)
                {
                    // All values in the equality must be matched 
                    // Always a full match is required, otherwise the test fails
                    if (eq.GetType().Name == "Equality")
                    {
                        Equality equality = (Equality)eq;
                        Variable var = (Variable)equality.firstArgument;
                        Constant cons = (Constant)equality.secondArgument;
                        Attribute attribute = example.attributes.Find(atr => atr.name == var.name);
                        if (cons.valueSet.Contains(attribute.value) == false)
                            matchValidated = false;                           
                    }
                }
                // Returns the result of the test
                if (matchValidated)
                    return matchValidated;
            }

            return matchValidated;
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
            // Checks the rule string for abundant brackets by inferencing bracket pairs positions
            // Abundant brackets are removed from the rule string before it is displayed in the GUI
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
