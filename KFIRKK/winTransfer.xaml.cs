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
    /// Interaction logic for winTransfer.xaml
    /// </summary>
    public partial class winTransfer : Window
    {
        public winTransfer()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblFrom.Content = "";
            lblTo.Content = "";
            lblFromAmount.Content = "";
            lblToAmount.Content = "";
            UpdateDatagrids();
            loaded = true;
            tbxFromC.Focus();

        }
        Database1Entities database = new Database1Entities();

        private void UpdateDatagrids()
        {
            dgTo.ItemsSource = database.Clients.ToArray();
            dgFrom.ItemsSource = database.Clients.ToArray();
            dgTo.SelectedItem = null;
            dgFrom.SelectedItem = null;
            
        }
        private void OnlyDigits_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;

        }
        bool loaded = false;
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        public bool update = false;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (dgFrom.SelectedItem != null && dgTo.SelectedItem != null)
            {
                double amount;
                if(double.TryParse(tbxAmount.Text, out amount))
                {
                    var cFrom = database.Clients.Where(c => c.Id == ((KFIRKK.Client)dgFrom.SelectedItem).Id).FirstOrDefault();
                    var cTo = database.Clients.Where(c => c.Id == ((KFIRKK.Client)dgTo.SelectedItem).Id).FirstOrDefault();
                    if (amount <= cFrom.Units && amount > 0)
                    {
                        cFrom.Units -= amount;
                        cFrom.TransferLogs.Add(new TransferLog
                        {
                            OtherClientName = cTo.Name,
                            TransferAmount = -amount,
                            Date = DateTime.Now,
                            TransferLogText = "נשלח ל-",
                            PointsAfterTransfer = cFrom.Units
                        });
                        cTo.Units += amount;
                        cTo.TransferLogs.Add(new TransferLog
                        {
                            OtherClientName = cFrom.Name,
                            TransferAmount = +amount,
                            Date = DateTime.Now,
                            TransferLogText = "התקבל מ-",
                            PointsAfterTransfer = cTo.Units
                        });
                        database.SaveChanges();
                        UpdateDatagrids();
                        update = true;
                        MessageBox.Show(string.Format("העברה מ - [{0}], בסך - {1} נק', עבור - [{2}], בוצע בהצלחה",cFrom.Name,amount,cTo.Name));
                        Clipboard.SetText(string.Format("העברה מ - [{0}], בסך - {1} נק', עבור - [{2}], בוצע בהצלחה", cFrom.Name, amount, cTo.Name));
                        tbxFromC.Text = "";
                        tbxToC.Text = "";
                        tbxFromC.Focus();
                    }
                    else
                    {
                        MessageBox.Show("כמות הנקודות לא נכונה!");
                        tbxAmount.Focus();
                        tbxAmount.SelectAll();
                    }
                }
            }
        }

        private void dgFrom_CurrentCellChanged(object sender, EventArgs e)
        {
        }

        private void dgFrom_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (loaded)
            {
                if(dgFrom.SelectedItem != null)
                {
                    lblFrom.Content = ((Client)(dgFrom.SelectedItem)).Name;
                    dgTo.ItemsSource = database.Clients.Where(c => c.Id != ((KFIRKK.Client)dgFrom.SelectedItem).Id).ToArray();
                    tbxAmount.Text = ((Client)(dgFrom.SelectedItem)).Units.ToString();
                    lblFromAmount.Content = tbxAmount.Text + "-";
                }
                else
                {
                    lblFrom.Content = string.Empty;
                    tbxAmount.Text = string.Empty;
                    lblFromAmount.Content = string.Empty;
                }
            }
        }

        private void dgTo_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(dgTo.SelectedItem != null)
            {
                lblTo.Content = ((Client)(dgTo.SelectedItem)).Name;
                lblToAmount.Content = "+" + tbxAmount.Text;
            }
            else
            {
                lblTo.Content = string.Empty;
                lblToAmount.Content = string.Empty;
            }
        }

        private void tbxAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            double textamount = 0;
            if (double.TryParse(tbxAmount.Text, out textamount))
            {
                if(textamount > ((Client)(dgFrom.SelectedItem)).Units)
                {
                    tbxAmount.Text = ((Client)(dgFrom.SelectedItem)).Units.ToString();
                    tbxAmount.SelectAll();
                }
            }
            if (!string.IsNullOrEmpty(lblTo.Content.ToString()))
            {
                lblToAmount.Content =  tbxAmount.Text + "+";
            }
            lblFromAmount.Content =  tbxAmount.Text + "-";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void label_Copy2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void tbxFromC_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxFromC.Text.Length > 0)
            {
                dgFrom.ItemsSource = database.Clients.Where(c => c.Name.Contains(tbxFromC.Text)).ToArray();
                if (dgFrom.HasItems)
                {
                    dgFrom.SelectedItem = dgFrom.Items[0];
                }
            }
            else
            {
                dgFrom.ItemsSource = database.Clients.ToArray();
            }
        }

        private void tbxToC_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxToC.Text.Length > 0)
            {
                dgTo.ItemsSource = database.Clients.Where(c => c.Name.Contains(tbxToC.Text)).ToArray();
                if (dgTo.HasItems)
                {
                    dgTo.SelectedItem = dgTo.Items[0];
                }
            }
            else
            {
                dgTo.ItemsSource = database.Clients.ToArray();
            }
        }

        private void tbxAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxAmount.SelectAll();
        }
    }
}
