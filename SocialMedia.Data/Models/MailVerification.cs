namespace SocialMedia.Data.Models;

public class MailVerification
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Expires { get; set; }
    public string Code {get; set;}
}