namespace Map.Domain.Models.User;
public class SupportContactMailDto
{
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Name { get; set; }

    public bool EmailOnCopy { get; set; }
}
