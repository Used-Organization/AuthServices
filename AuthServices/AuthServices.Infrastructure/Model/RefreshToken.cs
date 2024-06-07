using System;

namespace AuthServices.Infrastructure.Model
{
    public class RefreshToken
    {
        public int Id { get; set; } // Primary key
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime? Revoked { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByIp { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= Expires;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; } // Navigation property
    }
}
