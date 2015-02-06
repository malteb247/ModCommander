
namespace ModCommander.Service
{
    public interface IDialogService
    {
        string SelectDirectoryDialog(string defaultPath, string title = null);
        bool ShowInfoMessage(string caption, string text);
        bool ShowWarningMessage(string caption, string text);
        bool ShowQuestionMessage(string caption, string text);
        bool ShowErrorMessage(string caption, string text, System.Exception ex = null);
    }
}
