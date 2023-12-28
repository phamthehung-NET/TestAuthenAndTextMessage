namespace TestAuthenAndTextMessage.Models.DTO
{
    public class ResponseModel
    {
        public bool Error { get; set; } = false;

        public string Message { get; set; }

        public string Data { get; set; }
    }
}
