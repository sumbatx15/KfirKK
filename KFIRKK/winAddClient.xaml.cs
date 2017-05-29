using KFIRKK.Properties;
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
using System.Text.RegularExpressions;

namespace KFIRKK
{
    /// <summary>
    /// Interaction logic for winAddClient.xaml
    /// </summary>
    public partial class winAddClient : Window
    {
        bool loaded = false;
        public bool edit = false;
        public int clientid;
        Client newClient = new Client();
        Database1Entities database = new Database1Entities();
        public winAddClient()
        {
            InitializeComponent();
            loaded = true;

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        string[] creditCashToString = new string[] { "מזומן", "אשראי" };
        public bool updateMainDGs = false;
        string defaultName;
        private void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            #region CheckIfAnyWorkAdded
            int testWorkIsAdded;

            bool WorkIsAdded = false;

            TextBox[] tbxes = new TextBox[]
            {
                tbxTattooCount,
                tbxTattooPrice,
                tbxPiercingCount,
                tbxPiercingPrice,
                tbxMishaCount,
                tbxMishaPrice,
                tbxNeckleCount,
                tbxNecklePrice
            };


            foreach (var tbx in tbxes)
            {
                if (int.TryParse(tbx.Text, out testWorkIsAdded))
                {
                    WorkIsAdded = true;
                    updateMainDGs = true;
                    break;
                }
            }
            #endregion

            if (edit)
            {
                if (string.IsNullOrEmpty(tbxName.Text) || tbxName.Text.Contains("שם מלא"))
                {
                    tbxName.Focus();
                }
                else
                {

                    newClient.Name = tbxName.Text.Trim();
                    if (!string.IsNullOrEmpty(tbxPhone.Text) || !tbxPhone.Text.Contains("פלאפון"))
                    {
                        newClient.Phone = tbxPhone.Text.Trim();
                    }

                    if (WorkIsAdded)
                    {
                        AddWorkToClient(newClient);
                        database.SaveChanges();
                        WorkIsAdded = false;
                        Close();
                    }
                    else
                    {
                        database.SaveChanges();
                        Close();
                    }
                }

            }
            else
            {
                Client c = new Client()
                {
                    Name = tbxName.Text.Trim(),
                    InActive = true
                };
                if (!tbxPhone.Text.Contains("פלאפון"))
                {
                    c.Phone = tbxPhone.Text.Trim();
                }

                if (WorkIsAdded)
                {
                    if (string.IsNullOrEmpty(tbxName.Text) || tbxName.Text.Contains("שם מלא"))
                    {
                        tbxName.Focus();
                    }
                    else
                    {
                        AddWorkToClient(c);

                        database.Clients.Add(c);
                        database.SaveChanges();
                        WorkIsAdded = false;
                        Close();
                    }

                }
                else
                {
                    database.Clients.Add(c);
                    database.SaveChanges();
                    WorkIsAdded = false;
                    Close();
                }
            }
            
        }

