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
using static AРМ_Библиотекаря.Classes.Variables;

namespace AРМ_Библиотекаря
{
    /// <summary>
    /// Логика взаимодействия для DecommissionBook.xaml
    /// </summary>
    public partial class DecommissionBook : Window
    {
        Book bookItem;
        public DecommissionBook()
        {
            InitializeComponent();
            
        }
     
        public void GetBook(Book tempBookItem)
        {
            bookItem = tempBookItem;
            tbxNumber.Text = bookItem.BookNumber;
            tbxName.Text = bookItem.BookName;
            tbxAuthor.Text = bookItem.BookAutor;
            tbxPublishing.Text = bookItem.BookPublishing;
            tbxYear.Text = bookItem.BookYear;
            tbxCount.Text = bookItem.BookPages;
            tbxAge.Text = bookItem.BookAge;
        }

        private void btnIssueBook_Click(object sender, RoutedEventArgs e)
        {
            string NumberBook = tbxNumber.Text;           
            if (MessageBox.Show($"Списать книгу {tbxName.Text}?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                int id_book = int.Parse(BasicFunction.Select($"SELECT id_book FROM Book WHERE inventory_number = '{NumberBook}'").Rows[0][0].ToString());
                int year = int.Parse(tbxYear.Text);
                int count = int.Parse(tbxCount.Text);
                int age = int.Parse(tbxAge.Text);      
                BasicFunction.Select($"DELETE FROM Book WHERE inventory_number = '{NumberBook}'");
                BasicFunction.Select($"INSERT INTO DecommissionBook VALUES ('{tbxCause.Text}', '{tbxNumber.Text}', '{tbxName.Text}', '{tbxAuthor.Text}', '{tbxPublishing.Text}', {year}, {count}, {age})");
                MessageBox.Show($"Книга успешно списана!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbxCause_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            char[] charList = textBox.Text.ToCharArray();
            for (int i = 0; i < charList.Length; i++)
            {
                if (BasicFunction.validateSymbols(charList[i].ToString()))
                {
                    textBox.Text = textBox.Text.Remove(i, 1);
                }
            }
        }
    }
}
