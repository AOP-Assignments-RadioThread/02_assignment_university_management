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
    private readonly string filePath = "../Assets/Subjects.json";
    
    // Private constructor (ensures only this class can instantiate itself)
    private SubjectRepo()
    {
        LoadSubjects();
    }

    // Public accessor for Singleton instance
    public static SubjectRepo Instance => _instance.Value;
    
    // Load from Json
    private void LoadSubjects()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            
            // Load subject list from json, if null create new list
            subjects = JsonConvert.DeserializeObject<List<Subject>>(json) ?? new List<Subject>();
        }

        else
        {
            subjects = new List<Subject>();
        }
    }

    private void SaveSubjects()
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
    
    // Get the subjects taught by teacher by id
    public List<Subject> GetSubjectsByTeacher(int teacherId)
    {
        return subjects.Where(s => s.TeacherId == teacherId).ToList();
    }

    public void EnrollStudent(int studentId, int subjectId)
    {
        var subject = subjects.Find(s => s.Id == subjectId);
        if (subject == null)
        {
            if (!subject.StudentsEnrolled.Contains(studentId))
            {
                subject.StudentsEnrolled.Add(studentId);
            }
        }
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