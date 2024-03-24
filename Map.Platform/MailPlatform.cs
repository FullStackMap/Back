using Map.Domain.Models.EmailDto;
using Map.Domain.Settings;
using Map.Platform.Interfaces;
using Map.Provider.Interfaces;
using MimeKit;

namespace Map.Platform;
internal class MailPlatform : IMailPlatform
{
    #region Props

    private readonly IMailProvider _mailProvider;
    private readonly MailSettings _mailSettings;


    #endregion


    #region Ctor

    public MailPlatform(MailSettings mailSettings, IMailProvider mailProvider)
    {
        _mailSettings = mailSettings ?? throw new ArgumentNullException(nameof(mailSettings));
        _mailProvider = mailProvider ?? throw new ArgumentNullException(nameof(mailProvider));
    }

    #endregion


    public string GetTemplate(string templateName)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "MailTemplate", $"{templateName}");
        return File.Exists(filePath) ? File.ReadAllText(filePath) : throw new FileNotFoundException(filePath);
    }

    public async Task SendMailAsync(MailDto mailDto)
    {
        MimeMessage email = new();

        MailboxAddress mailFrom = new(mailDto.SenderName ?? _mailSettings.NoReplyName,
                                      mailDto.SenderMail ?? _mailSettings.NoReplyEmail);
        email.From.Add(mailFrom);

        MailboxAddress emailTo = new(mailDto.Name, mailDto.Email);
        if (!mailDto.IsMailToUser)
            emailTo = new(_mailSettings.SupportName, _mailSettings.SupportEmail);

        email.To.Add(emailTo);

        if (mailDto.EmailOnCopy)
            email.Cc.Add(new MailboxAddress(mailDto.Name, mailDto.Email));

        BodyBuilder bodyBuilder = new()
        {
            HtmlBody = mailDto.Body,
        };
        email.Body = bodyBuilder.ToMessageBody();

        email.Subject = mailDto.Subject;

        await _mailProvider.SendMailAsync(email);
    }
}
