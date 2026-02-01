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
    /// Логика взаимодействия для IssuedBooks.xaml
    /// </summary>
    public partial class IssuedBooks : Window
    {
        public ObservableCollection<IssueBook> IssueList { get; set; } = new ObservableCollection<IssueBook>();
        public IssuedBooks()
        {
            InitializeComponent();
            CheckDebtors();
            LoadIssueBook();           
        }

        void CheckDebtors()
        {
            DataTable checkDebtors = BasicFunction.Select("SELECT * FROM IssueBook"); 

            for (int i = 0; i < checkDebtors.Rows.Count; i++)
            {
                DateTime now = DateTime.Today.AddDays(0);
                DateTime pass_day = Convert.ToDateTime(BasicFunction.Select($"SELECT date_pass FROM IssueBook").Rows[i][0].ToString());

                if (now > pass_day)
                {
                    BasicFunction.Select($"UPDATE IssueBook SET condition = 'X' WHERE id_book = {checkDebtors.Rows[i][0]}");

                }
                else if (now > pass_day.AddDays(-1) || now > pass_day.AddDays(-2) || now > pass_day.AddDays(-3))
                {
                    BasicFunction.Select($"UPDATE IssueBook SET condition = 'O' WHERE id_book = {checkDebtors.Rows[i][0]}");
                }
                else if (now < pass_day)
                {
                    BasicFunction.Select($"UPDATE IssueBook SET condition = 'V' WHERE id_book = {checkDebtors.Rows[i][0]}");
                }
            }
        }

        void LoadIssueBook()
        {
            IssueList.Clear();
            DataTable dataIssueBook = BasicFunction.Select("SELECT * FROM IssueBook WHERE condition = 'O' or condition = 'V' ORDER BY date_pass ASC ");

            
            for (int i = 0; i < dataIssueBook.Rows.Count; i++)
            {
                string tempSurname = BasicFunction.Select($"SELECT surname FROM Reading_Card WHERE id_readingCard IN (SELECT id_readingCard FROM IssueBook WHERE id_book = {dataIssueBook.Rows[i][0]})").Rows[0][0].ToString();
                string tempName = BasicFunction.Select($"SELECT name FROM Reading_Card WHERE id_readingCard IN (SELECT id_readingCard FROM IssueBook WHERE id_book = {dataIssueBook.Rows[i][0]})").Rows[0][0].ToString();
                string tempPatronymic = BasicFunction.Select($"SELECT patronymic FROM Reading_Card WHERE id_readingCard IN (SELECT id_readingCard FROM IssueBook WHERE id_book = {dataIssueBook.Rows[i][0]})").Rows[0][0].ToString();
                string tempBookName = BasicFunction.Select($"SELECT name FROM Book WHERE id_book IN (SELECT id_book FROM IssueBook WHERE id_book = {dataIssueBook.Rows[i][0]})").Rows[0][0].ToString();
                string tempInventoryNumber = BasicFunction.Select($"SELECT inventory_number FROM Book WHERE id_book IN (SELECT id_book FROM IssueBook WHERE id_book = {dataIssueBook.Rows[i][0]})").Rows[0][0].ToString();

                IssueBook issueBook = new IssueBook()
                {
                    NumberReaderCard = dataIssueBook.Rows[i][1].ToString(),
                    ReaderName = tempSurname + " " + tempName + " " + tempPatronymic,
                    BookNumber = tempInventoryNumber,
                    BookName = tempBookName,
                    DataIssue = dataIssueBook.Rows[i][2].ToString().Replace("0:00:00", "").Trim(),
                    DataPass = dataIssueBook.Rows[i][3].ToString().Replace("0:00:00", "").Trim(),
                    Condition = dataIssueBook.Rows[i][4].ToString(),
                };
                IssueList.Add(issueBook);
            }
            dtgIssueBook.ItemsSource = IssueList;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void miClose_Click(object sender, RoutedEventArgs e)
        {
            IssueBook issueBookItem = dtgIssueBook.SelectedItem as IssueBook;
            if (issueBookItem == null)
            {
                MessageBox.Show("Выберите строку из таблицы!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            BasicFunction.Select($"UPDATE Book SET is_enable = 'V' WHERE id_book = {int.Parse(issueBookItem.BookNumber)}");
            BasicFunction.Select($"DELETE FROM IssueBook WHERE id_book = {int.Parse(issueBookItem.BookNumber)}");
            MessageBox.Show("Возврат книги успешно оформлен!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadIssueBook();
        }

        private void dtgIssueBook_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            IssueBook rowItem = e.Row.Item as IssueBook;
            if (rowItem != null)
            {
                if (rowItem.Condition == "X")
                {
                    e.Row.Background = new SolidColorBrush(Color.FromArgb(200, 240, 222, 240));
                }
                else if (rowItem.Condition == "O")
                {
                    e.Row.Background = new SolidColorBrush(Color.FromArgb(200, 222, 240, 222)); 
                }
                
            }
        }

    }
}
