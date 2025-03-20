using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using UniversityManager.Models;

namespace UniversityManager.ViewModels;

public partial class MainWindowViewModel : BaseViewModel
{
    private SubjectRepo subjects;

    [ObservableProperty] 
    private BaseViewModel _currentView;
    
    [ObservableProperty]
    private User _currentUser;
    
    [ObservableProperty]
    private Subject _selectedSubject;

    private readonly StudentViewModel _studentView = new StudentViewModel();
    private readonly TeacherViewModel _teacherView = new TeacherViewModel();
    private readonly LogInViewModel _logInView = new LogInViewModel();

    public MainWindowViewModel()
    {
        _logInView = new LogInViewModel();
        _teacherView = new TeacherViewModel();
        
        // Subscribe to login successful event
        // Basically pass the user from the LogInViewModel through an Event
        _logInView.LoginSuccessful += OnLoginSuccessful;
        
        // Start with login view
        CurrentView = _logInView;
    }

    private void OnLoginSuccessful(User user)
    {
        CurrentUser = user;

        switch (user.Role)
        {
            case UserRole.Student:
                _studentView.Initialize(user.Username, user.Id);
                CurrentView = _studentView;
                break;
            case UserRole.Teacher:
                _teacherView.Initialize(user.Username, user.Id);
                CurrentView = _teacherView;
                break;
        }
        
    }
    
    
    
    
}