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
    /// Логика взаимодействия для Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        public Statistics()
        {
            InitializeComponent();
            DataTable sub1 = BasicFunction.Select("SELECT age FROM Reading_Card");
            DataTable sub2 = BasicFunction.Select("SELECT age FROM Reading_Card");
            DataTable sub3 = BasicFunction.Select("SELECT age FROM Reading_Card");
            int countSub1 = sub1.Rows.Count;
            int countSub2 = sub2.Rows.Count;
            int countSub3 = sub3.Rows.Count;
            DataTable sub = BasicFunction.Select("SELECT surname FROM Reading_Card");
            double[] values = { countSub1, countSub2, countSub3 };
            double[] position = { 0, 1, 2 };
            string[] labels = { (string)sub.Rows[0][0], (string)sub.Rows[1][0], (string)sub.Rows[2][0], (string)sub.Rows[3][0] };
            plotAbonim.Plot.AddBar(values, position);
            plotAbonim.Plot.XTicks(position, labels);
            plotAbonim.Plot.SetAxisLimits(yMin: 0);
            plotAbonim.Refresh();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
