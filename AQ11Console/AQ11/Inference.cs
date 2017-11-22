using System;
using System.Collections.Generic;
using System.Linq;

namespace AQ11Console
{
    public class Inference
    {
        class MalfunctionDetectedException : Exception{ };

        // Training set
        private List<Example> examples;
        // Rules will be generated for this class
        private string groupClass;

        public Inference(List<Example> examples, string groupClass)
        {
            this.examples = examples;
            this.groupClass = groupClass;
        }

        // Main function which does all the steps AQ11 takes to generate rules
        public Rule ruleInference()
        {
            // AQ11 inputs
            // Positive examples E1
            List<Example> positiveExamples = examples.FindAll(example => example.groupClass == groupClass);
            // Negative examples E2
            List<Example> negativeExamples = examples.Except(positiveExamples).ToList();

            // Data member for the most specific wrappers ei/ej(positive vs negative example)
            List<List<Disjunction>> firstLevelClausules = new List<List<Disjunction>>();
            // Data member for the more general wrappers ei/E2(positive example vs negative class)
            List<Conjunction> secondLevelClausules = new List<Conjunction>();
            // Data member indicting which examples were already covered by generated wrappers and thus 
            // no longer need to be analyzed
            List<int> skipList = new List<int>();
            // AQ11 runs through all positive examples
            for (int i = 0; i < positiveExamples.Count; i++)
            {
                // Replaces "remove all positive examples covered by any ei/E2 wrapper" to avoid 
                // problems with modifying the list that is currently being iterated
                if (skipList.Exists(num => num == i))
                    continue;
                Example pos = positiveExamples[i];
                firstLevelClausules.Add(new List<Disjunction>());
                // Generate ei/ej wrapper for every negative example
                for (int j = 0; j < negativeExamples.Count; j++)
                {
                    Example neg = negativeExamples[j];
                    // Each wrapper consists of a disjunction of inequalities
                    List<LogicalArgument> ineqs = new List<LogicalArgument>();
                    for (int k = 0; k < pos.attributes.Count; k++)
                    {
                        // Inequality for each separate attribute is generated if the positive and negative
                        // examples do not share the same value in it
                        if (pos.attributes[k].value != neg.attributes[k].value)
                        {
                            Variable var = new Variable(pos.attributes[k].name);
                            Constant cons = new Constant(neg.attributes[k].value);
                            Inequality ineq = new Inequality(var, cons);
                            ineqs.Add(ineq);
                        }
                    }
                    // Disjunction is created for all generated inequalities and added to correct data member
                    Disjunction disj = new Disjunction(ineqs);
                    firstLevelClausules[firstLevelClausules.Count - 1].Add(disj);
                }
                // Generates ei/E2 wrapper for the current positive example as conjunction of all 
                // ei/ej wrappers and by applying the absorb rule
                secondLevelClausules.Add(applyAbsorbRule(firstLevelClausules[firstLevelClausules.Count - 1]));
                // Using the newly generated ei/E2 wrapper, skip list is updated to include positive examples
                // covered by it
                skipList.AddRange(skipCoveredExamples(secondLevelClausules[secondLevelClausules.Count - 1], positiveExamples, i));
            }

            // All ei/E2 wrappers are merged using disjunctions in a final rule wrapper E1/E2
            Rule rule = new Rule(secondLevelClausules, groupClass);

            return rule;
        }

        private Conjunction applyAbsorbRule(List<Disjunction> disjunctions)
        {
            // Absorb rule baselines 
            // a AND (a OR b) <=> a
            // a OR (a AND b) <=> a
            // Symbols a,b represent disjuncts = inequalities
            // Each ei/ej wrapper is constructed as disjunction of inequalities
            // ei/ej wrappers are merged together into ei/E2 wrapper using conjunction

            for (int i = 0; i < disjunctions.Count; i++)
            {
                for (int j = 0; j < disjunctions.Count; j++)
                {
                    if (i == j)
                        continue;
                    // Absorb rule can only be applied to 2 different disjunctions, that meet the requirement
                    // of one having less separate disjuncts than the other
                    if (disjunctions[i].arguments.Count < disjunctions[j].arguments.Count)
                        clausuleInference(disjunctions[i].arguments, disjunctions[j].arguments);
                }
            }
            // Removes redundant clausules using the law: a AND a <=> a
            List<LogicalArgument> result = groupUpClausules(disjunctions).arguments;
            // Replaces inequalities with equalities if applicable
            result = equalitiesInference(result);
            // Creates the final conjunction
            Conjunction conj = new Conjunction(result);
            return conj;
        }

        private void clausuleInference(List<LogicalArgument> first, List<LogicalArgument> second)
        {
            // Tests if arguments of 2 disjuncts match the patterns required for absorb rule
            // a AND (a OR b) <=> a
            // a OR (a AND b) <=> a
            // Disjunction first will always have fewer disjuncts than disjunction second
            bool fullMatch = true;
            for (int i = 0; i < first.Count; i++)
            {
                bool localMatch = false;
                for (int j = 0; j < second.Count; j++)
                {
                    // Every disjunct present in the first disjunction must be located in the second disjunction
                    if (first[i].isEqual(second[j]))
                        localMatch = true;   
                }
                // if its not, absorb rule is not applicable
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
                    // Every disjunct in the second(longer) disjunction will be absorbed(deleted)
                    // if it has no mathching counterpart in the first disjunction
                    if (first.Find(arg => arg.isEqual(second[i])) == null)
                        deletees.Add(second[i]);
                }
                foreach(LogicalArgument deletee in deletees)
                    second.Remove(deletee);
            }
        }

