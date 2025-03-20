namespace UniversityManager.Models;


public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }  
    public UserRole Role { get; set; } 
    
    public User(int id, string username, string password, UserRole role)
    {
        Id = id;
        Username = username;
        Password = password;
        Role = role;
    }
}

public enum UserRole
{
    Student,
    Teacher,
}