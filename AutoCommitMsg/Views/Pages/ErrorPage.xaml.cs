namespace AutoCommitMsg.Views.Pages;

public partial class ErrorPage : System.Windows.Controls.UserControl
{
    public string ErrorMessage { get; }

    public ErrorPage(string errorMessage)
    {
        ErrorMessage = errorMessage;
        InitializeComponent();
        DataContext = this;
    }

    public event Action? OnReturnRequested;

    private void ReturnButton_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        OnReturnRequested?.Invoke();
    }
}
