﻿namespace Quotes.DTO.Requests
{
    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? DeviceType { get; set; }
        public string? DeviceToken { get; set; }

    }
}
