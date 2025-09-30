namespace Orion.API.UserProfileManagement.Services;

public interface IEmailService
{
    Task SendEmailConfirmationAsync(string userEmail, object name, string? confirmationLink);
}