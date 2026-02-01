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
    /// Логика взаимодействия для AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        public ObservableCollection<string> PublishingList = new ObservableCollection<string>();

        public ObservableCollection<string> AgeLimitList = new ObservableCollection<string>();
        public ObservableCollection<Genres> GenresList { get; set; } = new ObservableCollection<Genres>();
        public ObservableCollection<Authors> AuthorsList { get; set; } = new ObservableCollection<Authors>();

        public AddBook()
        {
            InitializeComponent();
            LoadGenres();
            LoadPublishing();
            LoadAuthors();
            LoadInventoryNumber();
            LoadAgeLimit();
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

        int[] CheckedAuthors()
        {
            int[] checkedAuthors = new int[0];

            for (int i = 0; i < AuthorsList.Count; i++)
            {
                if (AuthorsList[i].IsChecked == true)
                {
                    Array.Resize(ref checkedAuthors, checkedAuthors.Length + 1);
                    checkedAuthors[checkedAuthors.Length - 1] = i;
                }
            }
            return checkedAuthors;
        }

        void LoadInventoryNumber()
        {
            DataTable tempData = BasicFunction.Select("SELECT inventory_number FROM Book WHERE id_book = (SELECT Max(id_book) FROM Book)");
            int inventory_number = int.Parse(tempData.Rows[0][0].ToString());
            int new_inventory_number = inventory_number + 1;
            string number = "00000000" + new_inventory_number.ToString();            
            string result = number.Substring(number.Length - 7);          
            tbxNumber.Text = result;
        }

        void LoadAuthors()
        {
            DataTable dataAuthor = BasicFunction.Select("SELECT * FROM Author ORDER BY surname");
            for (int i = 0; i < dataAuthor.Rows.Count; i++)
            {
                Authors dataAuthors = new Authors()
                {
                    AuthorId = int.Parse(dataAuthor.Rows[i][0].ToString()),
                    AuthorName = dataAuthor.Rows[i][1].ToString() + " " + dataAuthor.Rows[i][2].ToString() + " " + dataAuthor.Rows[i][3].ToString(),
                    IsChecked = false,
                };
                cmbAuthor.Items.Add(dataAuthors);
                AuthorsList.Add(dataAuthors);
            }
        }

        void LoadAgeLimit()
        {
            DataTable dataAge = BasicFunction.Select("SELECT DISTINCT age_limit FROM Book ORDER BY age_limit");
            for (int i = 0; i < dataAge.Rows.Count; i++)
            {
                AgeLimitList.Add(dataAge.Rows[i][0].ToString());
            }
            cmbAgeLimit.ItemsSource = AgeLimitList;
            cmbAgeLimit.SelectedIndex = 0;
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

        void LoadPublishing()
        {
            PublishingList.Clear();
            DataTable dataPublishing = BasicFunction.Select($"SELECT name FROM Publishing");
            for (int i = 0; i < dataPublishing.Rows.Count; i++)
            {
                PublishingList.Add(dataPublishing.Rows[i][0].ToString());
            }
            cmbPublishing.ItemsSource = PublishingList;
            cmbPublishing.SelectedIndex = 0;
        }

        void Clear()
        {          
            tbxName.Clear();
            tbxNumber.Clear();
            tbxCountPages.Clear();
            tbxNumberAccompanying.Clear();
            tbxYearPublishing.Clear();
            tbxDateAccompanying.Clear();
            cmbAgeLimit.SelectedIndex = 0;
            cmbPublishing.SelectedIndex = 0;
            LoadGenres();
            LoadAuthors();
            LoadInventoryNumber();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAddBook_Click(object sender, RoutedEventArgs e)
        {
            int[] checkedGenres = CheckedGenres();                        
            int[] checkedAuthors = CheckedAuthors();   
            bool date_accompanying_ismatch = false;            
            bool date_publishing_ismatch = false;
           
            if (Regex.IsMatch(tbxYearPublishing.Text, @"^(18|19|20)?[0-9]{2}$")) date_publishing_ismatch = true;
            else
            {
                MessageBox.Show("\nНеправильно введена дата публикации. Дата должна быть в формате \"ГГГГ-ММ-ДД\"");
                return;
            };
            if (Regex.IsMatch(tbxDateAccompanying.Text, @"^(19|20)?[0-9]{2}[-](0?[1-9]|1[012])[-](0?[1-9]|[12][0-9]|3[01])$")) date_accompanying_ismatch = true;
            else
            {
                MessageBox.Show("\nНеправильно введена дата получения. Дата должна быть в формате \"ГГГГ-ММ-ДД\"");
                return;
            };

            if (tbxName.Text != "" && tbxCountPages.Text != "" && tbxNumberAccompanying.Text != ""
                && tbxYearPublishing.Text != "" && tbxDateAccompanying.Text != "" &&  date_accompanying_ismatch && date_publishing_ismatch)
            {
                if (checkedGenres.Length > 0)
                {
                    if (checkedAuthors.Length > 0)
                    {
                        int ageLimit = int.Parse(cmbAgeLimit.SelectedItem.ToString());
                        DateTime data_accompanuing = Convert.ToDateTime(tbxDateAccompanying.Text);
                        int id_publishing = int.Parse(BasicFunction.Select($"SELECT * FROM Publishing WHERE name = '{cmbPublishing.SelectedItem.ToString()}'").Rows[0][0].ToString());
                        BasicFunction.Select($"INSERT INTO Book VALUES" + $"('{tbxNumber.Text}', '{tbxName.Text}', {int.Parse(tbxYearPublishing.Text)}, {int.Parse(tbxCountPages.Text)}, {ageLimit}, NULL, '{data_accompanuing}', {int.Parse(tbxNumberAccompanying.Text)}, {id_publishing}, 'V')");

                        DataTable tempMaxBook = BasicFunction.Select("SELECT MAX(id_book) FROM Book");
                        int currentMaxBookId = int.Parse(tempMaxBook.Rows[0][0].ToString());

                        for (int i = 0; i < checkedGenres.Length; i++)
                        {
                            BasicFunction.Select($"INSERT INTO Book_Genre VALUES " + $"({currentMaxBookId}, {GenresList[checkedGenres[i]].GenreId})");
                        }

                        for (int i = 0; i < checkedAuthors.Length; i++)
                        {
                            BasicFunction.Select($"INSERT INTO Book_Author VALUES " + $"({currentMaxBookId}, {AuthorsList[checkedAuthors[i]].AuthorId})");
                        }

                        MessageBox.Show("Книга успешно добавлена в библиотечный фонд!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        Clear();
                    }
                    else
                    {
                        MessageBox.Show("Выберите автора!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите жанры!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
            }
            else
            {
                MessageBox.Show("Введите данные в поля!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
     
        private void btnAddPublishing_Click(object sender, RoutedEventArgs e)
        {
            AddPublishing addPublishing = new AddPublishing();
            addPublishing.Show();
        }

        private void btnAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            AddAuthor addAuthor = new AddAuthor();
            addAuthor.Show();
        }

        private void btnRefreshAuthor_Click(object sender, RoutedEventArgs e)
        {
            LoadAuthors();
        }

        private void tbxName_TextChanged(object sender, TextChangedEventArgs e)
        {          
            TextBox textBox = sender as TextBox;
            char[] charList = textBox.Text.ToCharArray();
            for (int i = 0; i < charList.Length; i++)
            {
                if (!BasicFunction.validateBookName(charList[i].ToString()))
                {
                    textBox.Text = textBox.Text.Remove(i, 1);
                }
            }
        }

        private void tbxYearPublishing_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbxYearPublishing.MaxLength = 4;
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

        private void tbxCountPages_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbxCountPages.MaxLength = 4;
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

        private void tbxNumberAccompanying_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbxNumberAccompanying.MaxLength = 6;
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

        private void tbxDateAccompanying_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbxDateAccompanying.MaxLength = 10;
            TextBox textBox = sender as TextBox;
            char[] charList = textBox.Text.ToCharArray();
            for (int i = 0; i < charList.Length; i++)
            {
                if (!BasicFunction.validateBirthday(charList[i].ToString()))
                {
                    textBox.Text = textBox.Text.Remove(i, 1);
                }
            }
        }
    }
}
