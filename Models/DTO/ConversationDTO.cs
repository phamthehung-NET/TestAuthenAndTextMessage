namespace TestAuthenAndTextMessage.Models.DTO
{
    public class ConversationDTO : DefaultModel
    {
        public int Id { get; set; }

        public CustomUser User1 { get; set; }

        public CustomUser User2 { get; set; }

        public bool? IsUser1Deleted { get; set; }

        public bool? IsUser2Deleted { get; set; }

        public MessageDTO LastestMessage { get; set; }

        public bool IsGroupChat { get; } = false;
    }
}
