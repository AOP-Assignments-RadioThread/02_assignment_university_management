using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using UniversityManager.Models;
using UniversityManager.ViewModels;
using Xunit;

namespace UniversityManager.Tests.ViewModels
{
    public class StudentViewModelTests
    {
        private readonly Mock<ISubjectRepository> _mockSubjectRepo;
        private readonly StudentViewModel _viewModel;
        private readonly List<Subject> _enrolledSubjects;
        private readonly List<Subject> _availableSubjects;
        
        public StudentViewModelTests()
        {
            _mockSubjectRepo = new Mock<ISubjectRepository>();
            _viewModel = new StudentViewModel(_mockSubjectRepo.Object);
            
            // Set up test data
            _enrolledSubjects = new List<Subject>
            {
                new Subject(1, "Enrolled Subject 1", "Description 1", 1, new List<int> { 1 }),
                new Subject(2, "Enrolled Subject 2", "Description 2", 1, new List<int> { 1 })
            };
            
            _availableSubjects = new List<Subject>
            {
                new Subject(3, "Available Subject 1", "Description 3", 1, new List<int>()),
                new Subject(4, "Available Subject 2", "Description 4", 1, new List<int>())
            };
            
            // Set up repository behavior
            _mockSubjectRepo.Setup(repo => repo.GetEnrolledSubjects(1))
                .Returns(_enrolledSubjects);
                
            _mockSubjectRepo.Setup(repo => repo.GetAvailableSubjects(1))
                .Returns(_availableSubjects);
        }
        
        [Fact]
        public void Initialize_ShouldSetNameAndId_AndRefreshSubjects()
        {
            // Act
            _viewModel.Initialize("Test Student", 1);
            
            // Assert
            Assert.Equal("Test Student", _viewModel.Name);
            Assert.Equal(1, _viewModel.StudentId);
            
            // Verify subjects were loaded
            _mockSubjectRepo.Verify(repo => repo.GetEnrolledSubjects(1), Times.Once);
            _mockSubjectRepo.Verify(repo => repo.GetAvailableSubjects(1), Times.Once);
            
            // Check the loaded subjects
            Assert.NotNull(_viewModel.EnrolledSubjects);
            Assert.Equal(2, _viewModel.EnrolledSubjects.Count);
            Assert.NotNull(_viewModel.AvailableSubjects);
            Assert.Equal(2, _viewModel.AvailableSubjects.Count);
            
            // Check first subject is auto-selected
            Assert.NotNull(_viewModel.EnrolledSelectedSubject);
            Assert.Equal("Enrolled Subject 1", _viewModel.EnrolledSelectedSubject.Name);
            Assert.NotNull(_viewModel.AvailableSelectedSubject);
            Assert.Equal("Available Subject 1", _viewModel.AvailableSelectedSubject.Name);
        }
        
        [Fact]
        public void EnrollInSelectedSubject_ShouldCallRepositoryAndRefreshSubjects()
        {
            // Arrange
            _viewModel.Initialize("Test Student", 1);
            var subject = _availableSubjects[0];
            _viewModel.AvailableSelectedSubject = subject;
            
            // Act
            _viewModel.EnrollInSelectedSubject();
            
            // Assert
            _mockSubjectRepo.Verify(repo => repo.EnrollStudent(1, subject.Id), Times.Once);
            _mockSubjectRepo.Verify(repo => repo.SaveSubjects(), Times.Once);
            _mockSubjectRepo.Verify(repo => repo.GetEnrolledSubjects(1), Times.Exactly(2)); // Initial + refresh
            _mockSubjectRepo.Verify(repo => repo.GetAvailableSubjects(1), Times.Exactly(2)); // Initial + refresh
            
            // Selected subject should be cleared
            Assert.Null(_viewModel.AvailableSelectedSubject);
        }
        
        [Fact]
        public void EnrollInSelectedSubject_WithNullSelectedSubject_ShouldDoNothing()
        {
            // Arrange
            _viewModel.Initialize("Test Student", 1);
            _viewModel.AvailableSelectedSubject = null;
            
            // Act
            _viewModel.EnrollInSelectedSubject();
            
            // Assert
            _mockSubjectRepo.Verify(repo => repo.EnrollStudent(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockSubjectRepo.Verify(repo => repo.SaveSubjects(), Times.Never);
        }
        
        [Fact]
        public void DropSelectedSubject_ShouldCallRepositoryAndRefreshSubjects()
        {
            // Arrange
            _viewModel.Initialize("Test Student", 1);
            var subject = _enrolledSubjects[0];
            _viewModel.EnrolledSelectedSubject = subject;
            
            // Act
            _viewModel.DropSelectedSubject();
            
            // Assert
            _mockSubjectRepo.Verify(repo => repo.DropSubject(1, subject.Id), Times.Once);
            _mockSubjectRepo.Verify(repo => repo.SaveSubjects(), Times.Once);
            _mockSubjectRepo.Verify(repo => repo.GetEnrolledSubjects(1), Times.Exactly(2)); // Initial + refresh
            _mockSubjectRepo.Verify(repo => repo.GetAvailableSubjects(1), Times.Exactly(2)); // Initial + refresh
            
            // Selected subject should be cleared
            Assert.Null(_viewModel.EnrolledSelectedSubject);
        }
        
        [Fact]
        public void DropSelectedSubject_WithNullSelectedSubject_ShouldDoNothing()
        {
            // Arrange
            _viewModel.Initialize("Test Student", 1);
            _viewModel.EnrolledSelectedSubject = null;
            
            // Act
            _viewModel.DropSelectedSubject();
            
            // Assert
            _mockSubjectRepo.Verify(repo => repo.DropSubject(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
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