namespace N5.WebApi.dto
{
    public class ResponseMessageDto <T>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        
    }
}
