using System.ComponentModel.DataAnnotations;

namespace AuthServices.Domain.DTO
{
    public record LoginDTO([EmailAddress] string Email,string Password);
}
