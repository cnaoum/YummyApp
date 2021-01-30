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
    /// Interaction logic for CategoryForm.xaml
    /// </summary>
    /// author of the code : Rajwinder Kaur
    /// About the code : display details about particular category.
    ///                  Show its image name and Recipes under that particular Category.
    public partial class CategoryForm : Window
    {
        yummyDatabaseDataContext dc = new yummyDatabaseDataContext();
        BitmapImage image; //for image
        Category category = new Category();

        //Pass Category object in constructor to load details of selected category
        public CategoryForm(Category categoryObj)
        {
            InitializeComponent();
            if (categoryObj != null)
                loadCategoryForm(categoryObj);
        }

        //load catageory 
        private void loadCategoryForm(Category categoryObj)
        {
            try
            {
                categoryName.Text = categoryObj.CategoryName;
                //store categoryId to get at edit time
                category.CategoryId = categoryObj.CategoryId;
                if (categoryObj.CategoryImage != null)
                {  // to load image
                    image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream(categoryObj.CategoryImage.ToArray());
                    image.EndInit();
                    categoryImg.Source = image;
                }

                //Hide grid and Total Recipe label if there is no Recipe in particular category
                recipesUnderCg.Visibility = Visibility.Hidden;
                    recipeDataGrid.Visibility = Visibility.Hidden;

                // get Recipes in paticilar category
                var totalRecipes = (from R in dc.Recipes where R.Category == categoryObj.CategoryId select R).Count();
                if (totalRecipes != 0)
                {
                    // Show repeipe label and grid if recipe exist in particualr category
                    recipesUnderCg.Visibility = Visibility.Visible;
                    recipeDataGrid.Visibility = Visibility.Visible;

                    //get recipes of category in Ascending order
                    recipeDataGrid.ItemsSource = from R in dc.Recipes where R.Category == categoryObj.CategoryId orderby R.Name select new { Receipe = R.Name };
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        
        }

        //handle edit operation(categoryId present in Editing mode)
        private void editCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (category.CategoryId != 0)
                {
                    //pass select category id for editing in constructor 
                    ModifyCategory mc = new ModifyCategory(category.CategoryId);
                    mc.Show();
                    this.Close();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        }

        //handle add operation(No CategoryId in Adding mode)
        public void addCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ModifyCategory mc = new ModifyCategory();
                mc.modifyCategory.Content = "ADD";
                mc.modifyName.Focus();
                //Hide Date Labels on Adding Category
                mc.LMlabel.Visibility = Visibility.Hidden;
                mc.LMdateLabel.Visibility = Visibility.Hidden;
                mc.LMtimeLabel.Visibility = Visibility.Hidden;
                mc.LMdate.Visibility = Visibility.Hidden;
                mc.LMtime.Visibility = Visibility.Hidden;
                mc.Show();
                this.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        }

        //Close current form and redirect to Catalog form
        private void closeCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Catalog ct = new Catalog();
                this.Content = ct;
                this.Close();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        }
    }
}
