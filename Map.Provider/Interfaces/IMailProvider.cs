using Map.Domain.Models.EmailDto;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Provider.Interfaces;
public interface IMailProvider
{
    public Task SendMailAsync(MimeMessage email);

}
