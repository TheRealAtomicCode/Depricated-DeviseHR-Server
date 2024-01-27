namespace DeviseHR_Server.DTOs
{
    public class ServiceResponse<T>
    {

        public ServiceResponse(T data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }

        public ServiceResponse(T data, bool success, string message, string jwt)
        {
            Data = data;
            Success = success;
            Message = message;
            Jwt = jwt;
        }

        public ServiceResponse(T data, bool success, string message, string jwt, string refreshToken)
        {
            Data = data;
            Success = success;
            Message = message;
            Jwt = jwt;
            RefreshToken = refreshToken;
        }

        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public string? Jwt { get; set; }
        public string? RefreshToken { get; set; }
    }
}
