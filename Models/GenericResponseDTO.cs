namespace BACKEND_STORE.Models
{
    public class GenericResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = true;
        public object? Data { get; set; } = null;
        public int StatusCode { get; set; } = 200;
        public string? ErrorCode { get; set; } = null;
        public string? ErrorMessage { get; set; } = null;
        public string? StackTrace { get; set; } = null;
        public string? InnerException { get; set; } = null;
    }
}