        private void AddWorkToClient(Client c)
        {
            Work w = new Work();
            w.Cash = 0;
            w.Credit = 0;
            w.Name = c.Name;
            w.Date = dpdate.SelectedDate;
            w.Description = tbxDescription.Text;

            if(w.TattooPricePoints == null) { w.TattooPricePoints = 0; }
            if(w.PiercingPricePoints == null) { w.PiercingPricePoints = 0; }
            if(w.MishaPricePoints == null) { w.MishaPricePoints = 0; }
            if(w.NecklePricePoints == null) { w.NecklePricePoints = 0; }
            if(w.PointsSum == null) { w.PointsSum = 0; }

            #region CheckTextBoxesAndAddWork
            if (!string.IsNullOrEmpty(tbxTattooPrice.Text))
            {
                w.TattooPrice = decimal.Parse(tbxTattooPrice.Text);
                w.TattooPricePoints = (w.TattooPrice / 100) * Settings.Default.Precent;
                if (!string.IsNullOrEmpty(tbxTattooCount.Text))
                {
                    w.TattooCount = int.Parse(tbxTattooCount.Text);
                }
                else
                {
                    w.TattooCount = 1;
                }

                if (tbxTattooPriceType.Text.Contains("מזומן"))
                {
                    w.Cash += decimal.Parse(tbxTattooPrice.Text);
                    w.TattooPriceType = false;
                }
                else
                {
                    w.Credit += decimal.Parse(tbxTattooPrice.Text);
                    w.TattooPriceType = true;
                }
            }
            else if (!string.IsNullOrEmpty(tbxTattooCount.Text))
            {
                w.TattooCount = int.Parse(tbxTattooCount.Text);
            }

            if (!string.IsNullOrEmpty(tbxPiercingPrice.Text))
            {
                w.PiercingPrice = decimal.Parse(tbxPiercingPrice.Text);
                w.PiercingPricePoints = (w.PiercingPrice / 100) * Settings.Default.Precent;

                if (!string.IsNullOrEmpty(tbxPiercingCount.Text))
                {
                    w.PiercingCount = int.Parse(tbxPiercingCount.Text);
                }
                else
                {
                    w.PiercingCount = 1;
                }

                if (tbxPiercingPriceType.Text.Contains("מזומן"))
                {
                    w.Cash += decimal.Parse(tbxPiercingPrice.Text);
                    w.PiercingPriceType = false;
                }
                else
                {
                    w.Credit += decimal.Parse(tbxPiercingPrice.Text);
                    w.PiercingPriceType = true;
                }
            }
            else if (!string.IsNullOrEmpty(tbxPiercingCount.Text))
            {
                w.PiercingCount = int.Parse(tbxPiercingCount.Text);
            }


            if (!string.IsNullOrEmpty(tbxMishaPrice.Text))
            {
                w.MishaPrice = decimal.Parse(tbxMishaPrice.Text);
               // w.MishaPricePoints = (w.MishaPrice / 100) * Settings.Default.Precent;

                if (!string.IsNullOrEmpty(tbxMishaCount.Text))
                {
                    w.MishaCount = int.Parse(tbxMishaCount.Text);
                }
                else
                {
                    w.MishaCount = 1;
                }
                if (tbxMishaPriceType.Text.Contains("מזומן"))
                {
                    w.Cash += decimal.Parse(tbxMishaPrice.Text);
                    w.MishaPriceType = false;
                }
                else
                {
                    w.Credit += decimal.Parse(tbxMishaPrice.Text);
                    w.MishaPriceType = true;
                }
            }
            else if (!string.IsNullOrEmpty(tbxMishaCount.Text))
            {
                w.MishaCount = int.Parse(tbxMishaCount.Text);
            }


            if (!string.IsNullOrEmpty(tbxNecklePrice.Text))
            {
                w.NecklePrice = decimal.Parse(tbxNecklePrice.Text);
               // w.NecklePricePoints = (w.NecklePrice / 100) * Settings.Default.Precent;

                if (!string.IsNullOrEmpty(tbxNeckleCount.Text))
                {
                    w.NeckleCount = int.Parse(tbxNeckleCount.Text);
                }
                else
                {
                    w.NeckleCount = 1;
                }

                if (tbxNecklePriceType.Text.Contains("מזומן"))
                {
                    w.Cash += decimal.Parse(tbxNecklePrice.Text);
                    w.NecklePriceType = false;
                }
                else
                {
                    w.Credit += decimal.Parse(tbxNecklePrice.Text);
                    w.NecklePriceType = true;
                }
            }
            else if (!string.IsNullOrEmpty(tbxNeckleCount.Text))
            {
                w.NeckleCount = int.Parse(tbxNeckleCount.Text);
            }
            w.Description = tbxDescription.Text;
            #endregion

            w.Sum = w.Cash + w.Credit;
            if(c.Units == null)
            {
                c.Units = 0;
            }
            w.PointsSum += (double)(w.TattooPricePoints + w.PiercingPricePoints + w.MishaPricePoints + w.NecklePricePoints);
            c.Units += w.PointsSum;
            c.TransferLogs.Add(new TransferLog
             {
                 OtherClientName = "עצמי - רכישה",
                 TransferAmount = (double)w.PointsSum,
                 Date = DateTime.Now,
                 TransferLogText = "התקבל",
                 PointsAfterTransfer = (double)c.Units
            });
            
            c.Works.Add(w);
        }

