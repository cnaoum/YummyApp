// Author Maria Leticia Leoncio Barbosa

using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace YummyApp
{
    /// <summary>
    /// Interaction logic for Catalog.xaml
    /// Author Maria Leticia Leoncio Barbosa
    /// </summary>
    
    // catalog page that shows a small part from the list of categories and recipes
    public partial class Catalog : Page
    {
        yummyDatabaseDataContext dc;
        List<MediaData> myCategories;
        List<MediaData> myRecipes;
        List<Category> catTable;
        List<Recipe> recTable;

        public Catalog()
        {
            InitializeComponent();

            // instatiating the database connection
            dc = new yummyDatabaseDataContext();

            // load the data to the carousel
            ShowCategories();
            ShowRecipes();
        }

        // method overloaded to load the data from the recipes table, and build an object to display inside the recipe carousel
        private void loadDataToDisplay(List<Recipe> tab)
        {
            dc = new yummyDatabaseDataContext();
            myRecipes = new List<MediaData>();

            // for each recipe inside the table build a Media data instance
            foreach (var recipeObj in tab)
            {
                MediaData cnt = new MediaData();
                if (recipeObj.Image != null)
                {
                    // getting the array of bytes and converting to a displayable image
                    cnt.ImageData = cnt.ByteArrayToImage(recipeObj.Image.ToArray());
                }
                cnt.Id = recipeObj.RecipeId;
                cnt.Title = recipeObj.Name;
                cnt.Description = recipeObj.Description;

                // add instance of MediaData to the list that will be used inside de carousel
                myRecipes.Add(cnt);
            }

            RecipesCarousel.ItemsSource = myRecipes; // setting the carousel data
        }

        // method overloaded to load the data from the categories table, and build an object to display inside the category carousel
        private void loadDataToDisplay(List<Category> tab)
        {
            dc = new yummyDatabaseDataContext();
            myCategories = new List<MediaData>();

            // for each category inside the table build a Media data instance
            foreach (var categoryObj in tab)
            {
                MediaData cnt = new MediaData();
                if (categoryObj.CategoryImage != null)
                {
                    // getting the array of bytes and converting to a displayable image
                    cnt.ImageData = cnt.ByteArrayToImage(categoryObj.CategoryImage.ToArray());
                }
                cnt.Id = categoryObj.CategoryId;
                cnt.Title = categoryObj.CategoryName;

                // add instance of MediaData to the list that will be used inside de carousel
                myCategories.Add(cnt);
            }
            CategoriesCarousel.ItemsSource = myCategories; // setting the carousel data
        }

        // method that load the data from the table and set the carousel to the retrieved data
        private void ShowCategories()
        {
            dc = new yummyDatabaseDataContext();
            // query that retrieves only the first 5 elements from the Category table ordered by the Category Name
            var query = (from Cat in dc.Categories orderby Cat.CategoryName ascending select Cat).Take(5);
            catTable = query.ToList();
            loadDataToDisplay(catTable); // loading the data in a proper format to display
        }

        // method that load the data from the table and set the carousel to the retrieved data
        private void ShowRecipes()
        {
            dc = new yummyDatabaseDataContext();
            // query that retrieves only the first 4 elements from the Recipe table ordered by the Name
            var query = (from Rec in dc.Recipes orderby Rec.Name ascending select Rec).Take(4);
            recTable = query.ToList();
            loadDataToDisplay(recTable); // loading the data in a proper format to display
        }

        // method called when an item is selected from the Recipe Carousel
        private void InspectRecipe(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (RecipesCarousel.SelectedItem != null) // checking if there is a selected item
            {
                // creates a new instance of the PrintRecipe page
                PrintRecipe printRecipe = new PrintRecipe((RecipesCarousel.SelectedItem as MediaData).Id);
                printRecipe.ShowDialog(); // show the Print Recipe with all the data about the recipe
                refreshRecipies();
            }
        }

        // method called when an item is selected from the Category Carousel
        private void InspectCategory(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CategoriesCarousel.SelectedItem != null) // checking if there is a selected item
            {
                int categoryId = (CategoriesCarousel.SelectedItem as MediaData).Id; // getting the select item id
                Category category = dc.Categories.Where(cat => cat.CategoryId == categoryId).Single(); // retrieving the selected category data from the Category table
                CategoryForm cf = new CategoryForm(category); // / creates a new instance of the CategoryForm page
                cf.Show(); // show the Category form filled with the data of the selected category
                refreshCategories();
            }
        }

        // method called when the user click on the Search Category button
        private void SearchCategory_Click(object sender, RoutedEventArgs e)
        {
            // query that checks if there is any category that contains inside their name the text inserted on the search input, and get the first 5
            var tab = (from C in dc.Categories where C.CategoryName.ToUpper().Contains(SearchCategoryInput.Text.ToUpper()) orderby C.CategoryName ascending select C).Take(5);

            loadDataToDisplay(tab.ToList()); // load all the filtered data
        }

        // method called when the user click on the Search Recipe button
        private void SearchRecipe_Click(object sender, RoutedEventArgs e)
        {
            // query that checks if there is any recipe that contains inside their name the text inserted on the search input, and get the first 4
            var tab = (from R in dc.Recipes where R.Name.ToUpper().Contains(SearchRecipeInput.Text.ToUpper()) orderby R.Name ascending select R).Take(4);

            loadDataToDisplay(tab.ToList()); // load all the filtered data
        }

        // method called when the user click on the "See More" link after Category Carousel
        private void SeeMoreRecipesClick(object sender, MouseButtonEventArgs e)
        {
            RecipesCatalogPage recipesCatalogPage = new RecipesCatalogPage(); // creates an instance of the Recipes list page
            var parent = this.Parent as Window; 
            parent.Content = recipesCatalogPage; // show the Catalog of Recipes page
        }

        // method called when the user click on the "See More" link after Recipe Carousel
        private void SeeMoreCategoriesClick(object sender, MouseButtonEventArgs e)
        {
            CategoriesCatalogPage categoriesCatalogPage = new CategoriesCatalogPage(); // creates an instance of the Category list page
            var parent = this.Parent as Window;
            parent.Content = categoriesCatalogPage; // show the Catalog of Categories page
        }

        // method to refresh the data of the recipe carousel
        private void refreshRecipies()
        {
            dc = new yummyDatabaseDataContext();
            RecipesCarousel.ItemsSource = null;

            // selecting specific columns to display in the recipe datagrid
            var recTab = (from R in dc.Recipes orderby R.Name ascending select R).Take(4);
            loadDataToDisplay(recTab.ToList());
        }

        // method to refresh the data of the category carousel
        private void refreshCategories()
        {
            dc = new yummyDatabaseDataContext();
            CategoriesCarousel.ItemsSource = null;

            // selecting specific columns to display in the recipe datagrid
            var catTab = (from C in dc.Categories orderby C.CategoryName ascending select C).Take(5);
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
