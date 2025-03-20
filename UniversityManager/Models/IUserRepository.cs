using System.Collections.Generic;
namespace UniversityManager.Models;

public interface IUserRepository
{
    List<User> GetAllUsers();
    User GetUserByUsername(string username);
    User AuthenticateUser(string username, string password);
    void AddUser(User newUser);
    void SaveUsers();
}