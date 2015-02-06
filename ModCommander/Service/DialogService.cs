namespace ModCommander.Service
{
    using Microsoft.WindowsAPICodePack.Dialogs;
    using System;
    using System.Windows;

    public class DialogService : IDialogService
    {
        public string SelectDirectoryDialog(string defaultPath, string title = null)
        {
            var dlg = new CommonOpenFileDialog()
            {
                Title = "",
                IsFolderPicker = true,
                InitialDirectory = defaultPath,
                AddToMostRecentlyUsedList = false,
                AllowNonFileSystemItems = false,
                DefaultDirectory = defaultPath,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dlg.FileName;
            }

            return null;
        }

        public bool ShowInfoMessage(string caption, string text)
        {
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);

            if (result == MessageBoxResult.OK)
                return true;

            return false;
        }

        public bool ShowWarningMessage(string caption, string text)
        {
            return MessageBoxResult.OK ==  Xceed.Wpf.Toolkit.MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public bool ShowQuestionMessage(string caption, string text)
        {
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }

        public bool ShowErrorMessage(string caption, string text, Exception ex = null)
        {
            if (ex != null)
                text += "\r\n\r\n" + ex.ToString();

            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Error);

            if (result == MessageBoxResult.OK)
                return true;

            return false;
        }
    }
}
