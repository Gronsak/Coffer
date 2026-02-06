using System.Collections.Immutable;
using System.Security.Cryptography.Pkcs;
using System.Text.Json;
using Coffer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;

namespace Coffer.Services;

public class JsonMailer(string rootPath) : IEmailSender<AppUser>
{
    private string RootPath { get; } = rootPath;
    public async Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
    {
        var dict = new Dictionary<string, string>
        {
            { "EmailAdress", email },
            { "Subject", "Account Confirmation" },
            { "Body", confirmationLink }
        };
        var json = JsonSerializer.Serialize(dict);
        var path = Path.Combine(RootPath,"temp/mail.json");
        await File.AppendAllTextAsync(path, json + Environment.NewLine);
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var dict = new Dictionary<string, string>
        {
            { "EmailAdress", email },
            { "Subject", subject },
            { "Body", htmlMessage }
        };
        var json = JsonSerializer.Serialize(dict);
        var path = Path.Combine(RootPath,"temp/mail.json");
        await File.AppendAllTextAsync(path, json + Environment.NewLine);
    }

    public async Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
    {
        var dict = new Dictionary<string, string>
        {
            { "EmailAdress", email },
            { "Subject", "Password Reset Code" },
            { "Body", resetCode }
        };
        var json = JsonSerializer.Serialize(dict);
        var path = Path.Combine(RootPath,"temp/mail.json");
        await File.AppendAllTextAsync(path, json + Environment.NewLine);
    }

    public async Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
    {
        var dict = new Dictionary<string, string>
        {
            { "EmailAdress", email },
            { "Subject", "Password Reset" },
            { "Body", resetLink }
        };
        var json = JsonSerializer.Serialize(dict);
        var path = Path.Combine(RootPath,"temp/mail.json");
        await File.AppendAllTextAsync(path, json + Environment.NewLine);
    }
}