using SimCorp.WordCounter.Application;

if (args.Length == 0)
{
    Console.WriteLine("Please provide a file path.");
    return;
}

var filePath = args[0];

var (result, success, error) = await WordCountService.Execute(filePath);

if (success)
{
    foreach (var (word, count) in result)
    {
        Console.WriteLine($"{word}: {count}");
    }
}
else
{
    Console.WriteLine($"Error: {error}");
}