using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQ11Console
{
    public class Inference
    {
        class MalfunctionDetectedException : Exception{ };

        private List<Example> examples;
        private string groupClass;

        public Inference(List<Example> examples, string groupClass)
        {
            this.examples = examples;
            this.groupClass = groupClass;
        }

        public List<LogicalArgument> ruleInference()
        {
            List<LogicalArgument> rule = new List<LogicalArgument>();

            List<Example> positiveExamples = examples.FindAll(example => example.groupClass == groupClass);
            List<Example> negativeExamples = examples.Except(positiveExamples).ToList();

            List<List<List<LogicalArgument>>> firstLevelClausules = new List<List<List<LogicalArgument>>>();
            List<List<LogicalArgument>> secondLevelClausules = new List<List<LogicalArgument>>();
            List<int> skipList = new List<int>();
            for (int i = 0; i < positiveExamples.Count; i++)
            {
                if (skipList.Exists(num => num == i))
                    continue;
                Example pos = positiveExamples[i];
                firstLevelClausules.Add(new List<List<LogicalArgument>>());
                for (int j = 0; j < negativeExamples.Count; j++)
                {
                    Example neg = negativeExamples[j];
                    List<LogicalArgument> ineqs = new List<LogicalArgument>();
                    for (int k = 0; k < pos.attributes.Count; k++)
                    {
                        if (pos.attributes[k].value != neg.attributes[k].value)
                        {
                            Variable var = new Variable(pos.attributes[k].name);
                            Constant cons = new Constant(neg.attributes[k].value);
                            Inequality ineq = new Inequality(var, cons);
                            ineqs.Add(ineq);
                        }
                    }
                    firstLevelClausules[firstLevelClausules.Count - 1].Add(ineqs);
                }
                secondLevelClausules.Add(applyAbsorbRule(firstLevelClausules[firstLevelClausules.Count - 1]));
                skipList.AddRange(skipCoveredExamples(secondLevelClausules[secondLevelClausules.Count - 1], positiveExamples, i));
            }

            rule = groupUpClausules(secondLevelClausules);

            return rule;
        }

        private List<LogicalArgument> applyAbsorbRule(List<List<LogicalArgument>> disjunctions)
        {
            for (int i = 0; i < disjunctions.Count; i++)
            {
                for (int j = 0; j < disjunctions.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (disjunctions[i].Count < disjunctions[j].Count)
                        clausuleInference(disjunctions[i], disjunctions[j]);
                }
            }
            List<LogicalArgument> result = groupUpClausules(disjunctions);
            result = equalitiesInference(result);
            return result;
        }

        private void clausuleInference(List<LogicalArgument> first, List<LogicalArgument> second)
        {
            bool fullMatch = true;
            for (int i = 0; i < first.Count; i++)
            {
                bool localMatch = false;
                for (int j = 0; j < second.Count; j++)
                {
                    if (first[i].isEqual(second[j]))
                        localMatch = true;   
                }
                if (localMatch == false)
                {
                    fullMatch = false;
                    break;
                }
            }

            List<LogicalArgument> deletees = new List<LogicalArgument>();
            if (fullMatch)
            {
                for (int i = 0; i < second.Count; i++)
                {
                    if (first.Find(arg => arg.isEqual(second[i])) == null)
                        deletees.Add(second[i]);
                }
                foreach(LogicalArgument deletee in deletees)
                    second.Remove(deletee);
            }
        }

        private List<LogicalArgument> groupUpClausules(List<List<LogicalArgument>> disjunctions)
        {
            List<LogicalArgument> result = new List<LogicalArgument>();
            result.AddRange(disjunctions[0]);
            foreach (List<LogicalArgument> disj in disjunctions)
            {
                foreach (LogicalArgument arg in disj)
                {
                    if (result.Exists(a => a.isEqual(arg)) == false)
                        result.Add(arg);
                }
            }

            return result;
        }

        private List<LogicalArgument> equalitiesInference(List<LogicalArgument> result)
        {
            AttributeValueMap[] maps = new AttributeValueMap[examples[0].attributes.Count];
            for (int i = 0; i < maps.Length; i++)
            {
                maps[i] = new AttributeValueMap(examples[0].attributes[i].name, examples[0].attributes[i].type);
            }
            foreach (Example ex in examples)
            {
                for(int i = 0; i < ex.attributes.Count; i++)
                {
                    Attribute atr = ex.attributes[i];
                    if (maps[i].values.Exists(val => val == atr.value) == false)
                        maps[i].values.Add(atr.value);
                }
            }

            List<AttributeValueMap> resultMap = new List<AttributeValueMap>();
            foreach (LogicalArgument arg in result)
            {
                if (arg.GetType().Name == "Inequality")
                {
                    Inequality ineq = (Inequality)arg;
                    if (ineq.firstArgument.GetType().Name == "Variable")
                    {
                        Variable var = (Variable)ineq.firstArgument;
                        Constant cons = (Constant)ineq.secondArgument;
                        bool mapFound = false;
                        foreach (AttributeValueMap map in resultMap)
                        {
                            if (map.name == var.name)
                                if (map.values.Exists(val => val == cons.value) == false)
                                {
                                    mapFound = true;
                                    map.values.Add(cons.value);
                                }
                        }
                        if (!(mapFound))
                        {
                            AttributeType type = maps.ToList().Find(m => m.name == var.name).type;
                            AttributeValueMap map = new AttributeValueMap(var.name, type);
                            map.values.Add(cons.value);
                            resultMap.Add(map);
                        }
                    }
                }
            }

            List<LogicalArgument> output = new List<LogicalArgument>();
            foreach (AttributeValueMap map in resultMap)
            {
                AttributeValueMap candidate = maps.ToList().Find(m => m.name == map.name);
                if (candidate != null)
                {
                    if (map.values.Count == candidate.values.Count - 1)
                    {
                        Variable var = new Variable(map.name);
                        string missingValue = "";
                        foreach (string value in candidate.values)
                        {
                            if (map.values.Exists(val => val == value) == false)
                            {
                                missingValue = value;
                                break;
                            }
                        }
                        Constant cons = new Constant(missingValue);
                        Equality eq = new Equality(var, cons);
                        output.Add(eq);
                    }
                    else
                    {
                        foreach (string value in map.values)
                        {
                            Variable var = new Variable(map.name);
                            Constant cons = new Constant(value);
                            Inequality ineq = new Inequality(var, cons);
                            output.Add(ineq);
                        }
                    }
                }
            }
            return output;
        }

        private List<int> skipCoveredExamples(List<LogicalArgument> clausules, List<Example> positiveExamples, int currentPosition)
        {
            List<Equality> eqs = new List<Equality>();
            List<Inequality> ineqs = new List<Inequality>();
            List<int> skipList = new List<int>();

            foreach (LogicalArgument clausule in clausules)
            {
                if (clausule.GetType().Name == "Inequality")
                    ineqs.Add((Inequality)clausule);
                else if (clausule.GetType().Name == "Equality")
                    eqs.Add((Equality)clausule);
                else
                    throw new MalfunctionDetectedException();
            }

            for (int i = currentPosition + 1; i < positiveExamples.Count; i++)
            {
                bool valueChanged = false;
                bool covered = true;
                foreach (Equality eq in eqs)
                {
                    Variable var = (Variable)eq.firstArgument;
                    Constant cons = (Constant)eq.secondArgument;
                    int count = positiveExamples[i].attributes.FindIndex(atr => atr.name == var.name);
                    if (count != -1)
                    {
                        if (positiveExamples[i].attributes[count].value == cons.value)
                        {
                            valueChanged = true;
                        }
                        else
                        {
                            covered = false;
                            break;
                        }
                    }
                }
                foreach (Inequality ineq in ineqs)
                {
                    Variable var = (Variable)ineq.firstArgument;
                    Constant cons = (Constant)ineq.secondArgument;
                    int count = positiveExamples[i].attributes.FindIndex(atr => atr.name == var.name);
                    if (count != -1)
                    {
                        if (positiveExamples[i].attributes[count].value != cons.value)
                        {
                            valueChanged = true;
                        }
                        else
                        {
                            covered = false;
                            break;
                        }
                    }
                }
                if (covered && valueChanged)
                    skipList.Add(i);
            }

            return skipList;
        }

        public void printRule(List<LogicalArgument> rule)
        {
            string ruleString = "IF ";
            foreach (LogicalArgument arg in rule)
            {
                ruleString = string.Concat(ruleString, arg.toString() + " & ");
            }
            ruleString = string.Concat(ruleString, "THEN " + groupClass);

            Console.WriteLine(ruleString);
        }
    }
}
