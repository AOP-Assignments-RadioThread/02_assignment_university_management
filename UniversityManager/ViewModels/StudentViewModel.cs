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
    
    // Student ID - in a real app this would come from authentication
    private int _studentId = 1;

    public StudentViewModel()
    {
        Name = "Student";
        
        EnrolledSubjects = new ObservableCollection<Subject>(subjectRepo.GetEnrolledSubjects(_studentId));
        AvailableSubjects = new ObservableCollection<Subject>(subjectRepo.GetAvailableSubjects(_studentId));
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
    
    private void RefreshSubjects()
    {
        // Get enrolled subjects for this student
        var enrolled = subjectRepo.GetEnrolledSubjects(_studentId);
        EnrolledSubjects = new ObservableCollection<Subject>(enrolled);

        // Get available subjects for this student
        var available = subjectRepo.GetAvailableSubjects(_studentId);
        AvailableSubjects = new ObservableCollection<Subject>(available);
    }
}