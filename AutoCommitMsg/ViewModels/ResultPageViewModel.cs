using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AutoCommitMsg.ViewModels;

public partial class ResultPageViewModel : ObservableObject
{
    public ObservableCollection<string> CommitMessages { get; init; } = [];

    public event Action? OnRegenerateRequested;
    public event Action? OnBackToStartRequested;

    [RelayCommand]
    private void Regenerate() => OnRegenerateRequested?.Invoke();

    [RelayCommand]
    private void BackToStart() => OnBackToStartRequested?.Invoke();

    [RelayCommand]
    private void CopyMessage(string message)
    {
        Clipboard.SetText(message);
    }
}
