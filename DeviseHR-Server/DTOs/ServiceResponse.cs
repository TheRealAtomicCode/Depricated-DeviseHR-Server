namespace DeviseHR_Server.DTOs
{
    public class ServiceResponse<T>
    {
        private object value;
        private bool v;


        public ServiceResponse(object value, bool v, string message)
        {
            this.value = value;
            this.v = v;
            Message = message;
        }

        public ServiceResponse(T data, bool success, string message, string jwt)
        {
            Data = data;
            Success = success;
            Message = message;
            Jwt = jwt;
        }
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public string? Jwt { get; set; }
    }
}
