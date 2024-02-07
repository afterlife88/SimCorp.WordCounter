namespace SimCorp.WordCounter.Domain.FileReading;

public interface IFileReadingStrategy
{
    Task<ResultOr<IEnumerable<string>>> Read(string filePath);
}
