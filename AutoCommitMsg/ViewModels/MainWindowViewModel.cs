using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoCommitMsg.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private AppLanguage _selectedLanguage = AppLanguage.English;

    [RelayCommand]
    private void ChangeLanguage(AppLanguage language)
    {
        SelectedLanguage = language;
    }
}

public enum AppLanguage
{
    English,
    Korean,
}
