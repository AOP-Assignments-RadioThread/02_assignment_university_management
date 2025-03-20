using UniversityManager.Models;
using UniversityManager.ViewModels;
using Xunit;

namespace UniversityManager.Tests.ViewModels
{
    public class LogInViewModelValidationTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("username", "")]
        [InlineData("", "password")]
        [InlineData("   ", "password")]
        [InlineData("username", "   ")]
        public void Login_WithEmptyOrWhitespaceCredentials_ShouldSetLoginFailed(string username, string password)
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