        private void PriceSummary()
        {
            int tprice, nprice, mprice, pprice, cash = 0;
            double card = 0;

            if (tbxPiercingPrice.Text.Length > 0)
            {
                pprice = int.Parse(tbxPiercingPrice.Text);
                if (tbxPiercingPriceType.Text.Contains("מזומן")) { cash += pprice; } else { card += pprice; }
            }
            else
            {
                pprice = 0;
            }
            if (tbxNecklePrice.Text.Length > 0)
            {
                nprice = int.Parse(tbxNecklePrice.Text);
                if (tbxNecklePriceType.Text.Contains("מזומן")) { cash += nprice; } else { card += nprice; }
            }
            else
            {
                nprice = 0;
            }
            if (tbxTattooPrice.Text.Length > 0)
            {
                tprice = int.Parse(tbxTattooPrice.Text);
                if (tbxTattooPriceType.Text.Contains("מזומן")) { cash += tprice; } else { card += tprice; }
            }
            else
            {
                tprice = 0;
            }
            if (tbxMishaPrice.Text.Length > 0)
            {
                mprice = int.Parse(tbxMishaPrice.Text);
                if (tbxMishaPriceType.Text.Contains("מזומן")) { cash += mprice; } else { card += mprice; }
            }
            else
            {
                mprice = 0;
            }

            // card += Math.Floor((card / 100) * 2.5);
            lblSum.Content = "לתשלום - מזומן: " + cash.ToString() + "₪ | אשראי: " + card.ToString() + "₪";

        }
        private void tbxTattoPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;

        }

        #region AddingWorks

