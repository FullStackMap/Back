using Map.Domain.Models.EmailDto;

namespace Map.Platform.Interfaces;
public interface IMailPlatform
{
    public Task SendMailAsync(MailDto mailDto);

    public string GetTemplate(string fileName);
}