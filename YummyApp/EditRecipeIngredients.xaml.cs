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
    /// Interaction logic for EditRecipeIngredients.xaml
    /// </summary>
    /// 
    //Carolina Naoum Junqueira
    public partial class EditRecipeIngredients : Window
    {
        RecipeIngredient _recipeIngredient;
        public EditRecipeIngredients(RecipeIngredient recipeIngredient)
        {
            InitializeComponent();
            _recipeIngredient = recipeIngredient;

            txtRecipeIngredientQuantity.Text = _recipeIngredient.Quantity.ToString();
            cbRecipeIngredientMeasurement.Text = _recipeIngredient.Measurement;
            txtRecipeIngredientIngredient.Text = _recipeIngredient.Ingredient.Name;
        }

        private void btnEditRecipeIngredientSave_Click(object sender, RoutedEventArgs e)
        {
            _recipeIngredient.Quantity = Convert.ToDouble(txtRecipeIngredientQuantity.Text);
            _recipeIngredient.Measurement = cbRecipeIngredientMeasurement.Text;
            _recipeIngredient.Ingredient.Name = txtRecipeIngredientIngredient.Text;
            Close();
        }

        private void btnEditRecipeIngredientCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
