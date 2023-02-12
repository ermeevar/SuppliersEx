using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SupplierManager.Database;
using SupplierManager.Utils;

namespace SupplierManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Supplier> DataSuppliers;
        public static LopushokEntities Context = new LopushokEntities();
        private int page = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            DataSuppliers = Context.Suppliers.ToList();
            DateStartSorter.SelectedIndex = 0;
            //MigratePictiresExecuter.Migrate();

            Filter();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (page < (int)Math.Round((double)Context.Suppliers.Count() / 20) - 1)
                page++;

            Filter();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (page > 0)
                page--;

            Filter();
        }

        private void DateStartSorter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void NameSorter_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Filter();
        }

        private void NameSorter_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Filter();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            page = Convert.ToInt32((sender as Button).Content) - 1;
            
            Filter();
        }

        /// <summary>
        /// Метод для фильтрации и пагинации в комбинациях
        /// </summary>
        private void Filter()
        {
            // Спеова все фильтруем по каждому объекту-фильтру/ сортировки
            if (!string.IsNullOrWhiteSpace(NameSorter.Text))
            {
                DataSuppliers = Context.Suppliers
                    .Where(x => x.Title.Contains(NameSorter.Text))
                    .OrderBy(x => x.ID)
                    .ToList();
            }
            else
            {
                DataSuppliers = Context.Suppliers .OrderBy(x => x.ID).ToList();
            }
            
            // Вызываем метод для обновления кнопок
            CreatePagingList();
            
            // Потом мы делаем пагинацию
            DataSuppliers = DataSuppliers.Skip(20 * page).Take(20).ToList();

            // После этого сортируем 
            DataSuppliers = DateStartSorter.SelectedIndex == 0 
                ? DataSuppliers.OrderBy(x => x.StartDate).ToList() 
                : DataSuppliers.OrderByDescending(x => x.StartDate).ToList();

            // Обновляем источник для элемента компоновки и обновляем лэйбл
            Suppliers.ItemsSource = DataSuppliers;
            CountOfSupplier.Content = $"{(20 * page) + DataSuppliers.Count}/{Context.Suppliers.Count()}";
        }

        /// <summary>
        /// Метод для более сложной пагинации с кнопками
        /// </summary>
        private void CreatePagingList()
        {
            // Очищаем весь список
            PagingPanel.Children.RemoveRange(0, PagingPanel.Children.Count);
            
            // Задаем все кнопки пагинации 
            for (var i = 1; i <= (int)Math.Round((double)DataSuppliers.Count / 20); i++)
            {
                var button = new Button
                {
                    Width = 30,
                    Height = 30,
                    Content = i.ToString()
                };
                button.Click += OnButtonClick;
                
                // Добавляем в панель
                PagingPanel.Children.Add(button);
            }
        }
    }
}
