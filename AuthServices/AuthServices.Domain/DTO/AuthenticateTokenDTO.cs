namespace AuthServices.Domain.DTO
{
    public record AuthenticateTokenDTO
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsInvalidRefreshToken { get; set; }
        public bool IsInvaliedAccessToken { get; set; }
    }
}
