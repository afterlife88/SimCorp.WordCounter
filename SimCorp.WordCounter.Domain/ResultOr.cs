namespace SimCorp.WordCounter.Domain;

public sealed record ResultOr<T>
{
    public T Value { get; }

    public bool IsSuccess { get; }

    public string Error { get; }

    private ResultOr(T value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static ResultOr<T> Success(T value) => new(value, true, null!);

    public static ResultOr<T> Failure(string error) => new(default!, false, error);

    public void Deconstruct(out T value, out bool isSuccess, out string? error) =>
        (value, isSuccess, error) = (Value, IsSuccess, Error);

    public async Task<ResultOr<TU>> BindAsync<TU>(Func<T, Task<ResultOr<TU>>> func)
    {
        if (!IsSuccess)
        {
            return ResultOr<TU>.Failure(Error);
        }

        try
        {
            return await func(Value);
        }
        catch (Exception ex)
        {
            return ResultOr<TU>.Failure(ex.Message);
        }
    }
}

public static class ResultOrExtensions
{
    public static async Task<ResultOr<TU>> Then<T, TU>(this Task<ResultOr<T>> task, Func<T, ResultOr<TU>> nextFunc)
    {
        var result = await task;
        return !result.IsSuccess ? ResultOr<TU>.Failure(result.Error) : nextFunc(result.Value);
    }
}