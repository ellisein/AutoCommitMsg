using AutoCommitMsg.ViewModels;

namespace AutoCommitMsg.Views.Pages;

public partial class LoadingPage : System.Windows.Controls.UserControl
{
    public LoadingPageViewModel ViewModel { get; }

    public LoadingPage()
    {
        InitializeComponent();
        ViewModel = new LoadingPageViewModel();
        DataContext = ViewModel;
    }
}
