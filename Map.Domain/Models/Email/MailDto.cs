namespace Map.Domain.Models.EmailDto;
public class MailDto
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Email of the sender
    /// </summary>
    /// <remarks>
    /// If empty, the default email will be used
    /// </remarks>
    public string? SenderMail { get; set; }
    /// <summary>
    /// Name of the sender
    /// </summary>
    /// <remarks>If empty, the defailt name will be used</remarks>
    public string? SenderName { get; set; }


    /// <summary>
    /// Email of the copy
    /// </summary>
    /// <remarks>If true, a copy of mail wil be send to the props Email <see cref="MailDto.Email"/></remarks>
    public bool EmailOnCopy { get; set; }
    public bool IsMailToUser { get; set; } = true;
}
