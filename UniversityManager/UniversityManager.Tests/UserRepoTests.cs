using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UniversityManager.Models;
using Xunit;

namespace UniversityManager.Tests
{
    public class UserRepoTests : IDisposable
    {
        private readonly string _testFilePath;
        
        public UserRepoTests()
        {
            // Create a temporary file for testing
            _testFilePath = Path.Combine(Path.GetTempPath(), $"test_users_{Guid.NewGuid()}.json");
            
            // Initialize test data
            var users = new List<User>
            {
                new User(1, "testStudent", "password", UserRole.Student),
                new User(2, "testTeacher", "password", UserRole.Teacher)
            };
            
            // Save test data to file
            File.WriteAllText(_testFilePath, JsonConvert.SerializeObject(users));
        }
        
        [Fact]
        public void GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var userRepo = new UserRepo(_testFilePath);
            
            // Act
            var users = userRepo.GetAllUsers();
            
            // Assert
            Assert.Equal(2, users.Count);
            Assert.Contains(users, u => u.Username == "testStudent");
            Assert.Contains(users, u => u.Username == "testTeacher");
        }
        
        [Fact]
        public void GetUserByUsername_ShouldReturnCorrectUser()
        {
            // Arrange
            var userRepo = new UserRepo(_testFilePath);
            
            // Act
            var user = userRepo.GetUserByUsername("testStudent");
            
            // Assert
            Assert.NotNull(user);
            Assert.Equal("testStudent", user.Username);
            Assert.Equal(UserRole.Student, user.Role);
        }
        
        [Fact]
        public void GetUserByUsername_WithNonexistentUsername_ShouldReturnNull()
        {
            // Arrange
            var userRepo = new UserRepo(_testFilePath);
            
            // Act
            var user = userRepo.GetUserByUsername("nonexistent");
            
            // Assert
            Assert.Null(user);
        }
        
        [Fact]
        public void AuthenticateUser_WithValidCredentials_ShouldReturnUser()
        {
            // Arrange
            var userRepo = new UserRepo(_testFilePath);
            
            // Act
            var user = userRepo.AuthenticateUser("testStudent", "password");
            
            // Assert
            Assert.NotNull(user);
            Assert.Equal("testStudent", user.Username);
        }
        
        [Fact]
        public void AuthenticateUser_WithInvalidPassword_ShouldReturnNull()
        {
            // Arrange
            var userRepo = new UserRepo(_testFilePath);
            
            // Act
            var user = userRepo.AuthenticateUser("testStudent", "wrongpassword");
            
            // Assert
            Assert.Null(user);
        }
        
        [Fact]
        public void AddUser_ShouldAddUserToRepository()
        {
            // Arrange
            var userRepo = new UserRepo(_testFilePath);
            var newUser = new User(3, "newUser", "password", UserRole.Student);
            
            // Act
            userRepo.AddUser(newUser);
            var users = userRepo.GetAllUsers();
            
            // Assert
            Assert.Equal(3, users.Count);
            Assert.Contains(users, u => u.Username == "newUser");
        }
        
        [Fact]
        public void SaveUsers_ShouldPersistChangesToFile()
        {
            // Arrange
            var userRepo = new UserRepo(_testFilePath);
            var newUser = new User(3, "saveTest", "password", UserRole.Student);
            
            // Act
            userRepo.AddUser(newUser);
            userRepo.SaveUsers();
            
            // Create a new repo instance to load from file
            var newUserRepo = new UserRepo(_testFilePath);
            var users = newUserRepo.GetAllUsers();
            
            // Assert
            Assert.Equal(3, users.Count);
            Assert.Contains(users, u => u.Username == "saveTest");
        }
        
        public void Dispose()
        {
            // Clean up test file
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }
} 