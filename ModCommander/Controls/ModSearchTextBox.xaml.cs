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

namespace ModCommander.Controls
{
    /// <summary>
    /// Interaction logic for ModSearchTextBox.xaml
    /// </summary>
    public partial class ModSearchTextBox : UserControl
    {
        public static DependencyProperty SearchTextProperty =
           DependencyProperty.Register("SearchText", typeof(string), typeof(ModSearchTextBox));

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static DependencyProperty IsNameCheckedProperty =
          DependencyProperty.Register("IsNameChecked", typeof(bool), typeof(ModSearchTextBox));

        public bool IsNameChecked
        {
            get { return (bool)GetValue(IsNameCheckedProperty); }
            set { SetValue(IsNameCheckedProperty, value); }
        }

        public static DependencyProperty IsDescriptionCheckedProperty =
         DependencyProperty.Register("IsDescriptionChecked", typeof(bool), typeof(ModSearchTextBox));

        public bool IsDescriptionChecked
        {
            get { return (bool)GetValue(IsDescriptionCheckedProperty); }
            set { SetValue(IsDescriptionCheckedProperty, value); }
        }

        public static DependencyProperty IsShopItemNameCheckedProperty =
         DependencyProperty.Register("IsShopItemNameChecked", typeof(bool), typeof(ModSearchTextBox));

        public bool IsShopItemNameChecked
        {
            get { return (bool)GetValue(IsShopItemNameCheckedProperty); }
            set { SetValue(IsShopItemNameCheckedProperty, value); }
        }

        public static DependencyProperty IsShopItemDescriptionCheckedProperty =
         DependencyProperty.Register("IsShopItemDescriptionChecked", typeof(bool), typeof(ModSearchTextBox));

        public bool IsShopItemDescriptionChecked
        {
            get { return (bool)GetValue(IsShopItemDescriptionCheckedProperty); }
            set { SetValue(IsShopItemDescriptionCheckedProperty, value); }
        }

        public ModSearchTextBox()
        {
            InitializeComponent();
        }
    }
}
