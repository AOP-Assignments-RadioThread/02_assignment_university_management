using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversityManager.Models;

namespace UniversityManager.ViewModels;

public partial class TeacherViewModel : BaseViewModel
{
    private readonly ISubjectRepository _subjectRepo;
    
    [ObservableProperty]
    private int _teacherId;
   
    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private ObservableCollection<Subject> _mySubjects;

    [ObservableProperty]
    private Subject _selectedSubject;

    [ObservableProperty]
    private string _newSubjectName;

    [ObservableProperty]
    private string _newSubjectDescription;
    
    public event Action LogoutRequested;

    public TeacherViewModel(ISubjectRepository subjectRepository)
    {
        _subjectRepo = subjectRepository;
        MySubjects = new ObservableCollection<Subject>();
    }

    private void LoadSubjects()
    {
        var subjects = _subjectRepo.GetSubjectsByTeacher(TeacherId);
        MySubjects = new ObservableCollection<Subject>(subjects);
        
        // First set to null to force property change
        SelectedSubject = null;
        
        // Then set to first subject if available
        if (subjects.Any())
        {
            SelectedSubject = subjects.First();
            
            // Force UI refresh by reassigning the same value
            var temp = SelectedSubject;
            SelectedSubject = null;
            SelectedSubject = temp;
        }
    }

    [RelayCommand]
    public void CreateSubject()
    {
        if (!string.IsNullOrWhiteSpace(NewSubjectName) && !string.IsNullOrWhiteSpace(NewSubjectDescription))
        {
            var newSubject = new Subject(
                _subjectRepo.GetAllSubjects().Count + 1, 
                NewSubjectName, 
                NewSubjectDescription, 
                _teacherId, 
                new List<int>()
            );
            
            _subjectRepo.AddSubject(newSubject);
            _subjectRepo.SaveSubjects();
            LoadSubjects();
            
            NewSubjectName = string.Empty;
            NewSubjectDescription = string.Empty;
        }
    }

    [RelayCommand]
    public void DeleteSubject()
    {
        if (SelectedSubject != null)
        {
            _subjectRepo.RemoveSubject(SelectedSubject.Id);
            _subjectRepo.SaveSubjects();
            LoadSubjects();
            SelectedSubject = null;
        }
    }

    [RelayCommand]
    public void Logout()
    {
        LogoutRequested?.Invoke();
    }
    
    public void Initialize(string name, int id)
    {
        Name = name;
        TeacherId = id;
        LoadSubjects();
    }
}