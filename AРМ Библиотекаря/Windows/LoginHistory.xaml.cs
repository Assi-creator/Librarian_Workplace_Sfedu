using System;
using System.Collections.Generic;
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

namespace AРМ_Библиотекаря
{
    /// <summary>
    /// Логика взаимодействия для LoginHistory.xaml
    /// </summary>
    public partial class LoginHistory : Window
    {
        public LoginHistory()
        {
            InitializeComponent();
            LoadLoginHistory();
        }

        public class Login
        {
            public string userSNP { get; set; }
            public string userLogin { get; set; }
            public string userLoginDate { get; set; }       
        }

        void LoadLoginHistory() 
        {
            DataTable dataLogin = BasicFunction.Select($"SELECT * FROM [dbo].[LoginHistory] ORDER BY login_date DESC");

            for (int i = 0; i < dataLogin.Rows.Count; i++)
            {
                string tempSurname = BasicFunction.Select($"SELECT surname FROM [dbo].[User] WHERE id_user IN (SELECT id_user FROM LoginHistory WHERE id_user = {dataLogin.Rows[i][2]})").Rows[0][0].ToString();
                string tempName = BasicFunction.Select($"SELECT name FROM [dbo].[User] WHERE id_user IN (SELECT id_user FROM LoginHistory WHERE id_user = {dataLogin.Rows[i][2]})").Rows[0][0].ToString();
                string tempPatronymic = BasicFunction.Select($"SELECT patronymic FROM [dbo].[User] WHERE id_user IN (SELECT id_user FROM LoginHistory WHERE id_user = {dataLogin.Rows[i][2]})").Rows[0][0].ToString();                
                string tempLogin = BasicFunction.Select($"SELECT login FROM [dbo].[User] WHERE id_user IN (SELECT id_user FROM LoginHistory WHERE id_user = {dataLogin.Rows[i][2]})").Rows[0][0].ToString();                

                Login dataDoctor = new Login()
                {
                    userSNP = tempSurname + ' ' + tempName + ' ' + tempPatronymic,
                    userLoginDate = dataLogin.Rows[i][1].ToString(),
                    userLogin = tempLogin,
                };
                dgLoginHistory.Items.Add(dataDoctor);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {         
            this.Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt_user = BasicFunction.Select($"SELECT * FROM LoginHistory");
            if (MessageBox.Show("Вы действительно хотите очистить историю входа?", "Очистка истории входа", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (dt_user.Rows.Count > 0)
                {
                    BasicFunction.Select($"TRUNCATE TABLE [dbo].[LoginHistory]");
                    dgLoginHistory.Items.Clear();
                    dgLoginHistory.Items.Refresh();
                    MessageBox.Show("История входа успешно очищена!", "Очистка истории входа", MessageBoxButton.OK, MessageBoxImage.Information);                                     
                }
                else MessageBox.Show("История входа отсутсвует", "Очистка истории входа", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }    
    }
}
