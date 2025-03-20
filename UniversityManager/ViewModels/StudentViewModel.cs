using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversityManager.Models;

namespace UniversityManager.ViewModels;

public partial class StudentViewModel : BaseViewModel
{
    [ObservableProperty]
    private string? _name;
    
    [ObservableProperty]
    private ObservableCollection<Subject>? _enrolledSubjects;
    
    [ObservableProperty]
    private ObservableCollection<Subject>? _availableSubjects;
    
    [ObservableProperty]
    private Subject? _enrolledSelectedSubject;
    
    [ObservableProperty]
    private Subject? _availableSelectedSubject;

    private SubjectRepo subjectRepo = SubjectRepo.Instance;
    public event Action LogoutRequested;

    
    [ObservableProperty]
    private int _studentId;

    public StudentViewModel()
    {
        
    }

    [RelayCommand]
    public void EnrollInSelectedSubject()
    {
        if (AvailableSelectedSubject != null)
        {
            int subjectId = AvailableSelectedSubject.Id;

            subjectRepo.EnrollStudent(_studentId, subjectId);
            subjectRepo.SaveSubjects();

           RefreshSubjects();

            AvailableSelectedSubject = null;
            
        }
    }

   

    [RelayCommand]
    public void DropSelectedSubject()
    {
        if (EnrolledSelectedSubject != null)
        {
            int subjectId = EnrolledSelectedSubject.Id;

            subjectRepo.DropSubject(_studentId, subjectId);
            subjectRepo.SaveSubjects();

           RefreshSubjects();

            EnrolledSelectedSubject = null;
        }
    }

    public void Initialize(string name, int id)
    {
        Name = name;
        StudentId = id;
        RefreshSubjects();
    }
    
    private void RefreshSubjects()
    {
        //This is to ensure UI displays everything properly
        
        // Get enrolled subjects for this student
        var enrolled = subjectRepo.GetEnrolledSubjects(_studentId);
        EnrolledSubjects = new ObservableCollection<Subject>(enrolled);
        
        // First set to null to force property change
        EnrolledSelectedSubject = null;
        
        // Auto-select first enrolled subject if any exist
        if (enrolled.Any())
        {
            EnrolledSelectedSubject = enrolled.First();
            
            // Force UI refresh by reassigning the same value
            var temp = EnrolledSelectedSubject;
            EnrolledSelectedSubject = null;
            EnrolledSelectedSubject = temp;
        }

        // Get available subjects for this student
        var available = subjectRepo.GetAvailableSubjects(_studentId);
        AvailableSubjects = new ObservableCollection<Subject>(available);
        
        // First set to null to force property change
        AvailableSelectedSubject = null;
        
        // Auto-select first available subject if any exist
        if (available.Any())
        {
            AvailableSelectedSubject = available.First();
            
            // Force UI refresh by reassigning the same value
            var temp = AvailableSelectedSubject;
            AvailableSelectedSubject = null;
            AvailableSelectedSubject = temp;
        }
    }
    
    [RelayCommand]
    public void Logout()
    {
        // Notify that logout was requested
        LogoutRequested?.Invoke();
    }
}