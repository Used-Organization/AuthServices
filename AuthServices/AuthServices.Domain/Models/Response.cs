﻿namespace AuthServices.Domain.Models
{
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public virtual object? Result { get; set; }
    }
}