        private void tbxTattooPriceType_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var tbx = (TextBox)sender;
            tbx.Select(0, 0);
            if (tbx.Text == creditCashToString[0])
            {
                tbx.Text = creditCashToString[1];
            }
            else
            {
                tbx.Text = creditCashToString[0];
            }
        }
        private void lblNeckle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxNeckleCount.Text))
            {
                tbxNeckleCount.Text = (int.Parse(tbxNeckleCount.Text) + 1).ToString();
            }
            else
            {
                tbxNeckleCount.Text = "1";
            }
            tbxNecklePrice.Focus();
            tbxNecklePrice.SelectAll();
        }

        private void lblXtatto_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tbxTattooCount.Text = "";
            tbxTattooPrice.Text = "";
            tbxTattooPriceType.Text = creditCashToString[0];
        }

        private void lblPiercing_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxPiercingCount.Text))
            {
                tbxPiercingCount.Text = (int.Parse(tbxPiercingCount.Text) + 1).ToString();
                tbxPiercingPrice.Text = 100.ToString();
            }
            else
            {
                tbxPiercingCount.Text = "1";
                tbxPiercingPrice.Text = 100.ToString();
            
            }
            tbxPiercingPrice.Focus();
            tbxPiercingPrice.SelectAll();
        }

        private void lblXPiercing_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tbxPiercingCount.Text = "";
            tbxPiercingPrice.Text = "";
            tbxPiercingPriceType.Text = creditCashToString[0];
        }

        private void lblMisha_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxMishaCount.Text))
            {
                tbxMishaCount.Text = (int.Parse(tbxMishaCount.Text) + 1).ToString();
                tbxMishaPrice.Text = (int.Parse(tbxMishaCount.Text) * 40).ToString();
            }
            else
            {
                tbxMishaCount.Text = "1";
                tbxMishaPrice.Text = "40";
            }
            tbxMishaPrice.Focus();
            tbxMishaPrice.SelectAll();
        }

        private void lblXMisha_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tbxMishaCount.Text = "";
            tbxMishaPrice.Text = "";
            tbxMishaPriceType.Text = creditCashToString[0];
        }

        private void lblTattoo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxTattooCount.Text))
            {
                tbxTattooCount.Text = (int.Parse(tbxTattooCount.Text) + 1).ToString();
            }
            else
            {
                tbxTattooCount.Text = "1";
            }
            tbxTattooPrice.Focus();
            tbxTattooPrice.SelectAll();
        }

        private void lblXNeckle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            tbxNeckleCount.Text = "";
            tbxNecklePrice.Text = "";
            tbxNecklePriceType.Text = creditCashToString[0];
        }
        #endregion

        private void label1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void tbxName_GotFocus(object sender, RoutedEventArgs e)
        {
            if(tbxName.Text.Contains("שם מלא"))
            {
                tbxName.Text = "";
            }
        }

        private void tbxName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxName.Text))
            {
                tbxName.Text = "שם מלא";
            }
            else
            {
                tbxName.Text = tbxName.Text.Trim();
            }
        }

        private void tbxPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbxPhone.Text.Contains("פלאפון"))
            {
                tbxPhone.Text = "";
            }
        }

        private void tbxPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxPhone.Text))
            {
                tbxPhone.Text = "פלאפון";
            }
            else
            {
                tbxPhone.Text = tbxPhone.Text.Trim();
            }
        }

        private void tbxTattooPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Text = Regex.Replace(((TextBox)sender).Text, "[^0-9]", "");
            PriceSummary();
        }
        int pPrice = 100, tPrice = 0, mPrice = 40, nPrice = 0;

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            tbxDateTime.Text = dpdate.SelectedDate.Value.ToShortDateString();
        }

        private void tbxTattooCount_LostFocus(object sender, RoutedEventArgs e)
        {
            var tbx = (TextBox)sender;
            tbx.Text = tbx.Text.Trim();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!edit)
            {
                tbxName.Focus();
            }
            else
            {
                lblUnits.Visibility = Visibility.Visible;
                

                label.Content = "לקוח קיים";
                newClient = database.Clients.Where(c => c.Id == clientid).FirstOrDefault();
                tbxName.Text = newClient.Name;
                defaultName = newClient.Name;
                tbxPhone.Text = newClient.Phone;
                lblUnits.Content = "נקודות: " + newClient.Units;

            }
            dpdate.SelectedDate = DateTime.Now;
            tbxDateTime.Text = DateTime.Now.ToShortDateString();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void tbxName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {

        }
        // Saves user typen numbers to ints
        private void tbxTattooPrice_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox tbx = (TextBox)sender;
            tbx.Text = tbx.Text.Trim();
            switch (tbx.Name)
            {
                case "tbxTattooPrice":
                    if (!string.IsNullOrEmpty(tbx.Text))
                    {
                        tPrice = int.Parse(tbx.Text);
                    }
                    break;
                case "tbxPiercingPrice":
                    if (!string.IsNullOrEmpty(tbx.Text))
                    {
                        pPrice = int.Parse(tbx.Text);
                    }
                    break;
                case "tbxMishaPrice":
                    if (!string.IsNullOrEmpty(tbx.Text))
                    {
                        mPrice = int.Parse(tbx.Text);
                    }
                    break;
                case "tbxNecklePrice":
                    if (!string.IsNullOrEmpty(tbx.Text))
                    {
                        nPrice = int.Parse(tbx.Text);
                    }
                    break;
            }
        }

        private void tbxTattooPriceType_TextChanged(object sender, TextChangedEventArgs e)
        {
          if(loaded)
            {
                var tbxType = (TextBox)sender;
                TextBox[] tbxes = new TextBox[]
                {
                tbxTattooPrice,
                tbxPiercingPrice,
                tbxMishaPrice,
                tbxNecklePrice
                };
                switch (tbxType.Text)
                {
                    case "מזומן":

                        foreach (var tbx in tbxes)
                        {
                            if (tbxType.Name.Contains(tbx.Name))
                            {
                                if (!string.IsNullOrEmpty(tbx.Text))
                                {
                                    switch (tbx.Name)
                                    {
                                        case "tbxTattooPrice":

                                            tbx.Text = tPrice.ToString();
                                            break;
                                        case "tbxPiercingPrice":

                                            tbx.Text = pPrice.ToString();
                                            break;
                                        case "tbxMishaPrice":
                                            tbx.Text = mPrice.ToString();

                                            break;
                                        case "tbxNecklePrice":
                                            tbx.Text = nPrice.ToString();
                                            break;
                                    }
                                }
                            }
                        }
                        break;

                    case "אשראי":

                        foreach (var tbx in tbxes)
                        {
                            if (tbxType.Name.Contains(tbx.Name))
                            {
                                if (!string.IsNullOrEmpty(tbx.Text))
                                {
                                    switch (tbx.Name)
                                    {
                                        case "tbxTattooPrice":
                                            tbx.Text = Math.Floor(tPrice * 1.025).ToString();
                                            break;
                                        case "tbxPiercingPrice":
                                            tbx.Text = Math.Floor(pPrice * 1.025).ToString();
                                            break;
                                        case "tbxMishaPrice":
                                            tbx.Text = Math.Floor(mPrice * 1.025).ToString();
                                            break;
                                        case "tbxNecklePrice":
                                            tbx.Text = Math.Floor(nPrice * 1.025).ToString();
                                            break;
                                    }
                                }
                            }
                        }

                        break;
                }
                PriceSummary();
            }
        }
    }
}
