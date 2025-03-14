using System.Collections.Generic;

namespace UniversityManager.Models;

public class Student
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public Student(int id, string name, string username, string password, List<Subject> subjects)
    {
        ID = id;
        Name = name;
        Username = username;
        Password = password;
    }
    
}