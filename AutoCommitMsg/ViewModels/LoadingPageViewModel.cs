using CommunityToolkit.Mvvm.ComponentModel;

namespace AutoCommitMsg.ViewModels;

public partial class LoadingPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _statusMessage = "Loading...";
}
