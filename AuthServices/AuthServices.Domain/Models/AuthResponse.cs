using AuthServices.Domain.DTO;

namespace AuthServices.Domain.Models
{
    public class AuthResponse:Response
    {
        public AuthDTO? Result { get; set; }
    }
}
