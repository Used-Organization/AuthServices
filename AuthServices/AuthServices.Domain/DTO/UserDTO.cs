namespace AuthServices.Domain.DTO
{
    public record UserDTO
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
