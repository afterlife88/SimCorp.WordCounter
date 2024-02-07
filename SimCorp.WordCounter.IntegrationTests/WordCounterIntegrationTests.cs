using System.Diagnostics;

namespace SimCorp.WordCounter.IntegrationTests;

public sealed class WordCounterIntegrationTests
{
    [Fact]
    public void ApplicationRunsSuccessfullyWithHappyPath()
    {
        // Arrange
        const string testFilePath = "test-files/happy_path.txt";
        var expectedResults = new Dictionary<string, int>
        {
            { "go", 1 },
            { "do", 2 },
            { "that", 2 },
            { "thing", 1 },
            { "you", 1 }
        };

        // Act
        var result = RunApplicationWithFile(testFilePath);

        // Assert
        AssertResultContainsExpectedWordCounts(result, expectedResults);
    }

    [Fact]
    public void ApplicationRunsSuccessfullyWithEmptyFile()
    {
        // Arrange
        const string testFilePath = "test-files/zero_bytes";
        var expectedResults = new Dictionary<string, int>();

        // Act
        var result = RunApplicationWithFile(testFilePath);

        // Assert
        AssertResultContainsExpectedWordCounts(result, expectedResults);
    }

    [Fact]
    public void ApplicationRunsSuccessfullyWithNonExistentFile()
    {
        // Arrange
        const string testFilePath = "test-files/non_existent_file.txt";

        // Act
        var result = RunApplicationWithFile(testFilePath);

        // Assert
        Assert.Contains("File does not exist", result);
    }

    [Fact]
    public void ApplicationRunsSuccessfullyWithMultipleLines()
    {
        // Arrange
        const string testFilePath = "test-files/happy_path_multiple_lines";
        var expectedResults = new Dictionary<string, int>
        {
            { "the", 10 },
            { "of", 6 },
            { "for", 5 },
            { "to", 4 },
            { "a", 4 },
            { "payments", 3 },
            { "transactions", 3 },
            { "nonreversible", 1 }
        };

        // Act
        var result = RunApplicationWithFile(testFilePath);

        // Assert
        AssertResultContainsExpectedWordCounts(result, expectedResults);
    }

    [Fact]
    public void ApplicationRunsSuccessfullyWithLargeFile()
    {
        // Arrange
        const string testFilePath = "test-files/large_file.txt";
        var expectedResults = new Dictionary<string, int>
        {
            { "consectetur", 287129 },
            { "elit", 286510 },
            { "sit", 286005 },
            { "lorem", 285802 },
            { "dolor", 285788 },
        };

        // Act
        var result = RunApplicationWithFile(testFilePath);

        // Assert
        AssertResultContainsExpectedWordCounts(result, expectedResults);
    }

    [Fact]
    public void ApplicationRunsSuccessfullyWithBookFormatFile()
    {
        // Arrange
        const string testFilePath = "test-files/kafka.txt";
        var expectedResults = new Dictionary<string, int>
        {
            { "the", 1348 },
            { "to", 835 },
            { "and", 710 },
            { "he", 593 },
            { "of", 557 },
            { "his", 550 },
            { "in", 411 }
        };

        // Act
        var result = RunApplicationWithFile(testFilePath);

        // Assert
        AssertResultContainsExpectedWordCounts(result, expectedResults);
    }

    private static string RunApplicationWithFile(string testFilePath)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments =
                    $"SimCorp.WordCounter.Presentation.dll {testFilePath}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        var result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return result;
    }

    private void AssertResultContainsExpectedWordCounts(string result, Dictionary<string, int> expectedResults)
    {
        Assert.NotEmpty(result);
        foreach (var expectedString in expectedResults.Select(pair => $"{pair.Key}: {pair.Value}"))
        {
            Assert.Contains(expectedString, result);
        }
    }
}