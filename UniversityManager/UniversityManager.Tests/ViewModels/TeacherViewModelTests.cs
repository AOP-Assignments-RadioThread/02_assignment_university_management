using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using UniversityManager.Models;
using UniversityManager.ViewModels;
using Xunit;

namespace UniversityManager.Tests.ViewModels
{
    public class TeacherViewModelTests
    {
        private readonly Mock<ISubjectRepository> _mockSubjectRepo;
        private readonly TeacherViewModel _viewModel;
        private readonly List<Subject> _teacherSubjects;
        
        public TeacherViewModelTests()
        {
            _mockSubjectRepo = new Mock<ISubjectRepository>();
            _viewModel = new TeacherViewModel(_mockSubjectRepo.Object);
            
            // Set up test data
            _teacherSubjects = new List<Subject>
            {
                new Subject(1, "Subject 1", "Description 1", 1, new List<int>()),
                new Subject(2, "Subject 2", "Description 2", 1, new List<int>())
            };
            
            // Set up repository behavior
            _mockSubjectRepo.Setup(repo => repo.GetSubjectsByTeacher(1))
                .Returns(_teacherSubjects);
                
            _mockSubjectRepo.Setup(repo => repo.GetAllSubjects())
                .Returns(_teacherSubjects);
        }
        
        [Fact]
        public void Initialize_ShouldSetNameAndId_AndLoadSubjects()
        {
            // Act
            _viewModel.Initialize("Test Teacher", 1);
            
            // Assert
            Assert.Equal("Test Teacher", _viewModel.Name);
            Assert.Equal(1, _viewModel.TeacherId);
            
            // Verify subjects were loaded
            _mockSubjectRepo.Verify(repo => repo.GetSubjectsByTeacher(1), Times.Once);
            
            // Check the loaded subjects
            Assert.NotNull(_viewModel.MySubjects);
            Assert.Equal(2, _viewModel.MySubjects.Count);
            
            // Check first subject is auto-selected
            Assert.NotNull(_viewModel.SelectedSubject);
            Assert.Equal("Subject 1", _viewModel.SelectedSubject.Name);
        }
        
        [Fact]
        public void CreateSubject_WithValidData_ShouldAddSubjectAndRefresh()
        {
            // Arrange
            _viewModel.Initialize("Test Teacher", 1);
            _viewModel.NewSubjectName = "New Subject";
            _viewModel.NewSubjectDescription = "New Description";
            
            // Act
            _viewModel.CreateSubject();
            
            // Assert
            _mockSubjectRepo.Verify(repo => repo.AddSubject(It.Is<Subject>(s => 
                s.Name == "New Subject" && 
                s.Description == "New Description" && 
                s.TeacherId == 1)), 
                Times.Once);
                
            _mockSubjectRepo.Verify(repo => repo.SaveSubjects(), Times.Once);
            _mockSubjectRepo.Verify(repo => repo.GetSubjectsByTeacher(1), Times.Exactly(2)); // Initial + refresh
            
            // Fields should be cleared
            Assert.Empty(_viewModel.NewSubjectName);
            Assert.Empty(_viewModel.NewSubjectDescription);
        }
        
        [Theory]
        [InlineData("", "Description")]
        [InlineData("Name", "")]
        [InlineData("", "")]
        public void CreateSubject_WithInvalidData_ShouldNotAddSubject(string name, string description)
        {
            // Arrange
            _viewModel.Initialize("Test Teacher", 1);
            _viewModel.NewSubjectName = name;
            _viewModel.NewSubjectDescription = description;
            
            // Act
            _viewModel.CreateSubject();
            
            // Assert
            _mockSubjectRepo.Verify(repo => repo.AddSubject(It.IsAny<Subject>()), Times.Never);
            _mockSubjectRepo.Verify(repo => repo.SaveSubjects(), Times.Never);
        }
        
        [Fact]
        public void DeleteSubject_WithSelectedSubject_ShouldRemoveSubjectAndRefresh()
        {
            // Arrange
            _viewModel.Initialize("Test Teacher", 1);
            var subject = _teacherSubjects[0];
            _viewModel.SelectedSubject = subject;
            
            // Act
            _viewModel.DeleteSubject();
            
            // Assert
            _mockSubjectRepo.Verify(repo => repo.RemoveSubject(subject.Id), Times.Once);
            _mockSubjectRepo.Verify(repo => repo.SaveSubjects(), Times.Once);
            _mockSubjectRepo.Verify(repo => repo.GetSubjectsByTeacher(1), Times.Exactly(2)); // Initial + refresh
            
            // Selected subject should be cleared
            Assert.Null(_viewModel.SelectedSubject);
        }
        
        [Fact]
        public void DeleteSubject_WithNullSelectedSubject_ShouldDoNothing()
        {
            // Arrange
            _viewModel.Initialize("Test Teacher", 1);
            
            // Fix: Set SelectedSubject to null explicitly
            _viewModel.SelectedSubject = default!;
            
            // Act
            _viewModel.DeleteSubject();
            
            // Assert
            _mockSubjectRepo.Verify(repo => repo.RemoveSubject(It.IsAny<int>()), Times.Never);
            _mockSubjectRepo.Verify(repo => repo.SaveSubjects(), Times.Never);
        }
        
        [Fact]
        public void Logout_ShouldRaiseLogoutRequestedEvent()
        {
            // Arrange
            bool logoutRequested = false;
            _viewModel.LogoutRequested += () => logoutRequested = true;
            
            // Act
            _viewModel.Logout();
            
            // Assert
            Assert.True(logoutRequested);
        }
    }
} 