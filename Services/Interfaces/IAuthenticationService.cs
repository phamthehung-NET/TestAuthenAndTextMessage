using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<object> Login(LoginModel model);

        Task Resgiter(RegisterModel model);

        Task<CustomUser> GetUserInfo();
    }
}
