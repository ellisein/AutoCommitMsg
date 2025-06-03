using AutoCommitMsg.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AutoCommitMsg.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private FolderItem? _selectedFolder;

    public event Action? OnSelectFolder;

    public MainPageViewModel()
    {

    }

    [RelayCommand]
    private void SelectFolder()
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select a git repository folder",
            UseDescriptionForTitle = true,
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            var path = dialog.SelectedPath;
            if (!string.IsNullOrEmpty(path))
            {
                SelectedFolder = new FolderItem { Path = path };
                OnSelectFolder?.Invoke();
            }
        }
    }
}
