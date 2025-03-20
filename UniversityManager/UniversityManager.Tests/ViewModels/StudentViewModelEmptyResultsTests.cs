using System.Collections.Generic;
using System.Linq;
using UniversityManager.Models;
using UniversityManager.ViewModels;
using Xunit;

namespace UniversityManager.Tests.ViewModels
{
    public class StudentViewModelEmptyResultsTests
    {
        [Fact]
        public void Initialize_WithNoEnrolledSubjects_ShouldHaveNullSelection()
        {
            // Arrange
            var mockSubjectRepo = new Mock<ISubjectRepository>();
            
            // Set up to return empty lists
            mockSubjectRepo.Setup(repo => repo.GetEnrolledSubjects(It.IsAny<int>()))
                .Returns(new List<Subject>());
                
            mockSubjectRepo.Setup(repo => repo.GetAvailableSubjects(It.IsAny<int>()))
                .Returns(new List<Subject> 
                {
                    new Subject(1, "Available Subject", "Description", 1, new List<int>())
                });
            
            var viewModel = new StudentViewModel(mockSubjectRepo.Object);
            
            // Act
            viewModel.Initialize("Test Student", 1);
            
            // Assert
            Assert.NotNull(viewModel.EnrolledSubjects);
            Assert.Empty(viewModel.EnrolledSubjects);
            Assert.Null(viewModel.EnrolledSelectedSubject);
            
            Assert.NotNull(viewModel.AvailableSubjects);
            Assert.Single(viewModel.AvailableSubjects);
            Assert.NotNull(viewModel.AvailableSelectedSubject);
        }
        
        [Fact]
        public void Initialize_WithNoAvailableSubjects_ShouldHaveNullSelection()
        {
            // Arrange
            var mockSubjectRepo = new Mock<ISubjectRepository>();
            
            // Set up to return empty lists
            mockSubjectRepo.Setup(repo => repo.GetEnrolledSubjects(It.IsAny<int>()))
                .Returns(new List<Subject> 
                {
                    new Subject(1, "Enrolled Subject", "Description", 1, new List<int>{1})
                });
                
            mockSubjectRepo.Setup(repo => repo.GetAvailableSubjects(It.IsAny<int>()))
                .Returns(new List<Subject>());
            
            var viewModel = new StudentViewModel(mockSubjectRepo.Object);
            
            // Act
            viewModel.Initialize("Test Student", 1);
            
            // Assert
            Assert.NotNull(viewModel.EnrolledSubjects);
            Assert.Single(viewModel.EnrolledSubjects);
            Assert.NotNull(viewModel.EnrolledSelectedSubject);
            
            Assert.NotNull(viewModel.AvailableSubjects);
            Assert.Empty(viewModel.AvailableSubjects);
            Assert.Null(viewModel.AvailableSelectedSubject);
        }
    }
} 