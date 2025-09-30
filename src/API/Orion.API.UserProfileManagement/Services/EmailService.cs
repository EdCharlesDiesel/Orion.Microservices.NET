namespace Orion.API.UserProfileManagement.Services;

public class EmailService: IEmailService
{
    public async Task SendEmailConfirmationAsync(string userEmail, object name, string? confirmationLink)
    {
        throw new NotImplementedException();
    }
}