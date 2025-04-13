namespace Namatara.API.Models
{
    public class _ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; } = string.Empty;
        public T Data { get; set; } = default!;

        public _ApiResponse(int statusCode = StatusCodes.Status200OK, string? message = "", T? data = default)
        {
            StatusCode = statusCode;
            // Message = statusCode == 200 || statusCode == 201 ? "Success." : message;
            Message = !string.IsNullOrEmpty(message)
                ? message
                : (statusCode == 200 || statusCode == 201 ? "Success." : message);
            Data = data;
        }
    }
}