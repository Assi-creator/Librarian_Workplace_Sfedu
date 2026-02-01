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
    /// Логика взаимодействия для ReaderInformation.xaml
    /// </summary>
    public partial class ReaderInformation : Window
    {
        public ObservableCollection<Reader> ReadersList { get; set; } = new ObservableCollection<Reader>();

        public ReaderInformation()
        {
            InitializeComponent();
            LoadInformation("all", "all");
        }

        void FillingTable(DataTable dataReader)
        {
            ReadersList.Clear();
            for (int i = 0; i < dataReader.Rows.Count; i++)
            {
                string tempEducation = (BasicFunction.Select($"SELECT name FROM Education WHERE id_education = {dataReader.Rows[i][10]}") as DataTable).Rows[0][0].ToString();
                string tempPlaceOfStudy = dataReader.Rows[i][7].ToString();
                if (tempPlaceOfStudy == null || tempPlaceOfStudy == "")
                {
                    tempPlaceOfStudy = "Дошкольник";
                }

                string tempClass = dataReader.Rows[i][8].ToString();
                if (tempClass == "")
                {
                    tempClass = "-";
                }

                Reader dataCard = new Reader()
                {
                    ReaderNumber = dataReader.Rows[i][0].ToString(),
                    ReaderSNP = dataReader.Rows[i][1].ToString() + " " + dataReader.Rows[i][2].ToString() + " " + dataReader.Rows[i][3].ToString(),
                    ReaderPhone = dataReader.Rows[i][4].ToString(),
                    ReaderAddress = dataReader.Rows[i][5].ToString(),
                    ReaderEducation = tempEducation,
                    ReaderBithdate = dataReader.Rows[i][6].ToString().Replace("0:00:00", "").Trim(),
                    ReaderPlaceOfStudy = tempPlaceOfStudy,
                    ReaderClass = tempClass,
                    ReaderAgeLimit = dataReader.Rows[i][9].ToString(),
                };
                ReadersList.Add(dataCard);

            }
            dtgReader.ItemsSource = ReadersList;
        }

        void LoadInformation(string readersIds, string function)
        {
            dtgReader.ItemsSource = null;

            DataTable dataReaders;
            if (readersIds == "all" && function == "all")
            {
                dataReaders = BasicFunction.Select("SELECT * FROM [dbo].[Reading_Card]");
                FillingTable(dataReaders);
            }
            else if (function == "readernumber")
            {
                dataReaders = BasicFunction.Select($"SELECT * FROM [dbo].[Reading_Card] WHERE id_readingCard IN ({readersIds})");
                FillingTable(dataReaders);
            }
            else if (function == "readername")
            {
                dataReaders = BasicFunction.Select($"SELECT * FROM [dbo].[Reading_Card] WHERE id_readingCard IN ({readersIds})");
                FillingTable(dataReaders);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LibraryFund library = new LibraryFund();
            library.Show();
            this.Close();
        }

        private void btnRegistrated_Click(object sender, RoutedEventArgs e)
        {
            AddReaderCard addReaderCard = new AddReaderCard();
            addReaderCard.Show();          
        }

        private void btnIssueBook_Click(object sender, RoutedEventArgs e)
        {
            BookIsuance bookIsuance = new BookIsuance();
            bookIsuance.Show();
        }

        private void btnDebtorsList_Click(object sender, RoutedEventArgs e)
        {
            DebtorsList debtorsList = new DebtorsList();
            debtorsList.Show();           
        }

        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {
            Statistics statistics = new Statistics();
            statistics.Show();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            
            if (rbNumber.IsChecked == true) 
            {
                string condition = "";

                if (tbxSearch.Text != "")
                {
                    string number = tbxSearch.Text;

                    if (tbxSearch.Text != "" && BasicFunction.validateNumber(number))
                    {
                        condition += String.Format($"WHERE id_readingCard = {number}");
                    }

                    DataTable dataClients = BasicFunction.Select($"SELECT id_readingCard FROM [dbo].[Reading_Card] {condition}");
                    string readersIds = "";
                    if (dataClients.Rows.Count < 1)
                    {
                        MessageBox.Show("По вашему запросу ничего не найдено!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dataClients.Rows.Count; i++)
                        {
                            if (readersIds != "") readersIds += ",";
                            readersIds += dataClients.Rows[i][0].ToString();
                        }

                        string function = "readernumber";
                        LoadInformation(readersIds, function);
                    }
                }
            }
            
            if (rbName.IsChecked == true)
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

                    DataTable dataClients = BasicFunction.Select($"SELECT id_readingCard FROM [dbo].[Reading_Card] {condition}");
                    string readersIds = "";
                    if (dataClients.Rows.Count < 1)
                    {
                        MessageBox.Show("По вашему запросу ничего не найдено!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dataClients.Rows.Count; i++)
                        {
                            if (readersIds != "") readersIds += ",";
                            readersIds += dataClients.Rows[i][0].ToString();
                        }
                        string function = "readername";
                        LoadInformation(readersIds, function);
                    }
                }
            }
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

            if(rbName.IsChecked == true)
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
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить читательскую карточку?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Reader readerItem = dtgReader.SelectedItem as Reader;
                if (readerItem == null)
                {
                    MessageBox.Show("Выберите строку из таблицы!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    BasicFunction.Select($"DELETE FROM Reading_Card WHERE id_readingCard = {readerItem.ReaderNumber}");
                    MessageBox.Show("Читатель успешно удален!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadInformation("all", "all");
                }
            }
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
