using System.Windows;
using AutoCommitMsg.Services;
using AutoCommitMsg.ViewModels;
using AutoCommitMsg.Views.Pages;

namespace AutoCommitMsg;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ShowMainPage();
    }

    private void ShowMainPage()
    {
        var vm = new MainPageViewModel();
        var page = new MainPage { DataContext = vm };

        vm.OnSelectFolder += async () =>
        {
            try
            {
                var loadingPage = new LoadingPage();
                MainContent.Content = loadingPage;

                loadingPage.ViewModel.StatusMessage = "Checking folder...";
                GitService.CheckGitRepository(vm.SelectedFolder?.Path);
            }
            catch (Exception ex)
            {
                var errorPage = new ErrorPage(ex.Message);
                errorPage.OnReturnRequested += ShowMainPage;
                MainContent.Content = errorPage;
            }
        };

        MainContent.Content = page;
    }
}
