using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AРМ_Библиотекаря
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
      
        private void btnAutorization_Click(object sender, RoutedEventArgs e)
        {                      
                DataTable autorizationData = BasicFunction.Select("SELECT * FROM [dbo].[User] WHERE [login] = '" + tbxLogin.Text + "' AND [password] = '" + pbxPassword.Password + "'");
            if (autorizationData.Rows.Count > 0)
            {
                DateTime login = DateTime.Now;
                int user = Convert.ToInt32(BasicFunction.Select($"SELECT id_user FROM [dbo].[User] WHERE [login] = '" + tbxLogin.Text + "' AND [password] = '" + pbxPassword.Password + "'").Rows[0][0]);
                BasicFunction.Select("INSERT INTO LoginHistory (login_date, id_user) VALUES ('" + login + "','" + user + "')");
                MessageBox.Show("Пользователь авторизовался!", "Доступ в систему разрешен!", MessageBoxButton.OK, MessageBoxImage.Information);
                LibraryFund library = new LibraryFund();
                library.Show();
                Close();
            }
            else MessageBox.Show("Пользователь не найден", "Ошибка доступа в систему!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            LoginHistory history = new LoginHistory();
            history.Show();
        }

        private void tbxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;
            char[] charList = textBox.Text.ToCharArray();
            for (int i = 0; i < charList.Length; i++)
            {
                if (BasicFunction.validateSymbols(charList[i].ToString()))
                {
                    textBox.Text = textBox.Text.Remove(i, 1);
                }
            }
        }

        private void Button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            tbxPassword.Text = pbxPassword.Password;
            tbxPassword.Visibility = Visibility.Visible;
            pbxPassword.Visibility = Visibility.Hidden;
        }

        private void Button_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pbxPassword.Password = tbxPassword.Text;
            tbxPassword.Visibility = Visibility.Hidden;
            pbxPassword.Visibility = Visibility.Visible;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }
}
