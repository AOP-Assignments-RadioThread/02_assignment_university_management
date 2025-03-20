namespace UniversityManager.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class UserRepo
{
    private static readonly Lazy<UserRepo> _instance = new Lazy<UserRepo>(() => new UserRepo());
    private List<User> users;
    public static string filePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Assets", "Users.json");

    // Private constructor
    private UserRepo()
    {
        LoadUsers();
    }

    // Public accessor for Singleton instance
    public static UserRepo Instance => _instance.Value;

    // Load from Json
    private void LoadUsers()
    {
        string directory = Path.GetDirectoryName(filePath);
        
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
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
        File.WriteAllText(filePath, json);
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
        if (GetUserByUsername(newUser.Username) == null)
        {
            users.Add(newUser);
            SaveUsers();
        }
    }
}