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

namespace AРМ_Библиотекаря
{
    /// <summary>
    /// Логика взаимодействия для AddReaderCard.xaml
    /// </summary>
    public partial class AddReaderCard : Window
    {
        ObservableCollection<string> EducationList { get; set; } = new ObservableCollection<string>();
        public string Patronymic;       
        public int ageReader;
        public AddReaderCard()
        {
            InitializeComponent();
            LoadEducation();
        }

        //Рассчет возраста
        void CalcAge(string birthday)
        {
            DateTime Today = DateTime.Today;
            DateTime Birthday = Convert.ToDateTime(birthday);

            if (Birthday > Today)
            {
                MessageBox.Show("Неверная дата рождения!");
                return;
            }else if(Today.Month <= Birthday.Month && Today.Day < Birthday.Day)
            {
                ageReader = Convert.ToInt32(Today.Year - Birthday.Year) - 1;
            }
            else
            {
                ageReader = Convert.ToInt32(Today.Year - Birthday.Year);
            }
        }

        void LoadEducation()
        {
            EducationList.Clear();
            DataTable dataEducation = BasicFunction.Select("SELECT * FROM Education");
            for (int i = 0; i < dataEducation.Rows.Count; i++)
            {
                EducationList.Add(dataEducation.Rows[i][1].ToString());
            }

            cmbEducation.ItemsSource = EducationList;
            cmbEducation.SelectedIndex = 0;
        }

        void Clear()
        {
            tbxSurname.Clear();
            tbxName.Clear();
            tbxPatronymic.Clear();
            tbxPhone.Clear();
            tbxClass.Clear();
            tbxPlaceOfStudy.Clear();
            tbxAddress.Clear();
            tbxBirthday.Clear();
            cmbEducation.SelectedIndex = 0;
        }

