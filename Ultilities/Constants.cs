namespace TestAuthenAndTextMessage.Ultilities
{
    public class Constants
    {
        public const string GroupAvatarDirectory = "/Images/GroupAvatar";
        public const string UserAvatarDirectory = "/Images/UserAvatar";
        public const string AttachmentLink = "/Attachments";
        public const string UserRole = "User";
        public const string AdminRole = "Admin";

        public const string AESInitalVector = "OFRna73m*aze01xY";
		public const string SystemSecretKey = "System_Secretkey";

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

    public enum MessageType
    {
		File,
		Link,
		Media,
        Text,
	}
}
