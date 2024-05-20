namespace Customer.Domain.Dtos.User;

public class UserResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }    
    public string UserName { get; set; }    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool Active { get; set; }    
    public string PhotoUrl { get; set; }    
    public string Role { get; set; }    
}
