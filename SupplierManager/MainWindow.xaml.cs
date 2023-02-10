using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SupplierManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Supplier> DataSuppliers;
        LopushokEntities context = new LopushokEntities();
        private int page = 0;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            DataSuppliers = context.Suppliers.ToList();
            DateStartSorter.SelectedIndex = 0;

            Filter();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (page < (int)Math.Round((double)context.Suppliers.Count() / 20) - 1)
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

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(NameSorter.Text))
            {
                DataSuppliers = context.Suppliers
                    .Where(x => x.Title.Contains(NameSorter.Text))
                    .OrderBy(x => x.ID)
                    .Skip(20 * page)
                    .Take(20)
                    .ToList();
            }
            else
            {
                DataSuppliers = context.Suppliers
                    .OrderBy(x => x.ID)
                    .Skip(20 * page)
                    .Take(20)
                    .ToList();
            }

            if (DateStartSorter.SelectedIndex == 0)
            {
                DataSuppliers = DataSuppliers.OrderBy(x => x.StartDate).ToList();
            }
            else
            {
                DataSuppliers = DataSuppliers.OrderByDescending(x => x.StartDate).ToList();
            }


            Suppliers.ItemsSource = DataSuppliers;
            CountOfSupplier.Content = $"{(20 * page) + DataSuppliers.Count}/{context.Suppliers.Count()}";
        }
    }
}
