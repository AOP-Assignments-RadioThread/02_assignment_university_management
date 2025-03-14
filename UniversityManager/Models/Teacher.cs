using System.Collections.Generic;

namespace UniversityManager.Models;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username{get; set;}
    public string Password { get; set; }
    
    public Teacher (int id, string name, string username, string password)
    {
        Id = id;
        Name = name;
        Username = username;
        Password = password;
    }
}