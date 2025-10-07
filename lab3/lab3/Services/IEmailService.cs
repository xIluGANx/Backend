﻿public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
    Task<bool> ValidateEmailAsync(string email);
}