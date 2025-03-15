using System.Collections.Generic;

namespace UniversityManager.Models;

public class Subject
{
    public int Id{get;set;}
    public string Name{get;set;}
    public string Description{get;set;}
    public int TeacherId{get;set;}
    public List<int> StudentsEnrolled{get;set;}


    public Subject(int id, string name, string description, int teacherId, List<int> studentIds)
    {
        StudentsEnrolled = studentIds;
        Id = id;
        Name = name;
        Description = description;
        TeacherId = teacherId;
    }

    public override string ToString()
    {
        return $"Name: {Name}, Description: {Description}, TeacherId: {TeacherId}, StudentsEnrolled: {StudentsEnrolled}, SubjectId: {Id}";
    }
}