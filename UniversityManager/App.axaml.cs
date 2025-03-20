using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using UniversityManager.Models;
using UniversityManager.ViewModels;
using UniversityManager.Views;

namespace UniversityManager;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Create repositories
            IUserRepository userRepository = new UserRepo();
            ISubjectRepository subjectRepository = new SubjectRepo();
            
            // Create view models with DI
            var loginViewModel = new LogInViewModel(userRepository);
            var studentViewModel = new StudentViewModel(subjectRepository);
            var teacherViewModel = new TeacherViewModel(subjectRepository);
            
            // Create main view model
            var mainViewModel = new MainWindowViewModel(
                loginViewModel,
                studentViewModel,
                teacherViewModel
            );
            
            desktop.MainWindow = new MainWindow()
            {
                DataContext = mainViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}