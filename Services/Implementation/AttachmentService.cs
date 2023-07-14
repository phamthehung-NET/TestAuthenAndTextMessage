
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Services.Implementation
{
	public class AttachmentService : IAttachmentService
	{
		private readonly IAttachmentRepository repository;

        public AttachmentService(IAttachmentRepository _repository)
        {
			repository = _repository;
        }

        public void AddAttachment(Attachment attachment)
		{
			var result = repository.AddAttachment(attachment);
			if(result == ErrorException.DatabaseError)
			{
				throw new Exception("Failed to connect to the database");
			}
		}

		public Pagination<Attachment> GetAllAttachment(int conversationId, bool belongToGroup, int pageIndex, int pageSize)
		{
			return repository.GetAllAttachment(conversationId, belongToGroup, pageIndex, pageSize);
		}

		public void RemoveAttachment(string attachmentLink, int attachmentType)
		{
			var result = repository.RemoveAttachment(attachmentLink, attachmentType);
			if(result == ErrorException.NotPermitted)
			{
				throw new Exception("You don't have permission to remove this attachment");
			}
			if(result == ErrorException.NotExist)
			{
				throw new Exception("This attachment is not exist");
			}
		}
	}
}
