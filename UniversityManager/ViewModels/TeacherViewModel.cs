using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversityManager.Models;

namespace UniversityManager.ViewModels;

public partial class TeacherViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty] 
    private bool _isEditing = false;

    [ObservableProperty]
    private string _editableDescription = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Subject> _mySubjects;
    
    [ObservableProperty]
    private Subject _selectedSubject;
    
    [ObservableProperty]
    private string _newSubjectName = string.Empty;
    
    [ObservableProperty]
    private string _newSubjectDescription = string.Empty;
    
    [ObservableProperty]
    private int _teacherId;
    
    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _showStatusMessage;
    
    private readonly ISubjectRepository _subjectRepo;
    public event Action LogoutRequested;

    public TeacherViewModel(ISubjectRepository subjectRepository)
    {
        _subjectRepo = subjectRepository;
    }
    
    private void LoadSubjects()
    {
        var subjects = _subjectRepo.GetSubjectsByTeacher(_teacherId);
        MySubjects = new ObservableCollection<Subject>(subjects);
        
        if (MySubjects.Count > 0 && SelectedSubject == null)
        {
            SelectedSubject = MySubjects[0];
        }
    }

    [RelayCommand]
    public void StartEdit()
    {
        if (SelectedSubject != null)
        {
            EditableDescription = SelectedSubject.Description;
            IsEditing = true;
        }
    }

    [RelayCommand]
    public void CancelEdit()
    {
        IsEditing = false;
        EditableDescription = string.Empty;
    }

    [RelayCommand]
    public void SaveEdit()
    {
        if (SelectedSubject != null && !string.IsNullOrWhiteSpace(EditableDescription))
        {
            string subjectName = SelectedSubject.Name;
            SelectedSubject.Description = EditableDescription;
            _subjectRepo.UpdateSubject(SelectedSubject);
            _subjectRepo.SaveSubjects();

            StatusMessage = $"Subject '{subjectName}' updated successfully!";
            ShowStatusMessage = true;
            HideStatusMessageAfterDelay();

            IsEditing = false;
            EditableDescription = string.Empty;
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
            
            StatusMessage = $"Subject '{NewSubjectName}' created successfully!";
            ShowStatusMessage = true;
            HideStatusMessageAfterDelay();
            
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
            string subjectName = SelectedSubject.Name;
            _subjectRepo.RemoveSubject(SelectedSubject.Id);
            _subjectRepo.SaveSubjects();
            
            // Show status message
            StatusMessage = $"Subject '{subjectName}' deleted successfully!";
            ShowStatusMessage = true;
            HideStatusMessageAfterDelay();
            
            LoadSubjects();
            SelectedSubject = null;
        }
    }
    
    private void HideStatusMessageAfterDelay()
    {
        Task.Delay(3000).ContinueWith(_ => 
        {
            ShowStatusMessage = false;
        });
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