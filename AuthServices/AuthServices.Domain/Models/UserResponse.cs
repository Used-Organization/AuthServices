using AuthServices.Domain.DTO;

namespace AuthServices.Domain.Models
{
    public class UserResponse :Response
    {
        public UserDTO? Result { get; set; }
    }
}
