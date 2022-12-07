namespace Application.DTOs;

public class IdentitySignInResponseDTO
{
    public bool success { get; set; }
    public bool isLockedOut { get; set; }
}