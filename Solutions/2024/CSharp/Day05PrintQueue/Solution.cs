namespace Solutions.Y2024.CSharp.Day05PrintQueue
{
    public class Solution : ISolution
    {
        private static (Dictionary<int, List<int>>, IEnumerable<Dictionary<int, int>>) ParseRulesAndUpdate(IEnumerable<string> lines)
        {
            var rules = new Dictionary<int, List<int>>();

            var ruleLines = lines
                .TakeWhile(line => line != "")
                .Select(line => line.Split('|').Select(int.Parse).ToArray())
                .Select(line => new { index = line[0], value = line[1] });

            foreach (var rule in ruleLines)
            {
                if (!rules.TryAdd(rule.index, [rule.value]))
                {
                    rules[rule.index].Add(rule.value);
                }
            }

            var updates = lines
                    .SkipWhile(line => line != "").Skip(1)
                    .Select(line => line
                    .Split(',')
                    .Select((c, i) => new { pageNumber = int.Parse(c), index = i })
                    .ToDictionary(update => update.pageNumber, update => update.index));

            return (rules, updates);
        }

        public string SolveFirst(IEnumerable<string> lines)
        {
            var sum = 0;
            var (rules, updates) = ParseRulesAndUpdate(lines);

            foreach (var update in updates)
            {
                if (update.All(page =>
                 {
                     var pageNumber = page.Key;
                     var index = page.Value;

                     if (!rules.TryGetValue(pageNumber, out var rule)) return true;

                     return rule.All(r =>
                     {
                         if (update.TryGetValue(r, out var i))
                         {
                             return i > index;
                         }
                         return true;
                     });

                 })) sum += update.Keys.ElementAt((update.Count / 2));
            }

            return sum.ToString();
        }

        public string SolveSecond(IEnumerable<string> lines)
        {
            var (rules, updates) = ParseRulesAndUpdate(lines);
            var fixedUpdates = new List<Dictionary<int, int>>();
            var sum = 0;
            foreach (var update in updates)
            {
                var orderFixed = false;
                foreach (var page in update)
                {
                    var pageNumber = page.Key;
                    var index = page.Value;

                    checkRulesAndFixOrder(pageNumber);
                }

                if (orderFixed)
                {
                    fixedUpdates.Add(update);
                    sum += update.First(pair => pair.Value == update.Count/2).Key;
                }

                void checkRulesAndFixOrder(int pageNumber)
                {
                    if (!rules.TryGetValue(pageNumber, out var rule)) return;

                    foreach (var rulePage in rule)
                    {
                        if (!update.TryGetValue(rulePage, out var indexOfRulePage)) continue;
                        var index = update[pageNumber];
                        if (indexOfRulePage > index) continue;

                        update[pageNumber] = indexOfRulePage;
                        update[rulePage] = index;
                        orderFixed = true;
                        checkRulesAndFixOrder(rulePage);
                    }
                }
            }
            return sum.ToString();
        }
    }
}
