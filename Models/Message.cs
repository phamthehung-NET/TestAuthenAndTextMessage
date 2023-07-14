namespace TestAuthenAndTextMessage.Models
{
    public class Message : DefaultModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int ConversationId { get; set; }

        public bool BelongToGroup { get; set; }

        public string AuthorId { get; set; }

        public bool IsSelfDelete { get; set; } = false;

        public bool IsDeleteForEveryOne { get; set; } = false;

        public int MessageType { get; set; }
    }
}
