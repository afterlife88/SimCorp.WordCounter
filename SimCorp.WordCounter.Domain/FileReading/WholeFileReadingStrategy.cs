namespace SimCorp.WordCounter.Domain.FileReading;

public sealed class WholeFileReadingStrategy : IFileReadingStrategy
{
    public async Task<ResultOr<IEnumerable<string>>> Read(string filePath)
    {
        try
        {
            var text = await File.ReadAllTextAsync(filePath);
            var lines = text.Split(Environment.NewLine);
            return ResultOr<IEnumerable<string>>.Success(lines);
        }
        catch (Exception e)
        {
            return ResultOr<IEnumerable<string>>.Failure(e.Message);
        }
    }
}