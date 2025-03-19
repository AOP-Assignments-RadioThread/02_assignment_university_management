using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace UniversityManager.Models;

public class SubjectRepo
{
    private static readonly Lazy<SubjectRepo> _instance = new Lazy<SubjectRepo>(() => new SubjectRepo());
    private List<Subject> subjects;
    public static string filePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Assets", "Subjects.json");

    // Private constructor (ensures only this class can instantiate itself)
    private SubjectRepo()
    {
        Console.WriteLine(filePath);
        LoadSubjects();
    }

    // Public accessor for Singleton instance
    public static SubjectRepo Instance => _instance.Value;

    // Load from Json
    private void LoadSubjects()
    {
        string directory = Path.GetDirectoryName(filePath);
        
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            subjects = JsonConvert.DeserializeObject<List<Subject>>(json) ?? new List<Subject>();
        }
        else
        {
            subjects = new List<Subject>();
            File.WriteAllText(filePath, "[]");
        }
    }

    public void SaveSubjects()
    {
        var json = JsonConvert.SerializeObject(subjects);
        File.WriteAllText(filePath, json);
    }
    
    public List<Subject> GetAllSubjects()
    {
        return subjects;
    }

    // Get the available subjects for a student, the ones he is not already enrolled in
    public List<Subject> GetAvailableSubjects(int studentId)
    {
        return subjects.Where(s => !s.StudentsEnrolled.Contains(studentId)).ToList();
    }

    public Subject GetSubjectById(int subjectId)
    {
        return subjects.Find(s => s.Id == subjectId);
    }
    
    // Get the subjects taught by teacher by id
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