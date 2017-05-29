using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KFIRKK
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        public struct SumShnati
        {
            public DateTime date { set; get; }
            public int t { set; get; }
            public decimal tprice { set; get; }
            public int p { set; get; }
            public decimal pprice { set; get; }
            public int m { set; get; }
            public decimal mprice { set; get; }
            public int n { set; get; }
            public decimal nprice { set; get; }
            public decimal credit { set; get; }
            public decimal cash { set; get; }
            public decimal alltogather { set; get; }
        }

        Database1Entities database = new Database1Entities();
        int year = DateTime.Now.Year;
        int lastSelectedItemId;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Open-close DohShnati
            if (Properties.Settings.Default.DohShnatiOpen) { Height = 755; } else { Height = 474.2; }

            //Reset Labels
            lblCurrClient.Content = "";
            lblClientName.Content = "";

            //UpdateDatagrids
            UPDATEYEARLY();
            UPDATEDATABASE();
        }
        private void UPDATEYEARLY()
        {
            using (Database1Entities database = new Database1Entities())
            {
                List<SumShnati> listDohShnati = new List<SumShnati>();
                for (int i = 0; i < 12; i++)
                {
                    var sDate = new DateTime(year, i + 1, 1);
                    var eDate = sDate.AddMonths(1).AddMinutes(-1);
                    var works = database.Works.Where(w => w.Date >= sDate && w.Date <= eDate).ToArray();
                    var wCredit = 0m;
                    var wCash = 0m;
                    foreach (var item in works)
                    {
                        //Tattoo
                        if (item.TattooPrice != null)
                        {
                            if (item.TattooPriceType != null)
                            {
                                if (item.TattooPriceType == false) { wCash += item.TattooPrice.Value; } else { wCredit += item.TattooPrice.Value; }
                            }
                            else
                            {
                                wCash += item.TattooPrice.Value;
                            }
                        }
                        //Piercing
                        if (item.PiercingPrice != null)
                        {
                            if (item.PiercingPriceType != null)
                            {
                                if (item.PiercingPriceType == false) { wCash += item.PiercingPrice.Value; } else { wCredit += item.PiercingPrice.Value; }
                            }
                            else
                            {
                                wCash += item.PiercingPrice.Value;
                            }
                        }

                        //Misha
                        if (item.MishaPrice != null)
                        {
                            if (item.MishaPriceType != null)
                            {
                                if (item.MishaPriceType == false) { wCash += item.MishaPrice.Value; } else { wCredit += item.MishaPrice.Value; }
                            }
                            else
                            {
                                wCash += item.MishaPrice.Value;
                            }
                        }
                        //Neckle
                        if (item.NecklePrice != null)
                        {
                            if (item.NecklePriceType != null)
                            {
                                if (item.NecklePriceType == false) { wCash += item.NecklePrice.Value; } else { wCredit += item.NecklePrice.Value; }
                            }
                            else
                            {
                                wCash += item.NecklePrice.Value;
                            }
                        }
                    }

                    //Add row to list
                    listDohShnati.Add(new SumShnati()
                    {
                        date = sDate,
                        t = works.Where(w => w.TattooCount != 0 && w.TattooCount != null).Select(w => (int)w.TattooCount).Sum(),
                        tprice = works.Where(w => w.TattooPrice != 0 && w.TattooPrice != null).Select(w => (int)w.TattooPrice).Sum(),
                        p = works.Where(w => w.PiercingCount != 0 && w.PiercingCount != null).Select(w => (int)w.PiercingCount).Sum(),
                        pprice = works.Where(w => w.PiercingPrice != 0 && w.PiercingPrice != null).Select(w => (int)w.PiercingPrice).Sum(),
                        m = works.Where(w => w.MishaCount != 0 && w.MishaCount != null).Select(w => (int)w.MishaCount).Sum(),
                        mprice = works.Where(w => w.MishaPrice != 0 && w.MishaPrice != null).Select(w => (int)w.MishaPrice).Sum(),
                        n = works.Where(w => w.NeckleCount != 0 && w.NeckleCount != null).Select(w => (int)w.NeckleCount).Sum(),
                        nprice = works.Where(w => w.NecklePrice != 0 && w.NecklePrice != null).Select(w => (int)w.NecklePrice).Sum(),
                        credit = wCredit,
                        cash = wCash,
                        alltogather = wCash + wCredit,
                    });
                }

                //Update datagrid
                dg.ItemsSource = listDohShnati.ToArray();
                dg.Items.Refresh();
            }
        }


        private void UPDATEDATABASE()
        {
            database = new Database1Entities();

            clientDataGrid.ItemsSource = database.Clients.ToArray();

            //Select last selected clientrow
            if(lastSelectedItemId > -1)
            {
                if (database.Clients.Where(c => c.Id == lastSelectedItemId).FirstOrDefault() != null)
                {
                    var item = ((KFIRKK.Client[])clientDataGrid.ItemsSource).Where(c => c.Id == lastSelectedItemId).FirstOrDefault();
                   clientDataGrid.SelectedItem = item;
                }
            }
            SortEveryting();
        }

        public static void SortDataGrid(DataGrid dataGrid, int columnIndex = 0, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            var column = dataGrid.Columns[columnIndex];

            // Clear current sort descriptions
            dataGrid.Items.SortDescriptions.Clear();

            // Add the new sort description
            dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, sortDirection));

            // Apply sort
            foreach (var col in dataGrid.Columns)
            {
                col.SortDirection = null;
            }
            column.SortDirection = sortDirection;

            // Refresh items to display sort
            dataGrid.Items.Refresh();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void clientDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            lblClients.Content = "סה''כ לקוחות : " + clientDataGrid.Items.Count;
        }

        private void workDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            lblWorks.Content = "סה''כ עבודות : " + workDataGrid.Items.Count;
        }

        private void KFIRTATTODATABASE_png_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            winAddClient win = new winAddClient();
            win.ShowDialog();
            if (win.updateMainDGs)
            {
                UPDATEDATABASE();
                UPDATEYEARLY();
            }

        }

        private void clientDataGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(clientDataGrid.SelectedItem != null)
            {
                winAddClient win = new winAddClient();
                win.clientid = ((Client)clientDataGrid.SelectedItem).Id;
                win.edit = true;
                win.ShowDialog();
                UPDATEDATABASE();
                UPDATEYEARLY();
            }
        }
        private void clientDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            winSettings win = new winSettings();
            win.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WorkDelete_Click(object sender, RoutedEventArgs e)
        {
            if(workDataGrid.SelectedItem != null)
            {
                if (MessageBox.Show("האם אתה בטוח שאתה רוצה למחוק?", "אזהרה", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (Database1Entities database = new Database1Entities())
                    {
                        var id = ((Work)workDataGrid.SelectedItem).Id;
                        Client c = database.Works.Where(w => w.Id == id).FirstOrDefault().Client;
                        var sum = database.Works.Where(w => w.Id == id).FirstOrDefault().PointsSum;
                        c.Units -= (double)sum;
                        database.Works.Remove(database.Works.Where(w => w.Id == id).FirstOrDefault());
                        database.SaveChanges();
                        UPDATEDATABASE();
                        UPDATEYEARLY();
                    }
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("האם אתה בטוח שאתה רוצה למחוק?", "אזהרה", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (clientDataGrid.SelectedItem != null)
                {
                    int id = ((Client)clientDataGrid.SelectedItem).Id;
                    using (Database1Entities database = new Database1Entities())
                    {
                        var c = database.Clients.Where(w => w.Id == id).FirstOrDefault();
                        database.Logs.RemoveRange(c.Logs);
                        database.Works.RemoveRange(c.Works);
                        database.TransferLogs.RemoveRange(c.TransferLogs);
                        database.Clients.Remove(c);
                        
                        database.SaveChanges();
                        UPDATEDATABASE();
                        UPDATEYEARLY();
                    }
                }
            }
        }
        private void OpenTransferWindow()
        {
            winTransfer win = new winTransfer();
            win.ShowDialog();
            if (win.update)
            {
                UPDATEDATABASE();
            }
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            OpenTransferWindow();
        }

        private void transfer_png_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenTransferWindow();
        }
        private void clientDataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (clientDataGrid.SelectedItem != null)
            {
                
                lblCurrClient.Content = "לקוח נוכחי";
                lblClientName.Content = ((KFIRKK.Client)clientDataGrid.SelectedItem).Name;
                lastSelectedItemId = ((KFIRKK.Client)clientDataGrid.SelectedItem).Id;
                workDataGrid.ItemsSource = ((KFIRKK.Client)clientDataGrid.SelectedItem).Works.ToArray();
                transferLogDataGrid.ItemsSource = ((KFIRKK.Client)clientDataGrid.SelectedItem).TransferLogs.ToArray();
            }
            else
            {
                lblCurrClient.Content = "";
                lblClientName.Content = "";
                workDataGrid.ItemsSource = "";
                transferLogDataGrid.ItemsSource = "";
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void clientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //if (transferLogViewSource != null)
            //{
            //    if (transferLogViewSource.View != null)
            //    {
            //        transferLogViewSource.View.MoveCurrentToLast();
            //        SortDataGrid(transferLogDataGrid, 2, ListSortDirection.Descending);
            //    }
            //}
            //if (clientWorkViewSource != null)
            //{
            //    if (clientWorkViewSource.View != null)
            //    {
            //        clientWorkViewSource.View.MoveCurrentToLast();
            //        SortDataGrid(workDataGrid, 0, ListSortDirection.Descending);

            //    }
            //}
        }

        private void dg_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(dg.SelectedItem != null)
            {
                winMonth win = new winMonth(((SumShnati)dg.SelectedItem).date);
                win.ShowDialog();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.N))
            {
                winAddClient win = new winAddClient();
                win.ShowDialog();
                if (win.updateMainDGs)
                {
                    UPDATEDATABASE();
                    UPDATEYEARLY();
                }
            }
        }

        private void label_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void SortEveryting()
        {
            SortDataGrid(clientDataGrid, 0, ListSortDirection.Descending);
            SortDataGrid(workDataGrid, 0, ListSortDirection.Descending);
            SortDataGrid(transferLogDataGrid, 2, ListSortDirection.Descending);
        }
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox.Text.Contains("חיפוש לקוח.."))
            {
                clientDataGrid.ItemsSource = database.Clients.ToArray();

            }
            else
            {
                clientDataGrid.ItemsSource = database.Clients.Where(c =>c.Name.Contains(textBox.Text)).ToArray();
            }
            SortEveryting();
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text)) 
            {
                textBox.Text = "חיפוש לקוח..";
            }
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(textBox.Text.Contains("חיפוש לקוח.."))
            {
                textBox.Text = "";
            }
        }

    }
    public class AddConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            decimal[] array = new decimal[4];
            decimal.TryParse(values[0].ToString(), out array[0]);
            decimal.TryParse(values[1].ToString(), out array[1]);
            decimal.TryParse(values[2].ToString(), out array[2]);
            decimal.TryParse(values[3].ToString(), out array[3]);
            decimal result = array.Sum();
            return result.ToString("#,#.00;;#");
        }
        public object[] ConvertBack(object value, Type[] targetTypes,
               object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }

    public class CashCreditConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool v;
            if(value == null)
            {
                return "";
            }
            if(bool.TryParse(value.ToString(), out v))
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
}

