using Map.Domain.Models.EmailDto;
using Map.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Platform.Interfaces;
public interface IMailPlatform
{
    public Task SendMailAsync(MailDto mailDto);

    public string GetTemplate(string fileName);
}