namespace TestAuthenAndTextMessage.Models
{
    public class Conversation : DefaultModel
    {
        public int Id { get; set; }

        public string User1Id { get; set; }

        public string User2Id { get; set; }

        public bool? IsUser1Deleted { get; set; }

        public bool? IsUser2Deleted { get; set; }
    }
}
