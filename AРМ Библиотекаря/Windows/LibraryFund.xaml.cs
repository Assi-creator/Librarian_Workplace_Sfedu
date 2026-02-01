using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using static AРМ_Библиотекаря.Classes.Variables;

namespace AРМ_Библиотекаря
{
    /// <summary>
    /// Логика взаимодействия для LibraryFund.xaml
    /// </summary>
    public partial class LibraryFund : Window
    {
        public ObservableCollection<Genres> GenresList { get; set; } = new ObservableCollection<Genres>();
        public ObservableCollection<Book> BooksList { get; set; } = new ObservableCollection<Book>();
        
             
        public LibraryFund()
        {
            InitializeComponent();
            LoadInformation("all", "all");
            LoadGenres();
        }

        int[] CheckedGenres()
        {
            int[] checkedGenres = new int[0];

            for (int i = 0; i < GenresList.Count; i++)
            {
                if (GenresList[i].IsChecked == true)
                {
                    Array.Resize(ref checkedGenres, checkedGenres.Length + 1);
                    checkedGenres[checkedGenres.Length - 1] = i;
                }
            }
            return checkedGenres;
        }

        void LoadGenres()
        {
            DataTable dataGenre = BasicFunction.Select("SELECT * FROM Genre");
            for (int i = 0; i < dataGenre.Rows.Count; i++)
            {
                Genres dataGenres = new Genres()
                {
                    GenreId = int.Parse(dataGenre.Rows[i][0].ToString()),
                    GenreName = dataGenre.Rows[i][1].ToString(),
                    IsChecked = false,
                };
                cmbGenre.Items.Add(dataGenres);
                GenresList.Add(dataGenres);
            }
        }

        void FillingTable(DataTable dataBooks)
        {
            BooksList.Clear();
            for (int i = 0; i < dataBooks.Rows.Count; i++)
            {
                string tempSurname = (BasicFunction.Select($"SELECT surname FROM Author WHERE id_autor = (SELECT id_autor FROM Book_Author WHERE id_book = {dataBooks.Rows[i][0]})") as DataTable).Rows[0][0].ToString();
                string tempName = (BasicFunction.Select($"SELECT name FROM Author WHERE id_autor = (SELECT id_autor FROM Book_Author WHERE id_book = {dataBooks.Rows[i][0]})") as DataTable).Rows[0][0].ToString();
                string tempPatronymic = (BasicFunction.Select($"SELECT patronymic FROM Author WHERE id_autor = (SELECT id_autor FROM Book_Author WHERE id_book = {dataBooks.Rows[i][0]})") as DataTable).Rows[0][0].ToString();
                string tempPublishing = (BasicFunction.Select($"SELECT name FROM Publishing WHERE id_publishing = (SELECT id_publishing FROM Book WHERE id_book = {dataBooks.Rows[i][0]})") as DataTable).Rows[0][0].ToString();

                DataTable dataGenres = BasicFunction.Select($"SELECT name FROM Genre WHERE id_genre IN (SELECT id_genre FROM Book_Genre WHERE id_book = {dataBooks.Rows[i][0]})");

                string[] genreList = new string[dataGenres.Rows.Count];
                for (int j = 0; j < dataGenres.Rows.Count; j++)
                {
                    genreList[j] = dataGenres.Rows[j][0].ToString();
                }

                string genres = "";
                for (int f = 0; f < genreList.Length; f++)
                {
                    if (genres != "") genres += ", ";
                    genres += genreList[f];
                }


                Book dataBook = new Book()
                {
                    BookNumber = dataBooks.Rows[i][1].ToString(),
                    BookName = dataBooks.Rows[i][2].ToString(),
                    BookAutor = tempSurname + ' ' + tempName + ' ' + tempPatronymic,
                    BookGenre = genres,
                    BookPublishing = tempPublishing,
                    BookYear = dataBooks.Rows[i][3].ToString(),
                    BookPages = dataBooks.Rows[i][4].ToString(),
                    BookAge = dataBooks.Rows[i][5].ToString(),
                    IsEnable = dataBooks.Rows[i][10].ToString(),
                };
                BooksList.Add(dataBook);
                
            }
            dtgBook.ItemsSource = BooksList;
        }

