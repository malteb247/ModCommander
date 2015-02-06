using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModCommander.View
{
    /// <summary>
    /// Interaction logic for ShopItemView.xaml
    /// </summary>
    public partial class StoreItemListView : UserControl
    {
        // Dependency Property
        public static readonly DependencyProperty StoreItemsProperty = DependencyProperty.Register(
            "StoreItems",
            typeof(ICollection<StoreItemDataViewModel>),
            typeof(StoreItemListView),
            null);

        // .NET Property wrapper
        public ICollection<StoreItemDataViewModel> StoreItems
        {
            get { return (ICollection<StoreItemDataViewModel>)GetValue(StoreItemsProperty); }
            set { SetValue(StoreItemsProperty, value); }
        }

        // Dependency Property
        //public static readonly DependencyProperty SelectedModProperty = DependencyProperty.Register(
        //    "SelectedMod",
        //    typeof(StoreItemDataViewModel),
        //    typeof(StoreItemListView),
        //    null);

        // .NET Property wrapper
        //public ModViewModel SelectedMod
        //{
        //    get { return (ModViewModel)GetValue(SelectedModProperty); }
        //    set { SetValue(SelectedModProperty, value); }
        //}

        public StoreItemListView()
        {
            InitializeComponent();
        }
    }
}
