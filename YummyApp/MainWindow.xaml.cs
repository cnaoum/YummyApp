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
using System.Data.Linq;

namespace YummyApp
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        Shopping_Cart SC = new Shopping_Cart();
        //public MainWindow()
        //{
        //    InitializeComponent();
        //}

        //public MainWindow()
        //{
        //    InitializeComponent();
        //}

        //private void GoToMyCatalogPage(object sender, RoutedEventArgs e)
        //{
        //    Catalog catalogWindow = new Catalog();
        //    this.Content = catalogWindow;
        //}

        //private void GoToMyCategoryPage(object sender, RoutedEventArgs e)
        //{
        //    CategoryPage categoryPage = new CategoryPage();
        //    categoryPage.Show();
        //}
        //private void GoToMyRecipePage(object sender, RoutedEventArgs e)
        //{
        //    extra recipePage = new extra();
        //    recipePage.Show();
        //}


        private void ButtonLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonMenuClose_Click(object sender, RoutedEventArgs e)
        {
            ButtonMenuOpen.Visibility = Visibility.Visible;
            ButtonMenuClose.Visibility = Visibility.Collapsed;
        }

        private void ButtonMenuOpen_Click(object sender, RoutedEventArgs e)
        {
            ButtonMenuOpen.Visibility = Visibility.Collapsed;
            ButtonMenuClose.Visibility = Visibility.Visible;
        }

        private void Image_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            this.Content = new Catalog();
        }

        private void ListViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            this.Content = new extra();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cart_Button_Click(object sender, RoutedEventArgs e)
        {
            SC.ShowDialog();
        }
    }
}
