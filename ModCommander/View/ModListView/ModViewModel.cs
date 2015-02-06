using ModCommander.Data;
using ModCommander.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ModCommander.View
{
    public sealed class ModViewModel : ViewModelBase
    {
        Logger log = LogManager.GetCurrentClassLogger();
        ModData model;

        #region Model properties

        public string Author
        {
            get { return model.Author; }
            set
            {
                model.Author = value;
                NotifyPropertyChanged();
            }
        }

        public string Version
        {
            get { return model.Version; }
            set
            {
                model.Version = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsMultiplayerReady
        {
            get { return model.IsMultiplayerReady; }
            set
            {
                model.IsMultiplayerReady = value;
                NotifyPropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                return model.Title;
            }
            set
            {
                //model.Author = value;
                NotifyPropertyChanged();
            }
        }

        public string Description
        {
            get
            {

                return model.Description;
            }
            set
            {
                //model.Author = value;
                NotifyPropertyChanged();
            }
        }

        private ImageSource imageSource;
        public ImageSource Image
        {
            get
            {
                if (imageSource == null && model.Image != null)
                    imageSource = model.Image.CreateBitmapSource();

                return imageSource;
            }
            set
            {
                //innerDetail.Image = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return Path.GetExtension(model.FileName) != ".disabled"; }
            set { NotifyPropertyChanged(); }
        }

        private ObservableCollection<StoreItemDataViewModel> storeItems;

        public ObservableCollection<StoreItemDataViewModel> StoreItems
        {
            get
            {
                if (storeItems == null)
                {
                    storeItems = new ObservableCollection<StoreItemDataViewModel>(model.StoreItems.Select(x => new StoreItemDataViewModel(x)));
                }

                return storeItems;
            }
            set { NotifyPropertyChanged(); }
        }

        #endregion Model properties

        #region ViewModelProperties

        public string FileNameAndSizeFormated
        {
            get
            {
                double size = (double)(model.FileSize / 1024.0) / 1024.0;
                return string.Format("{0} ({1:0.00} MB)", Path.GetFileName(model.FullPath), size);
            }
        }
        #endregion

        public ModViewModel(ModData model)
        {
            this.model = model;
        }

        #region Helper

        public void ChangeState()
        {
            model.FullPath = IsEnabled ? MoveMod(".disabled") : MoveMod(".zip");

            IsEnabled = true;
        }

        string MoveMod(string newExtension)
        {
            string newPath = Path.ChangeExtension(model.FullPath, newExtension);

            try
            {
                File.Move(model.FullPath, newPath);
            }
            catch (Exception ex)
            {
                log.Error("Can't move mod '{0}'".Args(model.FullPath), ex);
                return model.FullPath;
            }

            return newPath;
        }

        #endregion Helper
    }
}
