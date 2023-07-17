using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Models.DTO;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<object> Login(LoginModel model);

        Task<ErrorException> Resgiter(RegisterModel model);

        Task<UserDTO> GetUserInfo();
    }
}
