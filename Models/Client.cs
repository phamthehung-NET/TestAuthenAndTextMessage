namespace TestAuthenAndTextMessage.Models
{
    public class Client : DefaultModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string SignalRClientId { get; set; }
    }
}
