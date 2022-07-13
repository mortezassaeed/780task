// See https://aka.ms/new-console-template for more information

Dictionary<char, int> words = new Dictionary<char, int>() { { 'a', 1 }, { 'b', 2 }, { 'c', 3 }, { 'd', 4 }, { 'e', 5 } };
List<string> opList = new List<string>() { "abcd", "bcde", "dede" };

string input = "abcd abcd aabbc ab a c ccd dede cccd cd";

List<int> results = new List<int>();
List<string> currentOprands = new List<string>();
foreach (string wordOrOp in (input.TrimEnd().TrimStart().Split(' ').Reverse()))
{
    if (opList.Contains(wordOrOp))
    {
        if (currentOprands.Count() != 0)
        {
            results.Add(calc(wordOrOp, currentOprands));
            currentOprands = new List<string>();
        }
        else if (results.Count() != 0)
        {
            var FINAL = CalculateOperation(wordOrOp, results);
            Console.Write(FINAL);
            results = new List<int>();
        }
        else
            throw new InvalidOperationException();
    }
    else
    {
        currentOprands.Add(wordOrOp);
    }
}

if (results.Count() == 1)
{
    var FINAL = results.Single();
    Console.Write(FINAL);
}

int calc(string wordOrOp, List<string> currentOprands)
{
    // loap over string oprands
    List<int> numOprands = new List<int>();
    foreach (var word in currentOprands)
    {
        var wordCharacters = RunLengthSplit(word);
        List<int> resultOfeachOperation = new List<int>();
        // each word converted to list of repeated charechrer exp : aaaabbbbdd => 'aaa', 'bbbb', 'dd'
        foreach (var item in wordCharacters)
        {
            var sumPartResult = item.Aggregate(0, (a, b) => a + words[b]);
            var mode5Result = sumPartResult % 5;
            var finalResult = (int)Math.Pow(mode5Result, 2);
            resultOfeachOperation.Add(finalResult);
        }
        numOprands.Add(resultOfeachOperation.Aggregate((a, b) => a + b));
    }

    return CalculateOperation(wordOrOp, numOprands);

}

int CalculateOperation(string @operator, List<int> oprands) =>
        @operator switch
        {
            "abcd" => oprands.Aggregate((a, b) => a + b),
            "bcde" => oprands.Aggregate((a, b) => a - b),
            "dede" => oprands.Aggregate((a, b) => a * b),
            _ => throw new InvalidOperationException() // never happen
        };

IEnumerable<string> RunLengthSplit(string source)
{
    using (var enumerator = source.GetEnumerator())
    {
        if (!enumerator.MoveNext()) yield break;
        char previous = enumerator.Current;
        int count = 1;
        while (enumerator.MoveNext())
        {
            if (previous == enumerator.Current)
            {
                count++;
            }
            else
            {
                yield return new string(Enumerable.Repeat(previous, count).ToArray());
                previous = enumerator.Current;
                count = 1;
            }
        }
        yield return new string(Enumerable.Repeat(previous, count).ToArray());
    }
}
