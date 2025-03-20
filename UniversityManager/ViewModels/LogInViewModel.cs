using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UniversityManager.Models;

namespace UniversityManager.ViewModels;

public partial class LogInViewModel : BaseViewModel 
{
    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty] 
    private string _userName = String.Empty;

    [ObservableProperty] 
    private bool _isLoginFailed;
    
    [ObservableProperty]
    private string _errorMessage;
    
    private readonly IUserRepository _userRepo;
    
    public event Action<User> LoginSuccessful;

    public LogInViewModel(IUserRepository userRepository)
    {
        _userRepo = userRepository;
    }

    [RelayCommand]
    public void Login()
    {
        IsLoginFailed = false;
        ErrorMessage = string.Empty;
        
        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Username and password are required";
            IsLoginFailed = true;
            return;
        }
        
        var user = _userRepo.AuthenticateUser(UserName, Password);
        
        if (user != null)
        {
            LoginSuccessful?.Invoke(user);
            UserName = string.Empty;
            Password = string.Empty;
        }
        else
        {
            ErrorMessage = "Invalid username or password";
            IsLoginFailed = true;
        }
    }
}