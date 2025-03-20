using System;
using Moq;
using UniversityManager.Models;
using UniversityManager.ViewModels;
using Xunit;

namespace UniversityManager.Tests.ViewModels
{
    public class LogInViewModelTests
    {
        [Fact]
        public void Login_WithValidCredentials_ShouldInvokeLoginSuccessful()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var validUser = new User(1, "testuser", "password", UserRole.Student);
            
            mockUserRepo.Setup(repo => repo.AuthenticateUser("testuser", "password"))
                .Returns(validUser);
            
            var viewModel = new LogInViewModel(mockUserRepo.Object)
            {
                UserName = "testuser",
                Password = "password"
            };
            
            bool loginSuccessfulCalled = false;
            User? passedUser = null;
            
            viewModel.LoginSuccessful += (user) => {
                loginSuccessfulCalled = true;
                passedUser = user;
            };
            
            // Act
            viewModel.Login();
            
            // Assert
            Assert.True(loginSuccessfulCalled);
            Assert.Equal(validUser, passedUser);
            Assert.False(viewModel.IsLoginFailed);
            Assert.Empty(viewModel.UserName);
            Assert.Empty(viewModel.Password);
        }
        
        [Fact]
        public void Login_WithInvalidCredentials_ShouldSetLoginFailed()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            
            mockUserRepo.Setup(repo => repo.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((User)null!);
            
            var viewModel = new LogInViewModel(mockUserRepo.Object)
            {
                UserName = "testuser",
                Password = "wrongpassword"
            };
            
            bool loginSuccessfulCalled = false;
            viewModel.LoginSuccessful += (user) => loginSuccessfulCalled = true;
            
            // Act
            viewModel.Login();
            
            // Assert
            Assert.False(loginSuccessfulCalled);
            Assert.True(viewModel.IsLoginFailed);
            Assert.Equal("Invalid username or password", viewModel.ErrorMessage);
        }
        
        [Theory]
        [InlineData("", "")]
        [InlineData("username", "")]
        [InlineData("", "password")]
        public void Login_WithEmptyCredentials_ShouldSetLoginFailed(string username, string password)
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var viewModel = new LogInViewModel(mockUserRepo.Object)
            {
                UserName = username,
                Password = password
            };
            
            // Act
            viewModel.Login();
            
            // Assert
            Assert.True(viewModel.IsLoginFailed);
            Assert.Equal("Username and password are required", viewModel.ErrorMessage);
            mockUserRepo.Verify(repo => repo.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
} 