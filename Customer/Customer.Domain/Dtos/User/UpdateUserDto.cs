﻿namespace Customer.Domain.Dtos.User;

public class UpdateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }
    public string PhoneNumber { get; set; }
}
