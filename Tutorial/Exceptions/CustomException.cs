namespace Tutorial.Exceptions
{
    public class CustomException: Exception
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public CustomException(int statusCode, string message):base(message) 
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }
    }
}
