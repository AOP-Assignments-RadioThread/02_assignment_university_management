namespace UniversityManager.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class UserRepo : IUserRepository
{
    private List<User> users;
    private readonly string _filePath;

    public UserRepo(string filePath)
    {
        _filePath = filePath;
        LoadUsers();
    }

    // Production constructor
    public UserRepo() : this(Path.Combine(
        Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, 
        "Assets", 
        "Users.json"))
    {
    }

    private void LoadUsers()
    {
        string directory = Path.GetDirectoryName(_filePath);
        
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            users = JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }
        else
        {
            users = new List<User>
            {
                new User(1, "student", "password", UserRole.Student),
                new User(2, "teacher", "password", UserRole.Teacher)
            };
            SaveUsers();
        }
    }

    public void SaveUsers()
    {
        var json = JsonConvert.SerializeObject(users);
        File.WriteAllText(_filePath, json);
    }
    
    public List<User> GetAllUsers()
    {
        return users;
    }

    public User GetUserByUsername(string username)
    {
        return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public User AuthenticateUser(string username, string password)
    {
        return users.FirstOrDefault(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
            u.Password == password);
    }

    public void AddUser(User newUser)
    {
        users.Add(newUser);
    }
}