namespace TestAuthenAndTextMessage.Models.DTO
{
    public class ConversationDTO : DefaultModel
    {
        public int Id { get; set; }

        public string User1Id { get; set; }

        public string User1FirstName { get; set; }

        public string User1LastName { get; set; }

        public string User1Avatar { get; set; }

        public string User2Id { get; set; }

        public string User2FirstName { get; set; }

        public string User2LastName { get; set; }
        
        public string User2Avatar { get; set; }

        public bool? IsUser1Deleted { get; set; }

        public bool? IsUser2Deleted { get; set; }

        public MessageDTO LastestMessage { get; set; }

        public bool IsGroupChat { get; } = false;
    }
}
