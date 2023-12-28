using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Repositories.Interfaces;
using TestAuthenAndTextMessage.Services.Interfaces;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Services.Implementation
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository repository;

        public AuthenticationService(IAuthenticationRepository _repository)
        {
            repository = _repository;
        }

        public async Task<ResponseModel> Login(LoginModel model)
        {
            var result = await repository.Login(model);
            if (result != null)
            {
                return result;
            }
            throw new Exception("UserName and Password is not correct");
        }

        public async Task Resgiter(RegisterModel model)
        {
            var result = await repository.Resgiter(model);
            if(result == ErrorException.DuplicateUserName)
            {
                throw new Exception("Username is doublicated");
            }
        }

        public async Task<ResponseModel> GetUserInfo()
        { 
            return await repository.GetUserInfo();
        }
    }
}
