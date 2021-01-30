using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace YummyApp
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        //this constructor was created to set the DataDirectoryto ignore the bin\debug folders when saving to database using connection strings
        public App()
        {
            var solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));
            AppDomain.CurrentDomain.SetData("DataDirectory", solutionDirectory);
        }
    }
}
