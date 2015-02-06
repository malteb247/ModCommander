using GalaSoft.MvvmLight.Messaging;
using ModCommander.Common;
using ModCommander.Data;
using ModCommander.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModCommander.View
{
    public class ProfileViewModel : ViewModelBase
    {
        Profile innerModel;
        IDialogService dialogService = new DialogService();

        public string Id
        {
            get
            {
                return innerModel.Id;
            }
        }

        public string Name
        {
            get { return innerModel.Name; }
            set
            {
                innerModel.Name = value;
                NotifyPropertyChanged();
                Save.RaiseCanExecuteChanged();
            }
        }

        public string Path
        {
            get { return innerModel.Path; }
            set
            {
                innerModel.Path = value;
                NotifyPropertyChanged();
            }
        }

        public string Url
        {
            get { return innerModel.Uri; }
            set
            {
                innerModel.Uri = value;
                NotifyPropertyChanged();
            }
        }

        public string User
        {
            get { return innerModel.User; }
            set
            {
                innerModel.User = value;
                NotifyPropertyChanged();
            }
        }

        public string Password
        {
            get { return innerModel.Password; }
            set
            {
                innerModel.Password = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsActive
        {
            get { return innerModel.IsActive; }
            set
            {
                innerModel.IsActive = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsNotActive { get { return !innerModel.IsActive; } }

        public ProfileViewModel(Profile model)
        {
            this.innerModel = model;
            Save.RaiseCanExecuteChanged();
        }

        private RelayCommand saveCommand;
        public RelayCommand Save
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(ExecSave, CanSave);

                return saveCommand;
            }
        }

        public bool CanSave()
        {
            return !string.IsNullOrEmpty(Name);
        }

        public void ExecSave(object param)
        {
            ProfileRepository.SaveProfile(this.innerModel);

            Messenger.Default.Send<string>("profileEdited");
        }

        private RelayCommand cancelCommand;
        public RelayCommand Cancel
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new RelayCommand(ExecCancel);

                return cancelCommand;
            }
        }

        public void ExecCancel(object param)
        {
            //ProfileRepository.LoadProfiles();
            Profile tmp = ProfileRepository.LoadProfile(innerModel.Id);

            if (tmp != null)
            {
                Name = tmp.Name;
                Path = tmp.Path;
                IsActive = tmp.IsActive;
            }

            Messenger.Default.Send<string>("profileEdited");
        }

        private RelayCommand selectPathCommand;
        public RelayCommand SelectPath
        {
            get
            {
                if (selectPathCommand == null)
                    selectPathCommand = new RelayCommand(ExecSelectPath);

                return selectPathCommand;
            }
        }

        public void ExecSelectPath(object param)
        {
            string selectedPath = dialogService.SelectDirectoryDialog("");

            if (!string.IsNullOrEmpty(selectedPath))
                Path = selectedPath;
        }

    }
}
