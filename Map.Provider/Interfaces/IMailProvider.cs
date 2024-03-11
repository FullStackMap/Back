using MimeKit;

namespace Map.Provider.Interfaces;
public interface IMailProvider
{
    public Task SendMailAsync(MimeMessage email);

}
