using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using UniversityManager.Models;

namespace UniversityManager.ViewModels;

public partial class MainWindowViewModel : BaseViewModel
{
    [ObservableProperty] 
    private BaseViewModel _currentView;
    
    [ObservableProperty]
    private User _currentUser;
    
    [ObservableProperty]
    private Subject _selectedSubject;

    private readonly LogInViewModel _logInView;
    private readonly StudentViewModel _studentView;
    private readonly TeacherViewModel _teacherView;

    public MainWindowViewModel(
        LogInViewModel logInViewModel,
        StudentViewModel studentViewModel,
        TeacherViewModel teacherViewModel)
    {
        _logInView = logInViewModel;
        _studentView = studentViewModel;
        _teacherView = teacherViewModel;
        
        // Basically pass the user from the LogInViewModel through an Event
        _logInView.LoginSuccessful += OnLoginSuccessful;
        
        // Same thing but doesn't pass anything
        // OnLogoutRequested Runs when .LogoutRequested does
        _teacherView.LogoutRequested += OnLogoutRequested;
        _studentView.LogoutRequested += OnLogoutRequested;
        
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
    
    private void OnLogoutRequested()
    {
        CurrentUser = null;
        CurrentView = _logInView;
    }
}