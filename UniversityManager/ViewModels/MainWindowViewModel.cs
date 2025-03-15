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
    private Subject _selectedSubject;

    private readonly StudentViewModel _studentView = new StudentViewModel();
    private readonly TeacherViewModel _teacherView = new TeacherViewModel();

    public MainWindowViewModel()
    {
        //Schimba asta in _teacherView ca sa incepi a lucra la teacher view
        CurrentView = _teacherView;

      



    }
    
    
    
    
}