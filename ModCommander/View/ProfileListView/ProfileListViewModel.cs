using GalaSoft.MvvmLight.Messaging;
using ModCommander.Common;
using ModCommander.Data;
using ModCommander.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ModCommander.Utils;

namespace ModCommander.View
{
    public class ProfileListViewModel : ViewModelBase
    {
        private ObservableCollection<ProfileViewModel> profiles = new ObservableCollection<ProfileViewModel>();

        public ObservableCollection<ProfileViewModel> Profiles
        {
            get
            {
                return profiles;
            }
            set
            {
                profiles = value;
                NotifyPropertyChanged();
            }
        }

        private ProfileViewModel selectedProfile;

        public ProfileViewModel SelectedProfile
        {
            get { return selectedProfile; }
            set
            {
                selectedProfile = value;
                NotifyPropertyChanged();
                activateCommand.RaiseCanExecuteChanged();
            }
        }

        private bool isEditDialogVisible = false;

        public bool IsEditDialogVisible
        {
            get { return isEditDialogVisible; }
            set
            {
                isEditDialogVisible = value;
                NotifyPropertyChanged();
            }
        }

        private bool isDeleteDialogVisible = false;

        public bool IsDeleteDialogVisible
        {
            get { return isDeleteDialogVisible; }
            set
            {
                isDeleteDialogVisible = value;
                NotifyPropertyChanged();
            }
        }

        public ProfileListViewModel()
        {
            RefreshProfileListFromDisk();

            Messenger.Default.Register<string>
            (
                    this,
                    (param) =>
                    {
                        if (param == "profileEdited" || param == "profileEditCanceled")
                        {
                            RefreshProfileListFromDisk();

                            IsEditDialogVisible = false;
                        }
                    }
            );
        }

        #region commands

        #region Edit Command

        private RelayCommand editCommand;

        public RelayCommand EditCommand
        {
            get
            {
                if (editCommand == null)
                    editCommand = new RelayCommand(ExecEditCommand, CanExecEditCommand);

                return editCommand;
            }
        }

        private void ExecEditCommand(object obj)
        {
            IsEditDialogVisible = true;
        }

        private bool CanExecEditCommand()
        {
            return selectedProfile != null;
        }

        #endregion

        #region Add Command
        private RelayCommand addCommand;

        public RelayCommand AddCommand
        {
            get
            {
                if (addCommand == null)
                    addCommand = new RelayCommand(ExecAddCommand);

                return addCommand;
            }
        }

        private void ExecAddCommand(object obj)
        {
            Profile p = new Profile();
            SelectedProfile = new ProfileViewModel(p);

            IsEditDialogVisible = true;
        }

        #endregion

        #region Activate Command
        private RelayCommand activateCommand;

        public RelayCommand ActivateCommand
        {
            get
            {
                if (activateCommand == null)
                    activateCommand = new RelayCommand(ExecActivateCommand, CanExecActivateCommand);

                return activateCommand;
            }
        }

        private void ExecActivateCommand(object obj)
        {
            Parallel.ForEach(Profiles, (item) => { item.IsActive = false; });

            selectedProfile.IsActive = true;
        }

        private bool CanExecActivateCommand()
        {
            return selectedProfile != null && !selectedProfile.IsActive;
        }
        #endregion

        #region Delete Command
        private RelayCommand deleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                    deleteCommand = new RelayCommand(ExecDeleteCommand, CanExecDeleteCommand);

                return deleteCommand;
            }
        }

        private void ExecDeleteCommand(object obj)
        {
            ProfileRepository.DeleteProfile(SelectedProfile.Id);
            RefreshProfileListFromDisk();
            IsDeleteDialogVisible = false;
        }

        private bool CanExecDeleteCommand()
        {
            return selectedProfile != null && !selectedProfile.IsActive;
        }
        #endregion

        #region ShowDelete Command
        private RelayCommand showDeleteCommand;

        public RelayCommand ShowDeleteCommand
        {
            get
            {
                if (showDeleteCommand == null)
                    showDeleteCommand = new RelayCommand(ExecShowDeleteCommand, CanExecShowDeleteCommand);

                return showDeleteCommand;
            }
        }

        private void ExecShowDeleteCommand(object obj)
        {
            IsDeleteDialogVisible = true;
        }

        private bool CanExecShowDeleteCommand()
        {
            return selectedProfile != null && !selectedProfile.IsActive;
        }

        #endregion

        #region CancelDelete Command
        private RelayCommand cancelDeleteCommand;

        public RelayCommand CancelDeleteCommand
        {
            get
            {
                if (cancelDeleteCommand == null)
                    cancelDeleteCommand = new RelayCommand(ExecCancelDeleteCommand);

                return cancelDeleteCommand;
            }
        }

        private void ExecCancelDeleteCommand(object obj)
        {
            IsDeleteDialogVisible = false;
        }

        #endregion


        #endregion commands

        void RefreshProfileListFromDisk()
        {
            Profiles = new ObservableCollection<ProfileViewModel>();

            var list = ProfileRepository.LoadProfiles();

            foreach (Profile p in list)
            {
                ProfileViewModel pvm = new ProfileViewModel(p);

                if (pvm.IsActive)
                    App.CurrentProfile = pvm;

                Profiles.Add(new ProfileViewModel(p));
            }
        }
    }
}