        void LoadInformation(string booksIds, string function)
        {
            dtgBook.ItemsSource = null;

            DataTable dataBooks;
            if (booksIds == "all" && function == "all")
            {
                dataBooks = BasicFunction.Select("SELECT * FROM [dbo].[Book]");
                FillingTable(dataBooks);
            }
            else if (function == "booknumber")
            {
                dataBooks = BasicFunction.Select($"SELECT * FROM [dbo].[Book] WHERE id_book IN ({booksIds})");
                FillingTable(dataBooks);
            }
            else if (function == "bookname")
            {
                dataBooks = BasicFunction.Select($"SELECT * FROM [dbo].[Book] WHERE id_book IN ({booksIds})");
                FillingTable(dataBooks);
            }
            else if (function == "authorname")
            {
                dataBooks = BasicFunction.Select($"SELECT * FROM [dbo].[Book] WHERE id_book IN ({booksIds})");
                FillingTable(dataBooks);
            }
            else if (function == "checkbox")
            {
                dataBooks = BasicFunction.Select($"SELECT * FROM [dbo].[Book] WHERE id_book IN ({booksIds})");
                FillingTable(dataBooks);
            }
  
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }

        private void btnChangeAccount_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти из аккаунта?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MainWindow entry = new MainWindow();
                entry.Show();
                this.Close();
            }
        }

        private void miIssue_Click(object sender, RoutedEventArgs e)
        {
            Book bookItem = dtgBook.SelectedItem as Book;
            if (bookItem.IsEnable == "X")
            {
                MessageBox.Show("Эта книга уже выдана!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (bookItem == null)
            {
                MessageBox.Show("Выберите строку из таблицы!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                BookIsuance issue = new BookIsuance();
                issue.GetBook(bookItem);
                issue.Show();
            }
            
        }

        private void miDecommission_Click(object sender, RoutedEventArgs e)
        {
            Book bookItem = dtgBook.SelectedItem as Book;
           
            if (bookItem.IsEnable == "X")
            {
                MessageBox.Show("Невозможно списать выданную книгу!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (bookItem == null)
            {
                MessageBox.Show("Выберите строку из таблицы!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                DecommissionBook decomission = new DecommissionBook();
                decomission.GetBook(bookItem);
                decomission.Show();
            }
        }

        private void btnSearchGenre_Click(object sender, RoutedEventArgs e)
        {
            string condition = "";
            int[] checkedGenres = CheckedGenres();
            if (checkedGenres.Length > 0)
            {
                string checkedGenresIds = "";
                for (int i = 0; i < checkedGenres.Length; i++)
                {
                    if (checkedGenresIds != "") checkedGenresIds += ",";
                    checkedGenresIds += GenresList[checkedGenres[i]].GenreId.ToString();
                }

                if (checkedGenresIds != "")
                {
                    if (condition == "") condition += "WHERE ";
                    else condition += " AND ";

                    condition += String.Format("id_genre IN ({0})", checkedGenresIds);
                }
            }
          
            DataTable dataBook = BasicFunction.Select($"SELECT id_book FROM Book_Genre {condition}");
            string booksIds = "";
            if (dataBook.Rows.Count < 1)
            {
                MessageBox.Show("По вашему запросу ничего не найдено!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {      
                for (int i = 0; i < dataBook.Rows.Count; i++)
                {
                    if (booksIds != "") booksIds += ",";
                    booksIds += dataBook.Rows[i][0].ToString();
                }
                string function = "checkbox";
                LoadInformation(booksIds, function);                
            }
        }

        private void btnReaders_Click(object sender, RoutedEventArgs e)
        {
            ReaderInformation readerInformation = new ReaderInformation();
            readerInformation.Show();
            Close();
        }

        private void btnIssueBook_Click(object sender, RoutedEventArgs e)
        {
            BookIsuance bookIsuance = new BookIsuance();
            bookIsuance.Show();
        }

        private void btnIssuedBooks_Click(object sender, RoutedEventArgs e)
        {
            IssuedBooks issuedBooks = new IssuedBooks();
            issuedBooks.Show();            
        }

        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.Show();            
        }

        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook();
            addBook.Show();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {          
            if (rbNumber.IsChecked == true) 
            {
                string condition = "";
                bool inventory_number_ismatch = false;
                if (Regex.IsMatch(tbxSearch.Text.Trim(), @"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]*$")) inventory_number_ismatch = true;

                if (tbxSearch.Text != "")
                {
                    string number = tbxSearch.Text;

                    if (tbxSearch.Text != "" && BasicFunction.validateNumber(number) && inventory_number_ismatch)
                    {
                        condition += String.Format($"WHERE inventory_number = {number}");
                    }
                    else { MessageBox.Show("Неверный формат инвентарного номера!"); }

                    DataTable dataBook = BasicFunction.Select($"SELECT id_book FROM [dbo].[Book] {condition}");
                    string booksIds = "";
                    if (dataBook.Rows.Count < 1)
                    {
                        MessageBox.Show("По вашему запросу ничего не найдено!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {

                        for (int i = 0; i < dataBook.Rows.Count; i++)
                        {
                            if (booksIds != "") booksIds += ",";
                            booksIds += dataBook.Rows[i][0].ToString();
                        }

                        string function = "booknumber";
                        LoadInformation(booksIds, function);
                    }
                }
            }

            if (rbName.IsChecked == true) 
            {

                string condition = "";

                if (tbxSearch.Text != "")
                {
                    string namebook = tbxSearch.Text;

                    if (tbxSearch.Text != "" && BasicFunction.validateName(namebook))
                    {
                        condition += String.Format($"WHERE name LIKE '%{namebook}%'");
                    }

                    DataTable dataBook = BasicFunction.Select($"SELECT id_book FROM [dbo].[Book] {condition}");
                    string booksIds = "";
                    if (dataBook.Rows.Count < 1)
                    {
                        MessageBox.Show("По вашему запросу ничего не найдено!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dataBook.Rows.Count; i++)
                        {
                            if (booksIds != "") booksIds += ",";
                            booksIds += dataBook.Rows[i][0].ToString();
                        }

                        string function = "booknumber";
                        LoadInformation(booksIds, function);
                    }
                }
            }
           
            if (rbAuthor.IsChecked == true) 
                {
                string condition = "";

                if (tbxSearch.Text != "")
                {
                    string[] fullName = tbxSearch.Text.Split(' ');
                    string[] name = new string[3];
                    int currentNameIndex = 0;
                    for (int i = 0; i < fullName.Length; i++)
                    {
                        fullName[i] = fullName[i].Trim();
                        if (BasicFunction.validateName(fullName[i]) && currentNameIndex < 3)
                        {
                            name[currentNameIndex] = fullName[i];
                            currentNameIndex++;
                        }
                        if (currentNameIndex >= 3) break;
                    }

                    if (name[0] != null && BasicFunction.validateName(name[0].ToLower()))
                    {
                        condition += String.Format("WHERE (surname LIKE '%{0}%' OR name LIKE '%{0}%' OR patronymic LIKE '%{0}%')", name[0]);
                    }
                    if (name[1] != null && BasicFunction.validateName(name[1].ToLower()))
                    {
                        if (condition == "") condition += "WHERE ";
                        else condition += " AND ";

                        condition += String.Format("(surname LIKE '%{0}%' OR name LIKE '%{0}%' OR patronymic LIKE '%{0}%')", name[1]);
                    }
                    if (name[2] != null && BasicFunction.validateName(name[2].ToLower()))
                    {
                        if (condition == "") condition += "WHERE ";
                        else condition += " AND ";

                        condition += String.Format("(surname LIKE '%{0}%' OR name LIKE '%{0}%' OR patronymic LIKE '%{0}%')", name[2]);
                    }

                    DataTable dataBook = BasicFunction.Select($"SELECT id_book FROM Book_Author WHERE id_autor IN (SELECT id_autor FROM [dbo].[Author] {condition})");
                    string booksIds = "";
                    if (dataBook.Rows.Count < 1)
                    {
                        MessageBox.Show("По вашему запросу ничего не найдено!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dataBook.Rows.Count; i++)
                        {
                            if (booksIds != "") booksIds += ",";
                            booksIds += dataBook.Rows[i][0].ToString();
                        }
                        string function = "authorname";
                        LoadInformation(booksIds, function);
                    }
                }
            }     
        }
     
        private void cmbGenre_DropDownOpened(object sender, EventArgs e)
        {
            cmbGenre.SelectedIndex = 0;
        }
     
        private void tbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (rbNumber.IsChecked == true)
            {
                tbxSearch.MaxLength = 7;
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

            if (rbName.IsChecked == true)
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

            if (rbAuthor.IsChecked == true)
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

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadInformation("all", "all");
            tbxSearch.Clear();
            rbNumber.IsChecked = true;
        }

        private void btnClearGenreSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadInformation("all", "all");
            LoadGenres();
            CheckedGenres();
        }

        private void dtgBook_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Book rowItem = e.Row.Item as Book;
            if (rowItem != null)
            {
                if (rowItem.IsEnable == "X")
                    e.Row.Background = new SolidColorBrush(Color.FromArgb(200, 240, 231, 222));
            }
        }

       
    }
}

