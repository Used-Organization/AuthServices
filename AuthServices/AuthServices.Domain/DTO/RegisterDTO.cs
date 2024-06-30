using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServices.Domain.DTO
{
    public record RegisterDTO(string FirstName, string LastName, [EmailAddress] string Email, string Password);
}
