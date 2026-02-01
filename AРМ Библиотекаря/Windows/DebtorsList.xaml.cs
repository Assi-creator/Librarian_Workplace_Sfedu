
using Microsoft.Win32;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocToPDFConverter;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using static AРМ_Библиотекаря.Classes.Variables;
using static System.Net.Mime.MediaTypeNames;

namespace AРМ_Библиотекаря
{
    /// <summary>
    /// Логика взаимодействия для DebtorsList.xaml
    /// </summary>
    public partial class DebtorsList : Window
    {
        public ObservableCollection<IssueBook> IssueList { get; set; } = new ObservableCollection<IssueBook>();
        public DebtorsList()
        {
            InitializeComponent(); 
            LoadDebtorsList();
        }
                                    
        void LoadDebtorsList()
        {
            IssueList.Clear();
            System.Data.DataTable dataDebtors = BasicFunction.Select("SELECT * FROM IssueBook WHERE condition = 'X' ORDER BY date_pass ASC ");


            for (int i = 0; i < dataDebtors.Rows.Count; i++)
            {
                string tempSurname = BasicFunction.Select($"SELECT surname FROM Reading_Card WHERE id_readingCard IN (SELECT id_readingCard FROM IssueBook WHERE id_book = {dataDebtors.Rows[i][0]})").Rows[0][0].ToString();
                string tempName = BasicFunction.Select($"SELECT name FROM Reading_Card WHERE id_readingCard IN (SELECT id_readingCard FROM IssueBook WHERE id_book = {dataDebtors.Rows[i][0]})").Rows[0][0].ToString();
                string tempPatronymic = BasicFunction.Select($"SELECT patronymic FROM Reading_Card WHERE id_readingCard IN (SELECT id_readingCard FROM IssueBook WHERE id_book = {dataDebtors.Rows[i][0]})").Rows[0][0].ToString();
                string tempBookName = BasicFunction.Select($"SELECT name FROM Book WHERE id_book IN (SELECT id_book FROM IssueBook WHERE id_book = {dataDebtors.Rows[i][0]})").Rows[0][0].ToString();
                

                IssueBook issueBook = new IssueBook()
                {
                    NumberReaderCard = dataDebtors.Rows[i][1].ToString(),
                    ReaderName = tempSurname + " " + tempName + " " + tempPatronymic,                   
                    BookName = tempBookName,
                    DataIssue = dataDebtors.Rows[i][2].ToString().Replace("0:00:00", "").Trim(),
                    DataPass = dataDebtors.Rows[i][3].ToString().Replace("0:00:00", "").Trim(),
                    Condition = dataDebtors.Rows[i][4].ToString(),
                };
                IssueList.Add(issueBook);
            }
            dtgDebtorsList.ItemsSource = IssueList;
        }
    

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }   
       
        private void miCloseDebtors_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Хотите закрыть задолжность?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                IssueBook readerItem = dtgDebtorsList.SelectedItem as IssueBook;
                if (readerItem == null)
                {
                    System.Windows.MessageBox.Show("Выберите строку из таблицы!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    BasicFunction.Select($"UPDATE Book SET is_enable = 'V' WHERE name = '{readerItem.BookName}'");
                    BasicFunction.Select($"DELETE FROM IssueBook WHERE id_readingCard = {readerItem.NumberReaderCard}");
                    System.Windows.MessageBox.Show("Задолжность успешно закрыта!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadDebtorsList();
                }
            }
        }

