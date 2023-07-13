namespace TestAuthenAndTextMessage.Ultilities
{
    public class Constants
    {
        public const string GroupAvatarDirectory = "/Images/GroupAvatar";
        public const string UserAvatarDirectory = "/Images/UserAvatar";
        public const string AttachmentLink = "/Attachments";
    }

    public enum ErrorException
    {
        None,
        DoublicateUserName,
        NotExist,
        NotPermitted,
        DatabaseError,
    }

    public enum AttachmentType
    {
        File,
        Link,
        Media,
    }
}
