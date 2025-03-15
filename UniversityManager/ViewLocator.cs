namespace UniversityManager;
using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using UniversityManager.Views;
using UniversityManager.ViewModels;



public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;
        
        var name = data.GetType().FullName!.Replace("ViewModels", "Views");
        name = name.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            var instance = (Control)Activator.CreateInstance(type)!;
            instance.DataContext = data;
            return instance;
        }
        
        // Try to find the view type by assembly scanning
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = assembly.GetType(name);
            if (type != null)
            {
                var instance = (Control)Activator.CreateInstance(type)!;
                instance.DataContext = data;
                return instance;
            }
        }
        
        return new TextBlock { Text = $"View not found: {name}" };
    }

    public bool Match(object? data) => data is BaseViewModel;
}