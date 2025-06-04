using AutoCommitMsg.Services;
using AutoCommitMsg.ViewModels;
using AutoCommitMsg.Views.Pages;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

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
        var mainVm = new MainPageViewModel();
        var mainPage = new MainPage { DataContext = mainVm };

        mainVm.OnSelectFolder += async () =>
        {
            await GenerateCommitMessages(mainVm.SelectedFolder?.Path);
        };

        MainContent.Content = mainPage;
    }

    private void ShowResultPage(string? folderPath, string response)
    {
        var resultVm = new ResultPageViewModel
        {
            CommitMessages = new ObservableCollection<string>(
                response.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        };
        var resultPage = new ResultPage { DataContext = resultVm };

        resultVm.OnRegenerateRequested += async () =>
        {
            await GenerateCommitMessages(folderPath);
        };

        resultVm.OnBackToStartRequested += ShowMainPage;

        MainContent.Content = resultPage;
    }

    private async Task GenerateCommitMessages(string? folderPath)
    {
        try
        {
            var loadingPage = new LoadingPage();
            MainContent.Content = loadingPage;

            loadingPage.ViewModel.StatusMessage = "Checking folder...";
            GitService.CheckGitRepository(folderPath);

            loadingPage.ViewModel.StatusMessage = "Loading repository data...";
            var gitLogs = GitService.GetGitLogs(folderPath, 10);
            var gitDiff = GitService.GetGitDiff(folderPath);

            loadingPage.ViewModel.StatusMessage = "Generating commit message...";
            var response = await AiService.GenerateCommitMessagesAsync(gitLogs, gitDiff);
            Debug.WriteLine(response);

            ShowResultPage(folderPath, response);
        }
        catch (Exception ex)
        {
            var errorPage = new ErrorPage(ex.Message);
            errorPage.OnReturnRequested += ShowMainPage;
            MainContent.Content = errorPage;
        }
    }
}
