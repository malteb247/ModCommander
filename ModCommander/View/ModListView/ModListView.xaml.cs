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
    /// Interaction logic for ModListView.xaml
    /// </summary>
    public partial class ModListView : UserControl
    {
        // Dependency Property
        public static readonly DependencyProperty ModsProperty = DependencyProperty.Register(
            "Mods",
            typeof(IEnumerable<ModViewModel>),
            typeof(ModListView), 
            null);

        // .NET Property wrapper
        public IEnumerable<ModViewModel> Mods
        {
            get { return (IEnumerable<ModViewModel>)GetValue(ModsProperty); }
            set { SetValue(ModsProperty, value); }
        }

        // Dependency Property
        public static readonly DependencyProperty SelectedModProperty = DependencyProperty.Register(
            "SelectedMod",
            typeof(ModViewModel),
            typeof(ModListView),
            null);

        // .NET Property wrapper
        public ModViewModel SelectedMod
        {
            get { return (ModViewModel)GetValue(SelectedModProperty); }
            set { SetValue(SelectedModProperty, value); }
        }
                
        // Dependency Property
        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
            "SearchText",
            typeof(string),
            typeof(ModListView),
            null);

        // .NET Property wrapper
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }


        public ModListView()
        {
            InitializeComponent();            
        }
    }
}
