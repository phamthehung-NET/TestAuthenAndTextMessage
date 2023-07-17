using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<object> Login(LoginModel model);

        Task Resgiter(RegisterModel model);

        Task<UserDTO> GetUserInfo();
    }
}
