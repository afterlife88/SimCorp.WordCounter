namespace SimCorp.WordCounter.Domain.FileReading;

public sealed class LineByLineFileReadingStrategy : IFileReadingStrategy
{
    public async Task<ResultOr<IEnumerable<string>>> Read(string filePath)
    {
        try
        {
            var lines = new List<string>();
            await using var fileStream =
                new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            using var streamReader = new StreamReader(fileStream);
            while (await streamReader.ReadLineAsync() is { } line)
            {
                lines.Add(line);
            }

            return ResultOr<IEnumerable<string>>.Success(lines);
        }
        catch (Exception e)
        {
            return ResultOr<IEnumerable<string>>.Failure(e.Message);
        }
    }
}