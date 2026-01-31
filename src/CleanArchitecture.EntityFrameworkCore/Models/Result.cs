namespace CleanArchitecture.EntityFrameworkCore.Models;

/// <summary>
/// Result pattern để handle success/failure
/// </summary>
public class Result<T>
{
    public int Code { get; }
    public string Message { get; }
    public T? Data { get; }
    public List<string> Errors { get; }

    protected Result(int code, string message, T? data, List<string>? errors = null)
    {
        Code = code;
        Message = message;
        Data = data;
        Errors = errors ?? new List<string>();
    }

    public static Result<T> Success(T data, string message = "Success", int code = 200) => new(code, message, data);

    public static Result<T> Failure(string message, int code = 400) => new(code, message, default);

    public static Result<T> Failure(string message, List<string> errors, int code = 400) =>
        new(code, message, default, errors);
}

public class Result
{
    public int Code { get; }
    public string Message { get; }
    public List<string> Errors { get; }

    protected Result(int code, string message, List<string>? errors = null)
    {
        Code = code;
        Message = message;
        Errors = errors ?? new List<string>();
    }

    public static Result Success(string message = "Success") => new(200, message);

    public static Result Failure(string message, int code = 400) => new(code, message);

    public static Result Failure(string message, List<string> errors, int code = 400) =>
        new(code, message, errors);
}
