using SimCorp.WordCounter.Domain;
using SimCorp.WordCounter.Domain.FileReading;

namespace SimCorp.WordCounter.Application;

public static class WordCountService
{
    // 10 MB file size threshold to switch to line-by-line
    private const long
        LargeFileSizeThreshold =
            10 * 1024 * 1024;

    public static Task<ResultOr<IReadOnlyCollection<KeyValuePair<string, int>>>> Execute(string filePath)
        => ValidateFile(filePath)
            .BindAsync(ProcessFile);

    private static async Task<ResultOr<IReadOnlyCollection<KeyValuePair<string, int>>>> ProcessFile(FileInfo fileInfo)
        => await ChooseReadingStrategy(fileInfo)
            .BindAsync(async strategy => await strategy.Read(fileInfo.FullName))
            .Then(WordFrequencyCounter.CountWords);

    private static ResultOr<FileInfo> ValidateFile(string filePath)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return ResultOr<FileInfo>.Failure("File does not exist");
            }

            return fileInfo.Length switch
            {
                > int.MaxValue => ResultOr<FileInfo>.Failure("File is too large"),
                0 => ResultOr<FileInfo>.Failure("File is empty"),
                _ => ResultOr<FileInfo>.Success(fileInfo)
            };
        }
        catch (Exception ex)
        {
            return ResultOr<FileInfo>.Failure(ex.Message);
        }
    }

    private static ResultOr<IFileReadingStrategy> ChooseReadingStrategy(FileInfo fileInfo)
    {
        return fileInfo.Length < LargeFileSizeThreshold
            ? ResultOr<IFileReadingStrategy>.Success(new WholeFileReadingStrategy())
            : ResultOr<IFileReadingStrategy>.Success(new LineByLineFileReadingStrategy());
    }
}