using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for PrintRecipe.xaml
    /// </summary>
    /// 
    //Carolina Naoum Junqueira
    public partial class PrintRecipe : Window
    {
        yummyDatabaseDataContext dc;
        Recipe recipe;
        BitmapImage bitmapImage;

        //This method id used to load the recipe on the window
        public PrintRecipe(int recipeId)
        {
            InitializeComponent();
            //loades recipe when window opens
            loadRecipe(recipeId);
        }

        //Opens the print dialog and allows user to print the recipe
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog myPrintDialog = new PrintDialog();
            if (myPrintDialog.ShowDialog() == true)
            {
                myPrintDialog.PrintVisual(this, recipe.Name);
            }
        }

        //opens the edit recipe window and allows user to edit the recipe before printing

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            AddRecipe addRecipe = new AddRecipe(recipe.RecipeId);
            addRecipe.labelNewRecipe.Content = "Edit Recipe";
            addRecipe.Title = "Edit Recipe";
            var result = addRecipe.ShowDialog();
            //if user edits recipe the load recipe method is called so that the recipe will be updated in display window
            loadRecipe(recipe.RecipeId);        
        }

        //this method is used to load the recipe to the page by using its id
        private void loadRecipe(int recipeId)
        {
            dc = new yummyDatabaseDataContext();
            recipe = dc.Recipes.Where(recipe => recipe.RecipeId == recipeId).Single();

            if (recipe.Image != null)
            {
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(recipe.Image.ToArray());
                bitmapImage.EndInit();
                imgRecipePhoto.Source = bitmapImage;
            }
            labelRecipeName.Content = recipe.Name;
            txtRecipeDescription.Text = recipe.Description;
            txtRecipePrepTime.Text = recipe.PrepTime.ToString();
            txtRecipeServings.Text = recipe.Serving.ToString();
            txtRecipeDirections.Text = recipe.Directions;

            string recipeIngredients = string.Empty;
            foreach (var recipeIngredient in recipe.RecipeIngredients)
                recipeIngredients += $"{recipeIngredient.Quantity} {recipeIngredient.Measurement} {recipeIngredient.Ingredient.Name}\n";

            txtRecipeIngrediensList.Text = recipeIngredients;
        }
    }
}
