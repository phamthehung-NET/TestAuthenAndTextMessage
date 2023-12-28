namespace TestAuthenAndTextMessage.Utilities
{
    public class Constants
    {
        public const string GroupAvatarDirectory = "/Images/GroupAvatar";
        public const string UserAvatarDirectory = "/Images/UserAvatar";
        public const string AttachmentLink = "/Attachments";
        public const string UserRole = "User";
        public const string AdminRole = "Admin";

        public const string AESInitialVector = "AES256Configuration:AESInitialVector";
		public const string SystemSecretKey = "AES256Configuration:SystemSecretKey";
        
        public const string AccessToken = "access_token";
    }

    public enum ErrorException
    {
        None,
        DuplicateUserName,
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
