namespace UniversityManager.Models;


public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }  
    public UserRole Role { get; set; } 
    
    public User(int id, string username, string passwordHash, UserRole role)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
    }
}

public enum UserRole
{
    Student,
    Teacher,
}