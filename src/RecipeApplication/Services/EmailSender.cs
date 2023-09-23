using Microsoft.AspNetCore.Identity.UI.Services;
using FluentEmail.Core;

namespace RecipeApplication.Services;
public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;
    private readonly IFluentEmail _fluentEmail;
    public EmailSender(ILogger<EmailSender> logger, IFluentEmail fluentEmail)
    {
        _logger = logger;
        _fluentEmail = fluentEmail;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var response = await _fluentEmail
        .To(toEmail)
        .Subject(subject)
        .Body(message, true)
        .SendAsync();
        if (response.Successful)
            _logger.LogInformation($"Email to {toEmail} queued successfully!");

        else
            foreach (var error in response.ErrorMessages)
                _logger.LogError(error);
    }
}