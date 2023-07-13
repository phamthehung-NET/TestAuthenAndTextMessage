namespace TestAuthenAndTextMessage.Models.DTO
{
    public class MessageDTO : DefaultModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int ConversationId { get; set; }

        public bool BelongToGroup { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}
