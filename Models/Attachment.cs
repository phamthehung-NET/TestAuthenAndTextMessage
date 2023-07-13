namespace TestAuthenAndTextMessage.Models
{
    public class Attachment : DefaultModel
    {
        public int ID { get; set; }

        public string AttachmentLink { get; set; }

        public string AuthorId { get; set; }

        public int ConversationId { get; set; }

        public bool BelongToGroup { get; set; }

        public int AttachmentType { get; set; }
    }
}
