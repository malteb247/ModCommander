using GongSolutions.Wpf.DragDrop;
using ModCommander.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ModCommander.View
{
    public class LocalModsViewModel : ViewModelBase, IDropTarget
    {
        private ModViewModel selectedEnabledMod;
        private ModViewModel selectedDisabledMod;

        public ModViewModel SelectedEnabledMod
        {
            get { return selectedEnabledMod; }
            set
            {
                selectedEnabledMod = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<ModViewModel> allMods;

        public ObservableCollection<ModViewModel> AllMods
        {
            get { return allMods; }
            set
            {
                allMods = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<ModViewModel> EnabledMods
        {
            get { return new ObservableCollection<ModViewModel>(GetModList(EnabledSearchText, true)); }
        }

        public ObservableCollection<ModViewModel> DisabledMods
        {
            get { return new ObservableCollection<ModViewModel>(GetModList(DisabledSearchText, false)); }
        }

        public ModViewModel SelectedDisabledMod
        {
            get { return selectedDisabledMod; }
            set
            {
                selectedDisabledMod = value;
                NotifyPropertyChanged();
            }
        }

        private string enabledSearchText;

        public string EnabledSearchText
        {
            get { return enabledSearchText; }
            set
            {
                enabledSearchText = value;
                NotifyPropertyChanged();               
            }
        }

        private string disabledSearchText;

        public string DisabledSearchText
        {
            get { return disabledSearchText; }
            set
            {
                disabledSearchText = value;
                NotifyPropertyChanged();
            }
        }

        public LocalModsViewModel()
        {
            ModExtractor modex = new ModExtractor();
            var modData = modex.LoadModsFromDirectoryAsync(@"g:\ls\sp\").Select(x => new ModViewModel(x));

            AllMods = new ObservableCollection<ModViewModel>(modData);

            //NotifyPropertyChanged(() => EnabledMods);
            //NotifyPropertyChanged(() => DisabledMods);

        }

        #region Drag & drop

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget as ListView != null)
            {
                if (dropInfo.VisualTarget.GetHashCode() != dropInfo.DragInfo.VisualSource.GetHashCode())
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Move;
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            List<ModViewModel> itemsToMove = new List<ModViewModel>();

            if (dropInfo.Data is ModViewModel)
            {
                itemsToMove.Add((ModViewModel)dropInfo.Data);
            }
            else if (dropInfo.Data is IEnumerable<ModViewModel>)
            {
                itemsToMove = new List<ModViewModel>((IEnumerable<ModViewModel>)dropInfo.Data);
            }
            else
            {
                return;
            }

            foreach (var item in itemsToMove)
            {
                Task.Factory.StartNew(() => { item.ChangeState(); }).ContinueWith((t) =>
                    {
                        NotifyPropertyChanged(() => EnabledMods);
                        NotifyPropertyChanged(() => DisabledMods);
                        this.SelectedDisabledMod = item;
                    });
            }
        }

        #endregion Drag & drop

        #region filtering

        IEnumerable<ModViewModel> GetModList(string searchText, bool enabled)
        {
            if (string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText))
            {
                return AllMods.Where(x => x.IsEnabled == enabled);
            }

            return AllMods.Where(x =>
                x.IsEnabled == enabled &&
                (
                    x.Title.ToLowerInvariant().Contains(searchText.ToLowerInvariant()) ||
                    x.Description.ToLowerInvariant().Contains(searchText.ToLowerInvariant())
                ));

        }

        bool FilterEnabled(object item)
        {
            ModViewModel m = item as ModViewModel;

            if (!m.IsEnabled)
                return false;

            if (string.IsNullOrEmpty(EnabledSearchText))
                return true;


            if (m.Title.ToLowerInvariant().Contains(EnabledSearchText.ToLowerInvariant()) || m.Description.ToLowerInvariant().Contains(EnabledSearchText.ToLowerInvariant()))
                return true;

            return false;
        }

        bool FilterDisabled(object item)
        {
            ModViewModel m = item as ModViewModel;

            if (m.IsEnabled)
                return false;

            if (string.IsNullOrEmpty(EnabledSearchText))
                return true;


            if (m.Title.ToLowerInvariant().Contains(DisabledSearchText.ToLowerInvariant()) || m.Description.ToLowerInvariant().Contains(DisabledSearchText.ToLowerInvariant()))
                return true;

            return false;
        }

        #endregion filtering
    }
}
