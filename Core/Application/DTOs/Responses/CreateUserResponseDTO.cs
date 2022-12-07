namespace Application.DTOs;

public class CreateUserResponseDTO : IdentityResponseDTO
{
    public string verification_token { get; set; }
}