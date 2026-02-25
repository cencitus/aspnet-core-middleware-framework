namespace frameworks_pr1.Models;

public class ApiErrorResponse
{
    public string? RequestId { get; set; }
    public ApiError Error { get; set; } = new();
}

public class ApiError
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
}