        private void btnAddReadingCard_Click(object sender, RoutedEventArgs e)
        {
            bool date_ismatch = false;
            bool readerName_ismatch = false;

           

            if (tbxPatronymic.Text.Length < 1)
            {
                if (Regex.IsMatch(tbxSurname.Text.Trim(), @"^[А-ЯЁ][а-яё]*$") && Regex.IsMatch(tbxName.Text.Trim(), @"^[А-ЯЁ][а-яё]*$")) readerName_ismatch = true;
                else
                {
                    MessageBox.Show("\nНеправильно введено ФИО. Фамилия и имя должны начинаться с заглавной буквы.");
                    return;
                };
            } else
            {
                if (Regex.IsMatch(tbxSurname.Text.Trim(), @"^[А-ЯЁ][а-яё]*$") && Regex.IsMatch(tbxName.Text.Trim(), @"^[А-ЯЁ][а-яё]*$") && Regex.IsMatch(tbxPatronymic.Text.Trim(), @"^[А-ЯЁ][а-яё]*$")) readerName_ismatch = true;
                else
                {
                    MessageBox.Show("\nНеправильно введено ФИО. Фамилия, имя и отчествво должны начинаться с заглавной буквы.");
                    return;
                };
            }

            if (Regex.IsMatch(tbxBirthday.Text, @"^(19|20)?[0-9]{2}[-](0?[1-9]|1[012])[-](0?[1-9]|[12][0-9]|3[01])$")) date_ismatch = true;
            else
            {
                MessageBox.Show("\nНеправильно введена дата рождения. Дата должна быть в формате \"ГГГГ-ММ-ДД\"");
                return;
            };

            if (cmbEducation.SelectedIndex == 0)
            {             
                string birthday = tbxBirthday.Text;
                CalcAge(birthday);                                         
                int id_education = int.Parse(BasicFunction.Select($"SELECT id_education FROM Education WHERE name = '{cmbEducation.SelectedItem}'").Rows[0][0].ToString());               
                if (tbxName.Text.Length > 0 && tbxSurname.Text.Length > 0 && tbxAddress.Text.Length > 0 
                    && tbxBirthday.Text.Length > 0 && tbxPhone.Text.Length > 0 && date_ismatch && readerName_ismatch)
                {
                    BasicFunction.Select("INSERT INTO Reading_Card(surname, name, patronymic, phone, address, birthdate, place_of_study, сlass, age, id_education) VALUES" + $"('{tbxSurname.Text}', '{tbxName.Text}', '{tbxPatronymic.Text}', '{tbxPhone.Text}', '{tbxAddress.Text}', '{tbxBirthday.Text}', NULL, NULL , {ageReader}, {id_education})");
                    MessageBox.Show("Читатель успешно зарегистрирован в библиотеке!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    Clear();
                }
            }
            else
            {
                if (tbxName.Text.Length > 0 && tbxSurname.Text.Length > 0 && tbxAddress.Text.Length > 0
                    && tbxBirthday.Text.Length > 0 && tbxPhone.Text.Length > 0 && tbxClass.Text.Length > 0
                    && tbxPlaceOfStudy.Text.Length > 0 && date_ismatch && readerName_ismatch)
                {
                    string birthday = tbxBirthday.Text;
                    CalcAge(birthday);
                    int age = ageReader;
                    int id_education = int.Parse(BasicFunction.Select($"SELECT id_education FROM Education WHERE name = '{cmbEducation.SelectedItem}'").Rows[0][0].ToString());
                    BasicFunction.Select("INSERT INTO Reading_Card VALUES" + $"('{tbxSurname.Text}', '{tbxName.Text}', '{tbxPatronymic.Text}', '{tbxPhone.Text}', '{tbxAddress.Text}', '{tbxBirthday.Text}', '{tbxPlaceOfStudy.Text}', '{tbxClass.Text}', {ageReader} , {id_education})");
                    MessageBox.Show("Читатель успешно зарегистрирован в библиотеке!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    Clear();
                }                
            }

            if (tbxPatronymic.Text.Length < 0)
            {
                if (cmbEducation.SelectedIndex == 0)
                {                  
                    string birthday = tbxBirthday.Text;
                    CalcAge(birthday);
                    int id_education = int.Parse(BasicFunction.Select($"SELECT id_education FROM Education WHERE name = '{cmbEducation.SelectedItem}'").Rows[0][0].ToString());
                    if (tbxName.Text.Length > 0 && tbxSurname.Text.Length > 0 && tbxAddress.Text.Length > 0 
                        && tbxBirthday.Text.Length > 0 && tbxPhone.Text.Length > 0 && date_ismatch && readerName_ismatch)
                    {
                        BasicFunction.Select("INSERT INTO Reading_Card(surname, name, patronymic, phone, address, birthdate, place_of_study, сlass, age, id_education) VALUES" + $"('{tbxSurname.Text}', '{tbxName.Text}', NULL , '{tbxPhone.Text}', '{tbxAddress.Text}', '{tbxBirthday.Text}', NULL, NULL , {ageReader}, {id_education})");
                        MessageBox.Show("Читатель успешно зарегистрирован в библиотеке!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        Clear();
                    }
                }
                else
                {
                    if (tbxName.Text.Length > 0 && tbxSurname.Text.Length > 0 && tbxAddress.Text.Length > 0
                        && tbxBirthday.Text.Length > 0 && tbxPhone.Text.Length > 0 && tbxClass.Text.Length > 0
                        && tbxPlaceOfStudy.Text.Length > 0 && date_ismatch && readerName_ismatch)
                    {
                        string birthday = tbxBirthday.Text;
                        CalcAge(birthday);                       
                        int id_education = int.Parse(BasicFunction.Select($"SELECT id_education FROM Education WHERE name = '{cmbEducation.SelectedItem}'").Rows[0][0].ToString());
                        BasicFunction.Select("INSERT INTO Reading_Card VALUES" + $"('{tbxSurname.Text}', '{tbxName.Text}', NULL, '{tbxPhone.Text}', '{tbxAddress.Text}', '{tbxBirthday.Text}', '{tbxPlaceOfStudy.Text}', '{tbxClass.Text}', {ageReader} , {id_education})");
                        MessageBox.Show("Читатель успешно зарегистрирован в библиотеке!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        Clear();
                    }
                }
            }
            
        }

        private void cmbEducation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbEducation.SelectedIndex == 0)
            {
                tbxClass.Clear();
                tbxPlaceOfStudy.Clear();
                tbxClass.IsEnabled = false;
                tbxPlaceOfStudy.IsEnabled = false;
            } 
            else
            {
                tbxClass.IsEnabled = true;
                tbxPlaceOfStudy.IsEnabled = true;
            }
        }

        private void tbxSurname_TextChanged(object sender, TextChangedEventArgs e)
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

        private void tbxName_TextChanged(object sender, TextChangedEventArgs e)
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

        private void tbxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbxPhone.MaxLength = 11;
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

        private void tbxAddress_TextChanged(object sender, TextChangedEventArgs e)
        {           
            TextBox textBox = sender as TextBox;
            char[] charList = textBox.Text.ToCharArray();
            for (int i = 0; i < charList.Length; i++)
            {
                if (!BasicFunction.validateAddress(charList[i].ToString()))
                {
                    textBox.Text = textBox.Text.Remove(i, 1);
                }
            }
        }

        private void tbxBirthday_TextChanged(object sender, TextChangedEventArgs e)
        {
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

        private void tbxPlaceOfStudy_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            char[] charList = textBox.Text.ToCharArray();
            for (int i = 0; i < charList.Length; i++)
            {
                if (!BasicFunction.validateAddress(charList[i].ToString()))
                {
                    textBox.Text = textBox.Text.Remove(i, 1);
                }
            }
        }

        private void tbxClass_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbxPhone.MaxLength = 2;
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

        private void tbxPatronymic_TextChanged(object sender, TextChangedEventArgs e)
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
