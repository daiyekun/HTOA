namespace CWHT.OA.Application.DTOs;

public class ApiResponse<T>
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public bool Success => Code == 200;

    public static ApiResponse<T> SuccessResult(T data, string message = "操作成功")
    {
        return new ApiResponse<T>
        {
            Code = 200,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> FailResult(string message = "操作失败", int code = 500)
    {
        return new ApiResponse<T>
        {
            Code = code,
            Message = message,
            Data = default
        };
    }
}

public class ApiResponse : ApiResponse<object>
{
    public new static ApiResponse Success(string message = "操作成功")
    {
        return new ApiResponse
        {
            Code = 200,
            Message = message
        };
    }

    public new static ApiResponse Fail(string message = "操作失败", int code = 500)
    {
        return new ApiResponse
        {
            Code = code,
            Message = message
        };
    }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public long Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class PagedInput
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Keyword { get; set; }
}
