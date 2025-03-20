using CommunityToolkit.Mvvm.ComponentModel;

namespace UniversityManager.ViewModels;

public partial class LogInViewModel : BaseViewModel 
{
    [ObservableProperty]
    private string _password;

    [ObservableProperty] 
    private string _userName;

    public LogInViewModel()
    {
        
    }
    
}