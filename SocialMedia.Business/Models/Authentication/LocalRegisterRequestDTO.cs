namespace SocialMedia.Business.Models.Authentication;

public class LocalRegisterRequestDTO
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}