namespace TestAuthenAndTextMessage.Models.DTO
{
    public class GroupDTO : DefaultModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public string AdminId { get; set; }

        public string AvatarFileName { get; set; }

        public MessageDTO LastestMessage { get; set; }

        public bool IsGroupChat { get; } = true;

        public string Message { get; set; }
    }
}
