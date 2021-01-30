//
//
//

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace YummyApp
{
    /// <summary>
    /// Interaction logic for RecipesCatalogPage.xaml
    /// Author Maria Leticia Leoncio Barbosa
    /// </summary>
 
    // page to show all categories
    public partial class CategoriesCatalogPage : Page
    {
        yummyDatabaseDataContext dc;
        List<MediaData> myCategories;
        List<Category> catTable;

        public CategoriesCatalogPage()
        {
            InitializeComponent();
            ShowCategories();
        }

        // method overloaded to load the data from the categories table, and build an object to display inside the category carousel
        private void loadDataToDisplay(List<Category> tab)
        {
            dc = new yummyDatabaseDataContext();
            myCategories = new List<MediaData>();
            foreach (var categoryObj in tab)
            {
                MediaData cnt = new MediaData();
                if (categoryObj.CategoryImage != null)
                {
                    cnt.ImageData = cnt.ByteArrayToImage(categoryObj.CategoryImage.ToArray());
                }
                cnt.Id = categoryObj.CategoryId;
                cnt.Title = categoryObj.CategoryName;
                myCategories.Add(cnt);
            }
            CategoriesCarousel.ItemsSource = myCategories; // setting the carousel data
        }

        // method that load the data from the table and set the carousel to the retrieved data
        private void ShowCategories()
        {
            dc = new yummyDatabaseDataContext();
            var query = (from Cat in dc.Categories orderby Cat.CategoryName ascending select Cat);
            catTable = query.ToList();
            loadDataToDisplay(catTable);
        }

        // method called when the user click on the Search Category button
        private void SearchCategory_Click(object sender, RoutedEventArgs e)
        {
            var tab = (from C in dc.Categories where C.CategoryName.ToUpper().Contains(SearchCategoryInput.Text.ToUpper()) orderby C.CategoryName ascending select C);
            loadDataToDisplay(tab.ToList());
        }

        // method called when an item is selected from the Category Carousel
        private void InspectCategory(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CategoriesCarousel.SelectedItem != null)
            {
                int categoryId = (CategoriesCarousel.SelectedItem as MediaData).Id;
                Category category = dc.Categories.Where(cat => cat.CategoryId == categoryId).Single();
                CategoryForm cf = new CategoryForm(category);
                cf.Show();
            }
        }

        // add new category to the database
        private void AddNewCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ModifyCategory mc = new ModifyCategory();
                mc.modifyCategory.Content = "ADD";
                mc.modifyName.Focus();
                mc.LMlabel.Visibility = Visibility.Hidden;
                mc.LMdateLabel.Visibility = Visibility.Hidden;
                mc.LMtimeLabel.Visibility = Visibility.Hidden;
                mc.LMdate.Visibility = Visibility.Hidden;
                mc.LMtime.Visibility = Visibility.Hidden;
                mc.Show();
                refreshCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        }

        // refresh the data from the database and load the displayable data
        private void refreshCategories()
        {
            dc = new yummyDatabaseDataContext();
            CategoriesCarousel.ItemsSource = null;

            // selecting specific columns to display in the recipe datagrid
            var catTab = (from C in dc.Categories orderby C.CategoryName ascending select C);
            loadDataToDisplay(catTab.ToList());
        }

        // hide side menu
        private void ButtonMenuClose_Click(object sender, RoutedEventArgs e)
        {
            ButtonMenuOpen.Visibility = Visibility.Visible;
            ButtonMenuClose.Visibility = Visibility.Collapsed;
        }

        // show entire side menu
        private void ButtonMenuOpen_Click(object sender, RoutedEventArgs e)
        {
            ButtonMenuOpen.Visibility = Visibility.Collapsed;
            ButtonMenuClose.Visibility = Visibility.Visible;
        }

        // navigation to the catalog page
        private void CatalogButton_Selected(object sender, RoutedEventArgs e)
        {
            Catalog catalogPage = new Catalog(); // creates an instance of the Catalog page
            var parent = this.Parent as Window;
            parent.Content = catalogPage; // show the Catalog page
        }

        // navigation to all recipes page
        private void AllRecipesButton_Selected(object sender, RoutedEventArgs e)
        {
            extra extraPage = new extra(); // creates an instance of the All Recipes page
            var parent = this.Parent as Window;
            parent.Content = extraPage; // show the All Recipes page
        }

        // navigation to dashboard page
        private void DashboardButton_Selected(object sender, RoutedEventArgs e)
        {
            MainWindow dashboard = new MainWindow(); // creates an instance of the dashboard page
            dashboard.InitializeComponent();
            var parent = this.Parent as Window;
            parent.Content = dashboard.Content; // show the dashboard page
        }

        // navigation to all categories page
        private void AllCategoriesButton_Selected(object sender, RoutedEventArgs e)
        {
            CategoriesCatalogPage categoriesCatalogPage = new CategoriesCatalogPage(); // creates an instance of the All Categories page
            var parent = this.Parent as Window;
            parent.Content = categoriesCatalogPage; // show the All Categories page
        }

        private void Cart_Button_Click(object sender, RoutedEventArgs e)
        {
            Shopping_Cart SC = new Shopping_Cart();
            SC.ShowDialog();
        }
    }
}
