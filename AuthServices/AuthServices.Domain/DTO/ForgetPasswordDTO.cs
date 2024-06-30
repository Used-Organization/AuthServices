

using System.ComponentModel.DataAnnotations;

namespace AuthServices.Domain.DTO
{
    public record ForgetPasswordDTO([EmailAddress] string Email, string Password, string VerificationCode);
}
