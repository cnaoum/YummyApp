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

    // page to show all recipes
    public partial class RecipesCatalogPage : Page
    {
        yummyDatabaseDataContext dc;
        List<MediaData> myRecipes;
        List<Recipe> recTable;

        public RecipesCatalogPage()
        {
            InitializeComponent();
            ShowRecipes();
        }

        // method overloaded to load the data from the categories table, and build an object to display inside the category carousel
        private void loadDataToDisplay(List<Recipe> tab)
        {
            dc = new yummyDatabaseDataContext();
            myRecipes = new List<MediaData>();
            foreach (var recipeObj in tab)
            {
                MediaData cnt = new MediaData();
                if (recipeObj.Image != null)
                {
                    cnt.ImageData = cnt.ByteArrayToImage(recipeObj.Image.ToArray());
                }
                cnt.Id = recipeObj.RecipeId;
                cnt.Title = recipeObj.Name;
                cnt.Description = recipeObj.Description;
                myRecipes.Add(cnt);
            }
            RecipesCarousel.ItemsSource = myRecipes;
        }

        // method that load the data from the table and set the carousel to the retrieved data
        private void ShowRecipes()
        {
            dc = new yummyDatabaseDataContext();
            var query = (from Rec in dc.Recipes orderby Rec.Name ascending select Rec);
            recTable = query.ToList();
            loadDataToDisplay(recTable);
        }

        // method called when the user click on the Search Category button
        private void SearchRecipe_Click(object sender, RoutedEventArgs e)
        {
            var tab = (from R in dc.Recipes where R.Name.ToUpper().Contains(SearchRecipeInput.Text.ToUpper()) orderby R.Name ascending select R);

            loadDataToDisplay(tab.ToList());
        }

        // method called when an item is selected from the Recipe Carousel
        private void ViewRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesCarousel.SelectedItem != null)
            {
                PrintRecipe printRecipe = new PrintRecipe((RecipesCarousel.SelectedItem as MediaData).Id);
                printRecipe.ShowDialog();
            }
        }

        // add new category to the database
        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            AddRecipe addRecipe = new AddRecipe();
            addRecipe.ShowDialog();
            refreshRecipies();
        }

        // method called when an item is selected from the Recipe Carousel and the user try to edit this item
        private void EditRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesCarousel.SelectedItem != null)
            {
                AddRecipe addRecipe = new AddRecipe((RecipesCarousel.SelectedItem as MediaData).Id);
                addRecipe.labelNewRecipe.Content = "Edit Recipe";
                addRecipe.Title = "Edit Recipe";
                addRecipe.ShowDialog();
                refreshRecipies();
            }
            else
            {
                MessageBox.Show("Please select a recipe to update.", "Update Recipe");
            }
        }

        // method called when an item is selected from the Recipe Carousel and the user try to delete this item
        private void RemoveRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesCarousel.SelectedItem != null)
            {
                int recipeId = (RecipesCarousel.SelectedItem as MediaData).Id;
                string recipeName = (RecipesCarousel.SelectedItem as MediaData).Title;

                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete recipe '{recipeName}' ?", "Delete Recipe", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    var recipe = dc.Recipes.Single(r => r.RecipeId == recipeId);
                    dc.RecipeIngredients.DeleteAllOnSubmit(recipe.RecipeIngredients);
                    dc.Recipes.DeleteOnSubmit(recipe);
                    dc.SubmitChanges();
                    refreshRecipies();
                }
            }
            else
            {
                MessageBox.Show("Please select a recipe to delete.", "Delete Recipe");
            }
        }

        // refresh the data from the database and load the displayable data
        private void refreshRecipies()
        {
            dc = new yummyDatabaseDataContext();
            RecipesCarousel.ItemsSource = null;

            // selecting specific columns to display in the recipe datagrid
            var recTab = (from R in dc.Recipes orderby R.Name ascending select R);
            loadDataToDisplay(recTab.ToList());
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
