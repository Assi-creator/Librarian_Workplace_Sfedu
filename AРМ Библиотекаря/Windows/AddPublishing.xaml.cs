using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using static AРМ_Библиотекаря.Classes.Variables;

namespace AРМ_Библиотекаря
{
    /// <summary>
    /// Логика взаимодействия для AddPublishing.xaml
    /// </summary>
    public partial class AddPublishing : Window
    {
        ObservableCollection<string> CityList { get; set; } = new ObservableCollection<string>();

        public AddPublishing()
        {
            InitializeComponent();
            LoadCity();
        }

        void LoadCity()
        {
            CityList.Clear();
            DataTable dataPublishing = BasicFunction.Select("SELECT * FROM City ORDER BY name");
            for (int i = 0; i < dataPublishing.Rows.Count; i++)
            {
                CityList.Add(dataPublishing.Rows[i][1].ToString());
            }

            cmbCity.ItemsSource = CityList;
            cmbCity.SelectedIndex = 0;
        }

        private void btnAddCity_Click(object sender, RoutedEventArgs e)
        {
            AddCity addCity = new AddCity();
            addCity.Show();
        }

        private void btnRefreshCity_Click(object sender, RoutedEventArgs e)
        {
            LoadCity();
        }

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnFAQ_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnAddPublishing_Click(object sender, RoutedEventArgs e)
        { 
            if (tbxNamePublishing.Text.Length > 0 && cmbCity.SelectedIndex != -1)
            {
                string text = cmbCity.SelectionBoxItem.ToString();
                MessageBox.Show(text);
                int id_city = int.Parse(BasicFunction.Select($"SELECT id_city FROM City WHERE name = '{cmbCity.SelectedItem}'").Rows[0][0].ToString());
                MessageBox.Show(id_city.ToString());
                BasicFunction.Select($"INSERT INTO Publishing VALUES" + $"('{tbxNamePublishing.Text}', {id_city})");
                MessageBox.Show("Издательство успешно добавлено!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            MessageBox.Show("Введите корректные данные!", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void tbxNamePublishing_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            char[] charList = textBox.Text.ToCharArray();         
            for (int i = 0; i < charList.Length; i++)
            {
                if (!BasicFunction.validateName(charList[i].ToString()))
                {
                    textBox.Text = textBox.Text.Remove(i, 1);
                }
            }
        }
    }
}
