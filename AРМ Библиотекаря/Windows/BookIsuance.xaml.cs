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
using static AРМ_Библиотекаря.Classes.Variables;

namespace AРМ_Библиотекаря
{
    /// <summary>
    /// Логика взаимодействия для BookIsuance.xaml
    /// </summary>
    public partial class BookIsuance : Window
    {
        Book bookItem;
        public BookIsuance()
        {
            InitializeComponent();
            cndIssue.SelectedDate = DateTime.Today.AddDays(0);
            cndPass.SelectedDate = DateTime.Today.AddDays(14);
        }
        public void GetBook(Book tempBookItem)
        {
            bookItem = tempBookItem;
            tbxBookNumber.Text = bookItem.BookNumber;
            tbxBookName.Text = bookItem.BookName;
            tbxAuthor.Text = bookItem.BookAutor;
            tbxBookGenres.Text = bookItem.BookGenre;
            tbxNamePublishing.Text = bookItem.BookPublishing;
            tbxYearPublishing.Text = bookItem.BookYear;
            tbxAgeLimit.Text = bookItem.BookAge;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnUploadReader_Click(object sender, RoutedEventArgs e)
        {

            if (tbxNumberCard.Text.Length > 0)
            {
                DataTable dataReader = BasicFunction.Select($"SELECT * FROM Reading_Card WHERE id_readingCard = {int.Parse(tbxNumberCard.Text)}");
                for (int i = 0; i < dataReader.Rows.Count; i++)
                {
                    tbxNameReader.Text = dataReader.Rows[i][1].ToString() + " " + dataReader.Rows[i][2].ToString() + " " + dataReader.Rows[i][3].ToString();
                    tbxPhoneNumber.Text = dataReader.Rows[i][4].ToString();
                    tbxReaderAge.Text = dataReader.Rows[i][9].ToString();
                }
            }
            else MessageBox.Show("Введите номер карточки читателя!", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnIssuedBook_Click(object sender, RoutedEventArgs e)
        {
            if (tbxNameReader.Text.Length > 0 && tbxNameReader.Text.Length > 0)
            {
                DataTable debtors_check = BasicFunction.Select($"SELECT * FROM IssueBook WHERE id_readingCard = {int.Parse(tbxNumberCard.Text)}");
                if (debtors_check.Rows[0][4].ToString() == "X")
                {
                    MessageBox.Show("Данный читатель является задолжником библиотеки. Выдачи книги задолжникам не осуществляется.", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    int checkAgeReader = int.Parse(tbxReaderAge.Text);
                    int checkAgeLimit = int.Parse(tbxAgeLimit.Text);
                    if (checkAgeLimit < checkAgeReader || checkAgeLimit == checkAgeReader)
                    {
                        DataTable checkCountIssuedBook = BasicFunction.Select($"SELECT id_book FROM IssueBook WHERE id_readingCard = {int.Parse(tbxNumberCard.Text)}");
                        if (checkCountIssuedBook.Rows.Count < 5)
                        {
                            int id_book = int.Parse(BasicFunction.Select($"SELECT id_book FROM Book WHERE inventory_number = '{tbxBookNumber.Text}'").Rows[0][0].ToString());
                            BasicFunction.Select($"INSERT INTO IssueBook VALUES" + $"({id_book}, {int.Parse(tbxNumberCard.Text)}, '{cndIssue.SelectedDate}', '{cndPass.SelectedDate}', 'V')");
                            BasicFunction.Select($"UPDATE Book SET is_enable = 'X' WHERE id_book = {id_book}");
                            MessageBox.Show("Выдача успешно оформлена", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        }
                        else MessageBox.Show("Читатель может иметь не более 5 выданных книг", "", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else MessageBox.Show("Возраст читателя не соответсвует возрастному ограничению", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }else MessageBox.Show("Заполните поля читателя", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void tbxNumberCard_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbxNumberCard.MaxLength = 7;
            TextBox textBox = sender as TextBox;
            char[] charList = textBox.Text.ToCharArray();
            for (int i = 0; i < charList.Length; i++)
            {
                if (!BasicFunction.validateNumber(charList[i].ToString()))
                {
                    textBox.Text = textBox.Text.Remove(i, 1);
                }
            }
        }
    }
}
