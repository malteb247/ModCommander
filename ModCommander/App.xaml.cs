using ModCommander.Data;
using ModCommander.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ModCommander
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string SelectedLanguage = "de";
        public static string FallbackLanguage = "en";

        public static ProfileViewModel CurrentProfile { get; set; }


        protected override void OnStartup(StartupEventArgs e)
        {
        }
    }
}
