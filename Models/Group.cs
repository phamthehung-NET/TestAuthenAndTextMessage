namespace TestAuthenAndTextMessage.Models
{
    public class Group : DefaultModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public string AdminId { get; set; }
    }
}
