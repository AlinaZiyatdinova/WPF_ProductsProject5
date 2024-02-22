using Microsoft.EntityFrameworkCore;
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
using WPF5.Models;

namespace WPF5.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        int allRecords = 0;
        public ProductsWindow(bool isAdmin)
        {
            InitializeComponent();
            GetAllRecords();
            Collaboration();
            var context = new Fabric2Context();
            productsListView.ItemsSource = context
                .Products
                .Include(p => p.ProductManufacturer) 
                .ToList();

            productsListView.ItemsSource = LoadData();


            //фильтрация по производителю (добавление в комбобокс)
            var manufacturers = context.ProductManufactures.ToList();
            manufacturers.Insert(0, new ProductManufacture { ManufacturerId = 0, ManufacturerName = "Все производители" });
            filterComboBox.ItemsSource = manufacturers;

            var category = context.ProductCategories.ToList();
            category.Insert(0, new ProductCategory { CategriesId = 0, CategoriesName = "Все категории" });
            filterCategoriesComboBox.ItemsSource = category;

            sortingComboBox.SelectedIndex = 0;
            filterComboBox.SelectedIndex = 0;
            filterCategoriesComboBox.SelectedIndex = 0;

            if (!isAdmin)
            {
                deleteButton.Visibility = Visibility.Hidden;
                updateButton.Visibility = Visibility.Hidden;
                addButton.Visibility = Visibility.Hidden;
            }
        }
        private List<Product> LoadData()
        {
            using (var context = new Fabric2Context())
            {
                var productList = context
                    .Products
                    .Include(p => p.ProductManufacturer)
                    .ToList();

                return productList;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e) => Collaboration();

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Collaboration();

        private void Collaboration()
        {
            using (var context = new Fabric2Context())
            {
                var query = context.Products.Include(p => p.ProductManufacturer).Include(p => p.ProductCategory).AsQueryable();

                var selectedManufacturer = (ProductManufacture)filterComboBox.SelectedItem;
                if (selectedManufacturer != null && selectedManufacturer.ManufacturerId != 0)
                {
                    query = query.Where(p => p.ProductManufacturerId == selectedManufacturer.ManufacturerId);
                }

                var selectedCategories = (ProductCategory)filterCategoriesComboBox.SelectedItem;
                if (selectedCategories != null && selectedCategories.CategriesId != 0)
                {
                    query = query.Where(p => p.ProductCategoryId == selectedCategories.CategriesId);
                }

                var searchQuery = searchTextBox.Text;
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query = query.Where(p => p.ProductName.Contains(searchQuery));
                }

                switch (sortingComboBox.SelectedIndex)
                {
                    case 1: // По возрастанию цены
                        query = query.OrderBy(p => p.ProductCost);
                        break;
                    case 2: // По убыванию цены
                        query = query.OrderByDescending(p => p.ProductCost);
                        break;
                }

                var filteredList = query.ToList();
                productsListView.ItemsSource = filteredList;

                recordsTextBlock.Text = $"Показано {filteredList.Count} из {allRecords} записей";
            }
        }
        private void GetAllRecords()
        {
            using (var context = new Fabric2Context())
            {
                allRecords = context.Products.Count();
            }
        }
        private void sortingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Collaboration();

        private void button_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow page = new AddProductWindow();
            page.ShowDialog();
            GetAllRecords();
            Collaboration();
        }

        private void filterCategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Collaboration();

        private void productsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool hasSelectedItems = productsListView.SelectedItems.Count > 0;
            updateButton.IsEnabled = hasSelectedItems;
            deleteButton.IsEnabled = hasSelectedItems;
        }
       

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Product? selectedProduct = productsListView.SelectedItem as Product;
            if (selectedProduct is not null)
            {
                var window = new AddProductWindow(selectedProduct,true);
                window.ShowDialog();
                Collaboration();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Product? selectedProduct = productsListView.SelectedItem as Product;
            if (selectedProduct != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить продукт?", "Удаление продукта", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new Fabric2Context())
                    {
                        var productDelete = context.Products.FirstOrDefault(p => p.ProductArticleNumber == selectedProduct.ProductArticleNumber);
                        if (productDelete != null)
                        {
                            string productArticle = selectedProduct.ProductArticleNumber;
                            var sorted = context.OrderProducts.Include(p => p.Order).ToList();
                            if (sorted.Any(p => p.ProductArticleNumber == productArticle && p.Order.OrderStatusId == 1))
                            {
                                MessageBox.Show("Нельзя удалить продукт ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            context.Products.Remove(productDelete);
                            context.SaveChanges();
                        }
                    }
                    GetAllRecords();
                    Collaboration();
                }
            }
        }
    }
}