        private void miCreateWord_Click(object sender, RoutedEventArgs e)
        {
            DateTime now = DateTime.Now;
            IssueBook debtors_info = dtgDebtorsList.SelectedItem as IssueBook;
            string name_book = debtors_info.BookName;
            string date_issue = debtors_info.DataIssue;
            string date_pass = debtors_info.DataPass;

            WordDocument document = new WordDocument();         
            WSection section = document.AddSection() as WSection;           
            section.PageSetup.Margins.All = 72;           
            section.PageSetup.PageSize = new SizeF(612, 792);

            
            WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
            style.CharacterFormat.FontName = "Times New Roman";
            style.CharacterFormat.FontSize = 11f;
            style.ParagraphFormat.BeforeSpacing = 0;
            style.ParagraphFormat.AfterSpacing = 8;
            style.ParagraphFormat.LineSpacing = 13.8f;

            style = document.AddParagraphStyle("Heading 1") as WParagraphStyle;
            style.ApplyBaseStyle("Normal");
            style.CharacterFormat.FontName = "Times New Roman";
            style.CharacterFormat.FontSize = 16f;
            style.CharacterFormat.TextColor = System.Drawing.Color.FromArgb(46, 116, 181);
            style.ParagraphFormat.BeforeSpacing = 12;
            style.ParagraphFormat.AfterSpacing = 0;
            style.ParagraphFormat.Keep = true;
            style.ParagraphFormat.KeepFollow = true;
            style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;
            IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();
         
            paragraph.ApplyStyle("Normal");
            paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
            WTextRange textRange = paragraph.AppendText("Детская городская библиотека №5 им. Л. Толстого") as WTextRange;
            textRange.CharacterFormat.FontSize = 12f;
            textRange.CharacterFormat.FontName = "Times New Roman";
            textRange.CharacterFormat.TextColor = System.Drawing.Color.Black;
        
            paragraph = section.AddParagraph();
            paragraph.ApplyStyle("Heading 1");
            paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;
            textRange = paragraph.AppendText("Уважаемый читатель!") as WTextRange;
            textRange.CharacterFormat.FontSize = 18f;
            textRange.CharacterFormat.FontName = "Times New Roman";
          
            paragraph = section.AddParagraph();
            paragraph.ParagraphFormat.FirstLineIndent = 36;
            paragraph.BreakCharacterFormat.FontSize = 12f;
            textRange = paragraph.AppendText($"Уведомляем о том, что по состоянию на {now} у Вас имеется задолженность перед библиотекой в виде экземпляра книги {name_book} взятой на срок с {date_issue} по {date_pass}.") as WTextRange;
            textRange.CharacterFormat.FontSize = 12f;
            textRange.CharacterFormat.FontName = "Times New Roman";
           
            paragraph = section.AddParagraph();
            paragraph.ParagraphFormat.FirstLineIndent = 36;
            paragraph.BreakCharacterFormat.FontSize = 12f;
            textRange = paragraph.AppendText("Просим Вас вернуть данный экземпляр в библиотечный фонд в течение недели.") as WTextRange;
            textRange.CharacterFormat.FontSize = 12f;
            textRange.CharacterFormat.FontName = "Times New Roman";

            paragraph = section.AddParagraph();
            paragraph.ApplyStyle("Heading 1");
            paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
            textRange = paragraph.AppendText("Администрация библиотеки") as WTextRange;
            textRange.CharacterFormat.FontSize = 16f;
            textRange.CharacterFormat.FontName = "Times New Roman";

            document.Save("Извещение.docx");

            if (System.Windows.MessageBox.Show("Извещение успешно сохранено в Word-файл! Не желаете конвертировать его в PDF-формат?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {               
                WordDocument wordDocument = new WordDocument("Извещение.docx", FormatType.Docx);              
                DocToPDFConverter converter = new DocToPDFConverter();               
                PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);             
                pdfDocument.Save("Извещение.pdf");               
                pdfDocument.Close(true);
                wordDocument.Close();
                System.Windows.MessageBox.Show("Извещение успешно сохранено в PDF-формат!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void miCreateExcel_Click(object sender, RoutedEventArgs e)
        {        
            DataGrid dataGrid = new DataGrid();
            dataGrid.DataSource = IssueList;          

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2013;
            IWorkbook workbook = application.Workbooks.Create();
            IWorksheet worksheet = workbook.Worksheets[0];
          
            worksheet.ImportDataGrid(dataGrid, 1, 1, true, true);           
            worksheet.ShowColumn(7, false);
            worksheet.ShowColumn(4, false);

            worksheet.Range["A2:A20"].ColumnWidth = 15;
            worksheet.Range["B2:B20"].ColumnWidth = 31;
            worksheet.Range["C2:C20"].ColumnWidth = 40;
            worksheet.Range["E2:E20"].ColumnWidth = 15;
            worksheet.Range["F2:F20"].ColumnWidth = 15;
                    
            workbook.SaveAs("Таблица задолжников.xlsx");
            workbook.Close();
            excelEngine.Dispose();
            if (System.Windows.MessageBox.Show("Таблица задолжников успешно сохранена в Excel-файле! Не желаете конвертировать её в CVS-формат?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ExcelEngine excelEngine1 = new ExcelEngine();
                IApplication application1 = excelEngine1.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                IWorkbook workbook1 = application1.Workbooks.Create();
                IWorksheet sheet = workbook1.Worksheets[0];
                sheet.ImportDataGrid(dataGrid, 1, 1, true, true);

                sheet.SaveAs("Таблица задолжников.csv", ",");                
                System.Windows.MessageBox.Show("Таблица задолжников успешно сохранена в CVS-формат!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void miPrintWord_Click(object sender, RoutedEventArgs e)
        {
            string documentPath = "/AРМ Библиотекаря\\bin\\Debug\\Извещение.docx";
            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            application.Visible = true;
            Microsoft.Office.Interop.Word.Document document = application.Documents.Open(documentPath);
            Microsoft.Office.Interop.Word.Dialog printDialog = application.Application.Dialogs[Microsoft.Office.Interop.Word.WdWordDialog.wdDialogFilePrint];
            if (printDialog.Show() == 1)
            {
                document.PrintOut();
            }
            document.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
            application.Quit();
        }
    }
    
}
