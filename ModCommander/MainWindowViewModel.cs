namespace ModCommander
{
    using ModCommander.Common;
    using ModCommander.View;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using ModCommander.Utils;

    public class MainWindowViewModel : ViewModelBase
    {
        const string defaultTitle = "Mod Commander";

        string title = defaultTitle;
        GameVersion currentGameVersion;
        bool isVersionSelectModalDialogVisible;
        RelayCommand versionSelectCommand;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged();
            }
        }

        public string Version
        {
            get
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;

                return "{0}.{1}.{2}".Args(v.Major, v.Minor, v.Build);
            }
        }

        public GameVersion CurrentGameVersion
        {
            get { return currentGameVersion; }
            set
            {
                currentGameVersion = value;
                NotifyPropertyChanged();

                Title = defaultTitle + string.Format(" ({0})", currentGameVersion);
            }
        }
        
        public bool IsVersionSelectModalDialogVisible
        {
            get { return isVersionSelectModalDialogVisible; }
            set
            {
                isVersionSelectModalDialogVisible = value;
                base.NotifyPropertyChanged();
            }
        }
        
        public RelayCommand VersionSelectCommand
        {
            get
            {
                if (versionSelectCommand == null)
                {
                    versionSelectCommand = new RelayCommand(ExecVersionSelectCommand);

                }
                return versionSelectCommand;
            }
        }

        private void ExecVersionSelectCommand(object param)
        {
            switch (param.ToString())
            {
                case "11":
                    CurrentGameVersion = GameVersion.LS11;
                    break;
                case "13":
                    CurrentGameVersion = GameVersion.LS13;
                    break;
                case "15":
                    CurrentGameVersion = GameVersion.LS15;
                    break;
            }



            IsVersionSelectModalDialogVisible = false;
        }

        public MainWindowViewModel(bool showVersionSelector = true)
        {
            IsVersionSelectModalDialogVisible = showVersionSelector;
        }
    }
}
