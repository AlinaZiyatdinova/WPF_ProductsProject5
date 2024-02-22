using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using WPF5.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WPF5.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private Product currentProduct;
        private bool isAdd = false;
        private string originalPhotoString = null;
        public AddProductWindow()
        {
            InitializeComponent();
            currentProduct = new Product();
            DataContext = currentProduct;
            currentProduct.ProductPhoto = "нет";
            addUpdateButton.Content = "Добавить";
            productNameTextBox.Focus();

        }
        public AddProductWindow(Product product, bool update)
        {
            InitializeComponent();
            currentProduct = product;
            this.isAdd = update;
            addUpdateButton.Content = "Изменить";
            productNameTextBox.Focus();
            DataContext = currentProduct;
        }
        private void loadWindow(object sender, RoutedEventArgs e)
        {
            using (var context = new Fabric2Context())
            {
                var category = context.ProductCategories.ToList();
                categoriesComboBox.DisplayMemberPath = "CategoriesName";
                categoriesComboBox.ItemsSource = category;
                providerComboBox.ItemsSource = context.ProductProviders.ToList();
                providerComboBox.DisplayMemberPath = "ProviderName";
                manufacturerComboBox.ItemsSource = context.ProductManufactures.ToList();
                manufacturerComboBox.DisplayMemberPath = "ManufacturerName";

            
                categoriesComboBox.SelectedItem = context.ProductCategories.FirstOrDefault(c => c.CategriesId == currentProduct.ProductCategoryId);
                providerComboBox.SelectedItem = context.ProductProviders.FirstOrDefault(p => p.ProviderId == currentProduct.ProductProviderId);
                manufacturerComboBox.SelectedItem = context.ProductManufactures.FirstOrDefault(m => m.ManufacturerId == currentProduct.ProductManufacturerId);
            }
        }

        private void updatePhoto(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Изображения (*.png;*.jpg)|*.png;*.jpg",
                Title = "Выбор изображения"
            };
            if (dialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(originalPhotoString) && originalPhotoString != "")
                {
                    var oldImagePath = System.IO.Path.Combine(Environment.CurrentDirectory, "Images", originalPhotoString);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }
                var newPhotoName = System.IO.Path.GetRandomFileName() + ".png";
                var selectedPhoto = dialog.FileName;
                var newPhotoFullName = System.IO.Path.Combine(Environment.CurrentDirectory, "Images", newPhotoName);

                if (!IsImageSizeValid(selectedPhoto))
                {
                    MessageBox.Show("Пожалуйста, выберите другое изображение.");
                    return;
                }

                File.Copy(selectedPhoto, newPhotoFullName);
                currentProduct.ProductPhoto = newPhotoName;

                DataContext = null;
                DataContext = currentProduct;
            }
        }
        private bool IsImageSizeValid(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length <= 2097152;
        }
        private void addUpdateProduct(object sender, RoutedEventArgs e)
        {
            // создаем контекст валидации
            var validationContext = new ValidationContext(currentProduct);
            // список ошибок
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            // вызываем метод класс Validator - TryValidateObject
            // true для полной проверки
            var isValid = Validator.TryValidateObject(currentProduct, validationContext, results, true);

            // если объект не валидный
            if (!isValid)
            {
                errorsLabel.Content = string.Empty;

                foreach (var error in results)
                {
                    errorsLabel.Content += error.ErrorMessage + "\r\n";
                }
                return;
            }

            using (var context = new Fabric2Context())
            {
                if (isAdd) // изменение
                {
                    context.Products.Update(currentProduct);
                }
                else
                {
                    context.Entry(currentProduct).State = EntityState.Added;
                }
                context.SaveChanges();
            }

            originalPhotoString = currentProduct.ProductPhoto;
            Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
