namespace Map.Domain.Settings;

public class MailSettings
{
    public string Server { get; set; }
    public int Port { get; set; }
    public string NoReplyName { get; set; }
    public string NoReplyEmail { get; set; }
    public string SupportName { get; set; }
    public string SupportEmail { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}