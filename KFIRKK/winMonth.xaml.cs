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

namespace KFIRKK
{
    /// <summary>
    /// Interaction logic for winMonth.xaml
    /// </summary>
    public partial class winMonth : Window
    {
        DateTime datetime;
        public winMonth(DateTime datetime)
        {
            InitializeComponent();
            this.datetime = datetime;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDatagird();
        }
        private void UpdateDatagird()
        {
            lblmonth.Content = datetime.ToString("MMMM , yyyy");
            DateTime startdate = new DateTime(datetime.Year, datetime.Month, 1);
            DateTime enddate = startdate.AddMonths(1);
            using (Database1Entities database = new Database1Entities())
            {
                var items = database.Works.Where(w => w.Date >= startdate && w.Date <= enddate).ToArray().OrderBy(w => w.Date);
                workDataGrid.ItemsSource = items;
                if(items.Count() > 0)
                {
                    lblSum.Content = string.Format("• קעקועים:   -   {0}\n• פירסינג:   -   {1}\n• משחות:   -   {2}\n• תכשיטים:   -   {3}\n• אשראי:   -   {4}\n• מזומן:   -   {5}\n• סה''כ:   -   {6}"/*"סה''כ : קעקועים {0}, פירסינג {1}, משחות {2}, תכשיטים {3},   אשראי : {4}    -    מזומן : {5}    -    סה''כ: {6}"*/,
                        items.Select(w => w.TattooCount).Sum(),
                        items.Select(w => w.PiercingCount).Sum(),
                        items.Select(w => w.MishaCount).Sum(),
                        items.Select(w => w.NeckleCount).Sum(),
                        items.Select(w => w.Credit).Sum().Value.ToString("#,#.00;;#"),
                        items.Select(w => w.Cash).Sum().Value.ToString("#,#.00;;#"),
                        items.Select(w => w.Sum).Sum().Value.ToString("#,#.00;;#")
                        );
                }
                else
                {
                    lblSum.Content = "";
                }
            }
        }
        public class CashCreditConvertor : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                bool v;
                if (bool.TryParse(value.ToString(), out v))
                {
                    if (v)
                    {
                        return "אשראי";
                    }
                    else
                    {
                        return "מזומן";
                    }
                }
                else
                {
                    return "";
                }

            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Add || e.Key == Key.OemPlus || e.Key == Key.Right)
            {
                datetime = datetime.AddMonths(1);
                UpdateDatagird();
            }
            if (e.Key == Key.Subtract || e.Key == Key.OemMinus || e.Key == Key.Left)
            {
                datetime = datetime.AddMonths(-1);
                UpdateDatagird();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
