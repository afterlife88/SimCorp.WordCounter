namespace SimCorp.WordCounter.Domain.Tests;

public sealed class WordFrequencyCounterTests
{
    [Fact]
    public void CountWords_ShouldReturnCorrectWordCount()
    {
        // Arrange
        var lines = new List<string> { "Go do that thing that you do so well" };
        var expected = new List<KeyValuePair<string, int>>
        {
            new("do", 2),
            new("that", 2),
            new("go", 1),
            new("so", 1),
            new("thing", 1),
            new("well", 1),
            new("you", 1)
        };

        // Act
        var result = WordFrequencyCounter.CountWords(lines);

        // Assert
        var resultList = result.Value.ToList();
        Assert.Equal(expected, resultList);
    }

    [Fact]
    public void CountWords_WithEmptyList_ShouldReturnEmptyCollection()
    {
        // Act
        var result = WordFrequencyCounter.CountWords(new List<string>());

        // Assert
        Assert.Empty(result.Value);
    }

    [Fact]
    public void CountWords_WithNullInput_ShouldReturnFailure()
    {
        // Act
        var result = WordFrequencyCounter.CountWords(null!);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void CountWords_WithDifferentCaseWords_ShouldTreatThemAsSame()
    {
        // Arrange
        var lines = new List<string> { "Hello hello HeLLo" };
        var expected = new List<KeyValuePair<string, int>> { new("hello", 3) };

        // Act
        var result = WordFrequencyCounter.CountWords(lines);

        // Assert
        var resultList = result.Value.ToList();
        Assert.Equal(expected, resultList);
    }

    [Fact]
    public void CountWords_WithSpecialCharacters_InWords_ShouldExcludeThem()
    {
        // Arrange
        var lines = new List<string> { "Hello, world! This is great." };
        var data = new List<KeyValuePair<string, int>>
        {
            new("great", 1),
            new("hello", 1),
            new("is", 1),
            new("this", 1),
            new("world", 1)
        };

        // Act
        var result = WordFrequencyCounter.CountWords(lines);

        // Assert
        var resultList = result.Value.ToList();
        Assert.Equal(data, resultList);
    }

    [Fact]
    public void CountWords_WithNumbers_ShouldCountThemAsWords()
    {
        // Arrange
        var lines = new List<string> { "123 456 123" };
        var expected = new List<KeyValuePair<string, int>> { new("123", 2), new("456", 1) };

        // Act
        var result = WordFrequencyCounter.CountWords(lines);

        // Assert
        var resultList = result.Value.ToList();
        Assert.Equal(expected, resultList);
    }
}