        private Disjunction groupUpClausules(List<Disjunction> disjunctions)
        {
            List<LogicalArgument> result = new List<LogicalArgument>();
            result.AddRange(disjunctions[0].arguments);
            // Groups up remaining unabsorbed disjuncts into a one line disjunction
            // Removes redundant disjuncts
            foreach (Disjunction disj in disjunctions)
            {
                foreach (LogicalArgument arg in disj.arguments)
                {
                    if (result.Exists(a => a.isEqual(arg)) == false)
                        result.Add(arg);
                }
            }

            Disjunction disjResult = new Disjunction(result);

            return disjResult;
        }

        private List<LogicalArgument> equalitiesInference(List<LogicalArgument> result)
        {
            // Builds an attribute map containing:
            // 1.) names of all attributes
            // 2.) all possible values for each attribute present in the training set
            AttributeValueMap[] maps = new AttributeValueMap[examples[0].attributes.Count];
            for (int i = 0; i < maps.Length; i++)
            {
                // Uses the first example as a map prototype for the attribute names
                maps[i] = new AttributeValueMap(examples[0].attributes[i].name, examples[0].attributes[i].type);
            }
            foreach (Example ex in examples)
            {
                // Runs through all examples and saves their unique attribute values in appropriate maps
                for (int i = 0; i < ex.attributes.Count; i++)
                {
                    Attribute atr = ex.attributes[i];
                    if (maps[i].values.Exists(val => val == atr.value) == false)
                        maps[i].values.Add(atr.value);
                }
            }

            // Builds an attribute map containing:
            // 1.) names of all attributes present in the grouped up unabsorbed disjuncts
            // 2.) values of the attributes present in the unabsorbed inequalities
            List<AttributeValueMap> resultMap = new List<AttributeValueMap>();
            foreach (LogicalArgument arg in result)
            {
                if (arg.GetType().Name == "Inequality")
                {
                    Inequality ineq = (Inequality)arg;
                    if (ineq.firstArgument.GetType().Name == "Variable")
                    {
                        // Variable name represents the attribute name
                        Variable var = (Variable)ineq.firstArgument;
                        // Constant name represents the attribute value
                        Constant cons = (Constant)ineq.secondArgument;
                        bool mapFound = false;
                        foreach (AttributeValueMap map in resultMap)
                        {
                            // If attribute map is found for the given name
                            // it adds its value if its not yet listed in the map
                            if (map.name == var.name)
                                if (map.values.Exists(val => val == cons.value) == false)
                                {
                                    mapFound = true;
                                    map.values.Add(cons.value);
                                }
                        }
                        // If no map is found for attribute of the given name
                        if (!(mapFound))
                        {
                            // New map is created using data from the full attribute map generated from training set
                            AttributeType type = maps.ToList().Find(m => m.name == var.name).type;
                            AttributeValueMap map = new AttributeValueMap(var.name, type);
                            map.values.Add(cons.value);
                            resultMap.Add(map);
                        }
                    }
                }
            }

            // Inequality can be changed to equality if the inequality includes all but one attribute value
            List<LogicalArgument> output = new List<LogicalArgument>();
            foreach (AttributeValueMap map in resultMap)
            {
                AttributeValueMap candidate = maps.ToList().Find(m => m.name == map.name);
                if (candidate != null)
                {
                    // If the result map has one less candidate than full map
                    if (map.values.Count == candidate.values.Count - 1)
                    {
                        // Inequality is replaced with equality 
                        // Equality is built using the missing value in result map, which is retrieved from the full map
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
                    // If not, inequality is passed to the final result
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

        private List<int> skipCoveredExamples(Conjunction clausules, List<Example> positiveExamples, int currentPosition)
        {
            List<Equality> eqs = new List<Equality>();
            List<Inequality> ineqs = new List<Inequality>();
            List<int> skipList = new List<int>();

            foreach (LogicalArgument clausule in clausules.arguments)
            {
                if (clausule.GetType().Name == "Inequality")
                    ineqs.Add((Inequality)clausule);
                else if (clausule.GetType().Name == "Equality")
                    eqs.Add((Equality)clausule);
                else
                    throw new MalfunctionDetectedException();
            }

            // Only examples which are located behind the currently used example can be affected by the skip list
            for (int i = currentPosition + 1; i < positiveExamples.Count; i++)
            {
                bool valueChanged = false;
                bool covered = true;
                foreach (Equality eq in eqs)
                {
                    Variable var = (Variable)eq.firstArgument;
                    Constant cons = (Constant)eq.secondArgument;
                    int count = positiveExamples[i].attributes.FindIndex(atr => atr.name == var.name);
                    // Tests, if every value inside the equalities of the ei/E2 wrapper is matched
                    // by attribute values of the following positive examples
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
                    // Tests, if every value inside the inequalities of the ei/E2 wrapper is not matched
                    // by attribute values of the following positive examples
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
                // If a full match is reached, position of the example is added to the skip list
                if (covered && valueChanged)
                    skipList.Add(i);
            }

            return skipList;
        }
    }
}
