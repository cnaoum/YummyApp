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
using System.Windows.Shapes;

namespace YummyApp
{
    /// <summary>
    /// Interaction logic for Shopping_Cart.xaml
    /// </summary>
    public partial class Shopping_Cart : Window
    {
        yummyDatabaseDataContext dc; 
        public Shopping_Cart()
        {
            InitializeComponent();
            dc = new yummyDatabaseDataContext();
            ShopingLIstGrid.ItemsSource = null;

            // selecting specific columns to display in the recipe datagrid
            ShopingLIstGrid.ItemsSource = dc.Recipes.Select(recipe => new { recipe.RecipeId, recipe.Name, recipe.PrepTime, recipe.Serving, Category = recipe.Category1.CategoryName });
        }

       
        private void Generate_Shopping_List(object sender, RoutedEventArgs e)
        {
            if (ShopingLIstGrid.SelectedItem != null)
            {
                ShoppingList SL = new ShoppingList((ShopingLIstGrid.SelectedItem as dynamic).RecipeId);
                SL.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a recipe to get the shoping List.");
            }
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

 private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            this.Content = new Catalog();
        }

        private void ListViewItem_Selected_1(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
