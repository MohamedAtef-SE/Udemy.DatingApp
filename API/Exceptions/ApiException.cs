﻿namespace API.Exceptions
{
    public class ApiException(int statuscode, string message, string? details = null)
    {
        public int StatusCode { get; set; } = statuscode;
        public string Message { get; set; } = message;
        public string? Details { get; set; } = details;

    }
}
