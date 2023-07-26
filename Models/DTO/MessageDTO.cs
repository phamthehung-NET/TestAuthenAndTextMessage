namespace TestAuthenAndTextMessage.Models.DTO
{
    public class MessageDTO : DefaultModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int ConversationId { get; set; }

        public bool BelongToGroup { get; set; }

        public CustomUser Author { get; set; }

        public bool IsSelfDelete { get; set; } = false;

        public bool IsDeleteForEveryOne { get; set; } = false;

        public int MessageType { get; set; }

        public string FileName { get; set; }
    }
}
