using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversityManager.Models;

namespace UniversityManager.ViewModels;

public partial class TeacherViewModel : BaseViewModel
{
    
    private readonly SubjectRepo _subjectRepo = SubjectRepo.Instance;
    private int _teacherId = 1;
   
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

    public TeacherViewModel()
    {
        Name = "Teacher";

        LoadSubjects();
    }

    private void LoadSubjects()
    {
        MySubjects = new ObservableCollection<Subject>(_subjectRepo.GetSubjectsByTeacher(_teacherId));
    }

    [RelayCommand]
    public void CreateSubject()
    {
        if (!string.IsNullOrWhiteSpace(NewSubjectName) && !string.IsNullOrWhiteSpace(NewSubjectDescription))
        {
            var newSubject = new Subject(_subjectRepo.GetAllSubjects().Count + 1, NewSubjectName, NewSubjectDescription, _teacherId, new List<int>());
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
}