using System.Collections.Generic;

namespace UniversityManager.Models;

public interface ISubjectRepository
{
    List<Subject> GetAllSubjects();
    List<Subject> GetAvailableSubjects(int studentId);
    Subject GetSubjectById(int subjectId);
    List<Subject> GetSubjectsByTeacher(int teacherId);
    void EnrollStudent(int studentId, int subjectId);
    List<Subject> GetEnrolledSubjects(int studentId);
    void DropSubject(int studentId, int subjectId);
    void AddSubject(Subject newSubject);
    void RemoveSubject(int subjectId);
    void UpdateSubject(Subject subject);
    void SaveSubjects();
}