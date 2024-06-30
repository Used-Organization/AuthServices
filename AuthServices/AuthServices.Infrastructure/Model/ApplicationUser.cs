using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
namespace AuthServices.Infrastructure.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

    }
}

