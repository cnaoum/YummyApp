using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ModifyCategory.xaml
    /// </summary>
    ///  author of the code : Rajwinder Kaur
    /// About the code : display details about particular category for editing.
    ///                  For Adding Category also.
    public partial class ModifyCategory : Window
    {
        yummyDatabaseDataContext dc = new yummyDatabaseDataContext();
        BitmapImage image; //for image
        Category categoryObj = new Category();

        //CategoryId pass in case of editing
        public ModifyCategory(int? categoryId = null)
        {
            InitializeComponent();
            if (categoryId != null) {
                loadCategoryDetails(categoryId);
            }
        }

        //Load Category details on edit 
        private void loadCategoryDetails(int? categoryId)
        {
            try
            {
                modifyName.Focus();
                //based on Id get the details from Category table
                categoryObj = dc.Categories.Where(categoryObj => categoryObj.CategoryId == categoryId).Single(); ;
                modifyName.Text = categoryObj.CategoryName;

                //get last modified category date and using array spilt date and time
                string categoryDate = categoryObj.ModifyDate.ToString();
                if (categoryDate != "")
                {
                    string[] spiltDate = categoryDate.Split(' ');
                    LMdate.Text = spiltDate[0];
                    LMtime.Text = spiltDate[1] + spiltDate[2];
                    modifyCategory.Content = "Update";

                }

                //load the image from table
                if (categoryObj.CategoryImage != null)
                {
                    image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = new MemoryStream(categoryObj.CategoryImage.ToArray());
                    image.EndInit();
                    modifyImage.Source = image;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        }

        //Adding Image
        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Select a Category photo";
                //apply a filter, only jpg,jpeg,png file going to show
                openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Portable Network Graphic (*.png)|*.png";
                if ((bool)openFileDialog.ShowDialog())
                {
                    image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = File.OpenRead(openFileDialog.FileName);
                    image.EndInit();
                    modifyImage.Source = image;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        }

        //For deleting Image
        private void deleteImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                modifyImage.Source = null;
                image = null;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        }

        //on click of submit button
        private void modifyCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //check if category textbox is empty
                if (string.IsNullOrEmpty(modifyName.Text))
                {
                    modifyName.Text = "Category name is required";
                    modifyName.Foreground = Brushes.Red;

                }
                else
                {
                    saveRecord(); //function call for assign coming values to caegory object field

                    //Upadte Record if category Id exists
                    if (categoryObj.CategoryId != 0)
                    {
                        //get category name on that edit Id
                        string Cname = (from C in dc.Categories where C.CategoryId == categoryObj.CategoryId select C.CategoryName).Single(); 
                        // Check if Category name is changed
                        if(modifyName.Text.ToUpper() != Cname)
                        {
                            var category_exist = (from C in dc.Categories where C.CategoryName == modifyName.Text.ToUpper() select C).Count();
                            //if no record exist with enter name then upadte operation going to perform with image and name
                            if (category_exist == 0)
                            {
                                dc.SubmitChanges();
                                MessageBox.Show("Category Record is modified");
                            }
                        } else
                        { //if name exist with enter name then image going to update
                            dc.SubmitChanges();
                            MessageBox.Show("Category Record is modified");
                        }      

                    }
                    else
                    {
                        //For Adding new category
                        var category_exist = (from C in dc.Categories where C.CategoryName == modifyName.Text.ToUpper() select C).Count();
                        if (category_exist == 1)
                        {
                            modifyName.Text = "Category name is alredy exist";
                            modifyName.Foreground = Brushes.Red;
                        } else {
                            dc.Categories.InsertOnSubmit(categoryObj);
                            dc.SubmitChanges();
                            MessageBox.Show("New Category Record " + modifyName.Text.ToUpper() + " Created");
                        }
                        ////after addition and updation category form will open with latest entry
                        //CategoryForm cf = new CategoryForm(categoryObj);
                        //cf.Show();
                        //this.Close();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        }

        //for store incomming image,category text name, Modify Date into category object
        private void saveRecord()
        {
            try
            {
                categoryObj.CategoryName = modifyName.Text.ToUpper();
                categoryObj.ModifyDate = DateTime.Now;
                if (image != null)
                {
                    // get image byte array and store in table
                    byte[] imageLength = new byte[image.StreamSource.Length];
                    image.StreamSource.Seek(0, SeekOrigin.Begin);
                    image.StreamSource.Read(imageLength, 0, imageLength.Length);
                    categoryObj.CategoryImage = imageLength;
                }
                else
                {
                    //store null in image if no image selected
                    categoryObj.CategoryImage = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }
        }

        //change color from red to black in textbox after shown errors
        private void categoryTextChanged(object sender, TextChangedEventArgs e)
        {
            modifyName.Foreground = Brushes.Black;       
        }

        //to close form and open catalog form
        private void closeModifyForm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " error");
            }

        }
    }
}
