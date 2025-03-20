using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace UniversityManager.Models;

public class SubjectRepo : ISubjectRepository
{
    private List<Subject> subjects;
    private readonly string _filePath;

    public SubjectRepo(string filePath)
    {
        _filePath = filePath;
        LoadSubjects();
    }

    // Production constructor
    public SubjectRepo() : this(Path.Combine(
        Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, 
        "Assets", 
        "Subjects.json"))
    {
    }

    private void LoadSubjects()
    {
        string directory = Path.GetDirectoryName(_filePath);
        
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            subjects = JsonConvert.DeserializeObject<List<Subject>>(json) ?? new List<Subject>();
        }
        else
        {
            subjects = new List<Subject>();
            File.WriteAllText(_filePath, "[]");
        }
    }

    public void SaveSubjects()
    {
        var json = JsonConvert.SerializeObject(subjects);
        File.WriteAllText(_filePath, json);
    }
    
    public List<Subject> GetAllSubjects()
    {
        return subjects;
    }

    public List<Subject> GetAvailableSubjects(int studentId)
    {
        return subjects.Where(s => !s.StudentsEnrolled.Contains(studentId)).ToList();
    }

    public Subject GetSubjectById(int subjectId)
    {
        return subjects.Find(s => s.Id == subjectId);
    }
    
    public List<Subject> GetSubjectsByTeacher(int teacherId)
    {
        return subjects.Where(s => s.TeacherId == teacherId).ToList();
    }

    public void EnrollStudent(int studentId, int subjectId)
    {
        var subject = subjects.Find(s => s.Id == subjectId);
        if (subject != null)
        {
            if (!subject.StudentsEnrolled.Contains(studentId))
            {
                subject.StudentsEnrolled.Add(studentId);
            }
        }
    }

    public List<Subject> GetEnrolledSubjects(int studentId)
    {
        return subjects.Where(s => s.StudentsEnrolled.Contains(studentId)).ToList();
    }

    public void DropSubject(int studentId, int subjectId)
    {
        var subject = subjects.Find(s => s.Id == subjectId);
        if (subject != null)
        {
            if (subject.StudentsEnrolled.Contains(studentId))
            {
                subject.StudentsEnrolled.Remove(studentId);
            }
        }
    }

    public void AddSubject(Subject newSubject)
    {
        subjects.Add(newSubject);
    }

    public void RemoveSubject(int subjectId)
    {
        var subject = subjects.Find(s => s.Id == subjectId);
        if (subject != null)
        {
            subjects.Remove(subject);
        }
    }
}