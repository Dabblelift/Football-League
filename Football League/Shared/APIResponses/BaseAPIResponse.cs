namespace Football_League.Shared.APIResponses
{
    public class BaseAPIResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public BaseAPIResponse(bool success, string message)
        {
            Message = message;
            Success = success;
        }
    }
    public class BaseAPIResponse<TEntity> : BaseAPIResponse where TEntity : class
    {
        public TEntity? Data { get; set; }

        public BaseAPIResponse(bool success, string message, TEntity? data = null)
            : base(success, message) 
        {
            Data = data;
        }
    }
    
}
