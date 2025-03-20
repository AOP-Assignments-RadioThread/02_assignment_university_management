using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UniversityManager.Models;
using Xunit;

namespace UniversityManager.Tests
{
    public class SubjectRepoTests : IDisposable
    {
        private readonly string _testFilePath;
        
        public SubjectRepoTests()
        {
            // Create a temporary file for testing
            _testFilePath = Path.Combine(Path.GetTempPath(), $"test_subjects_{Guid.NewGuid()}.json");
            
            // Initialize test data
            var subjects = new List<Subject>
            {
                new Subject(1, "Math", "Mathematics course", 1, new List<int> { 2 }),
                new Subject(2, "Physics", "Physics course", 1, new List<int> { 3 })
            };
            
            // Save test data to file
            File.WriteAllText(_testFilePath, JsonConvert.SerializeObject(subjects));
        }
        
        [Fact]
        public void GetAllSubjects_ShouldReturnAllSubjects()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            var subjects = subjectRepo.GetAllSubjects();
            
            // Assert
            Assert.Equal(2, subjects.Count);
            Assert.Contains(subjects, s => s.Name == "Math");
            Assert.Contains(subjects, s => s.Name == "Physics");
        }
        
        [Fact]
        public void GetSubjectsByTeacher_ShouldReturnTeacherSubjects()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            var subjects = subjectRepo.GetSubjectsByTeacher(1);
            
            // Assert
            Assert.Equal(2, subjects.Count);
        }
        
        [Fact]
        public void GetSubjectsByTeacher_WithNonexistentTeacher_ShouldReturnEmptyList()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            var subjects = subjectRepo.GetSubjectsByTeacher(999);
            
            // Assert
            Assert.Empty(subjects);
        }
        
        [Fact]
        public void GetAvailableSubjects_ShouldReturnNonEnrolledSubjects()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            var availableForStudent2 = subjectRepo.GetAvailableSubjects(2);
            var availableForStudent3 = subjectRepo.GetAvailableSubjects(3);
            var availableForStudent4 = subjectRepo.GetAvailableSubjects(4);
            
            // Assert
            Assert.Single(availableForStudent2);
            Assert.Equal("Physics", availableForStudent2[0].Name);
            
            Assert.Single(availableForStudent3);
            Assert.Equal("Math", availableForStudent3[0].Name);
            
            Assert.Equal(2, availableForStudent4.Count);
        }
        
        [Fact]
        public void GetSubjectById_ShouldReturnCorrectSubject()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            var subject = subjectRepo.GetSubjectById(1);
            
            // Assert
            Assert.NotNull(subject);
            Assert.Equal("Math", subject.Name);
        }
        
        [Fact]
        public void GetSubjectById_WithNonexistentId_ShouldReturnNull()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            var subject = subjectRepo.GetSubjectById(999);
            
            // Assert
            Assert.Null(subject);
        }
        
        [Fact]
        public void EnrollStudent_ShouldAddStudentToSubject()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            subjectRepo.EnrollStudent(4, 1); // Enroll student 4 in Math
            
            // Assert
            var mathSubject = subjectRepo.GetSubjectById(1);
            Assert.Contains(4, mathSubject.StudentsEnrolled);
        }
        
        [Fact]
        public void EnrollStudent_WhenAlreadyEnrolled_ShouldNotDuplicate()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            subjectRepo.EnrollStudent(2, 1); // Student 2 is already enrolled in Math
            
            // Assert
            var mathSubject = subjectRepo.GetSubjectById(1);
            Assert.Single(mathSubject.StudentsEnrolled.FindAll(id => id == 2));
        }
        
        [Fact]
        public void GetEnrolledSubjects_ShouldReturnStudentSubjects()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            var enrolledSubjects = subjectRepo.GetEnrolledSubjects(2);
            
            // Assert
            Assert.Single(enrolledSubjects);
            Assert.Equal("Math", enrolledSubjects[0].Name);
        }
        
        [Fact]
        public void DropSubject_ShouldRemoveStudentFromSubject()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            subjectRepo.DropSubject(2, 1); // Drop student 2 from Math
            
            // Assert
            var mathSubject = subjectRepo.GetSubjectById(1);
            Assert.DoesNotContain(2, mathSubject.StudentsEnrolled);
        }
        
        [Fact]
        public void AddSubject_ShouldAddSubjectToRepository()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            var newSubject = new Subject(3, "Chemistry", "Chemistry course", 1, new List<int>());
            
            // Act
            subjectRepo.AddSubject(newSubject);
            
            // Assert
            var subjects = subjectRepo.GetAllSubjects();
            Assert.Equal(3, subjects.Count);
            Assert.Contains(subjects, s => s.Name == "Chemistry");
        }
        
        [Fact]
        public void RemoveSubject_ShouldRemoveSubjectFromRepository()
        {
            // Arrange
            var subjectRepo = new SubjectRepo(_testFilePath);
            
            // Act
            subjectRepo.RemoveSubject(1); // Remove Math
            
            // Assert
            var subjects = subjectRepo.GetAllSubjects();
            Assert.Single(subjects);
            Assert.DoesNotContain(subjects, s => s.Name == "Math");
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