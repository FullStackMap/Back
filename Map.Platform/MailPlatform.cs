using Map.Domain.Models.EmailDto;
using Map.Domain.Settings;
using Map.Platform.Interfaces;
using Map.Provider.Interfaces;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Platform;
public class MailPlatform : IMailPlatform
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
        MimeMessage email =  new();
        
        MailboxAddress mailFrom = new(_mailSettings.SenderName, _mailSettings.SenderMail);
        email.From.Add(mailFrom);

        MailboxAddress emailTo = new(mailDto.Name, mailDto.Email);
        email.To.Add(emailTo);

        BodyBuilder bodyBuilder = new()
        {
            HtmlBody = mailDto.Body,
        };
        email.Body = bodyBuilder.ToMessageBody();

        await _mailProvider.SendMailAsync(email);
    }
}
