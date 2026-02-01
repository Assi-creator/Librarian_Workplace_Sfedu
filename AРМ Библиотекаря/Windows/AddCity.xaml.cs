using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AРМ_Библиотекаря
{
    /// <summary>
    /// Логика взаимодействия для AddCity.xaml
    /// </summary>
    public partial class AddCity : Window
    {
        public AddCity()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddCity_Click(object sender, RoutedEventArgs e)
        {
            if (tbxCityName.Name != "")
            {
                BasicFunction.Select($"INSERT INTO City VALUES" + $"('{tbxCityName.Text}')");
                MessageBox.Show("Город успешно добавлен!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            MessageBox.Show("Введите название города!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void tbxCityName_TextChanged(object sender, TextChangedEventArgs e)
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
