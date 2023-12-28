using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Utilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ResponseModel> Login(LoginModel model);

        Task Resgiter(RegisterModel model);

        Task<ResponseModel> GetUserInfo();
    }
}
