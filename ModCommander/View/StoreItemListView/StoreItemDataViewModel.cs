using ModCommander.Data;
using ModCommander.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ModCommander.View
{
    public sealed class StoreItemDataViewModel : ViewModelBase
    {
        StoreItemData model;

        #region Model properties

        public string Name
        {
            get { return model.Name; }
            set { NotifyPropertyChanged(); }
        }

        public string Description
        {
            get { return model.Description; }
            set { NotifyPropertyChanged(); }
        }

        public string MachineType
        {
            get { return model.MachineType; }
            set { NotifyPropertyChanged(); }
        }

        public string Specs
        {
            get { return model.Specs; }
            set { NotifyPropertyChanged(); }
        }

        public string Brand
        {
            get { return model.Brand; }
            set { NotifyPropertyChanged(); }
        }

        public double Price
        {
            get { return model.Price; }
            set { NotifyPropertyChanged(); }
        }

        public double Upkeep
        {
            get { return model.Upkeep; }
            set { NotifyPropertyChanged(); }
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

        private ImageSource brandImageSource;
        public ImageSource ImageBrand
        {
            get
            {
                if (brandImageSource == null && model.BrandImage != null)
                    brandImageSource = model.BrandImage.CreateBitmapSource();

                return brandImageSource;
            }
            set
            {
                //innerDetail.Image = value;
                NotifyPropertyChanged();
            }
        }
        
        #endregion Model properties

        public StoreItemDataViewModel(StoreItemData model)
        {
            this.model = model;
        }
    }
}
