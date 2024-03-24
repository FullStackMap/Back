using MailKit.Net.Smtp;
using Map.Domain.Settings;
using Map.Provider.Interfaces;
using MimeKit;

namespace Map.Provider;
internal class MailProvider : IMailProvider
{
    #region Props

    private readonly MailSettings _mailSettings;

    #endregion

    #region Ctor

    public MailProvider(MailSettings mailSettings)
    {
        _mailSettings = mailSettings ?? throw new ArgumentNullException(nameof(mailSettings));
    }

    #endregion

    public async Task SendMailAsync(MimeMessage email)
    {
        using SmtpClient smtpClient = new();
        await smtpClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtpClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
        await smtpClient.SendAsync(email);
        await smtpClient.DisconnectAsync(true);
        smtpClient.Dispose();
    }
}
