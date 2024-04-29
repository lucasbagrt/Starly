namespace Customer.Domain.Dtos.User;

public class UserResponseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool Active { get; set; }    
}
