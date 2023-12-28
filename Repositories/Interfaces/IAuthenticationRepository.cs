using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<ResponseModel> Login(LoginModel model);

        Task<ErrorException> Resgiter(RegisterModel model);

        Task<ResponseModel> GetUserInfo();
    }
}
