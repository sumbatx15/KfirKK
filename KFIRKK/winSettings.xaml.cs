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
    /// Interaction logic for winSettings.xaml
    /// </summary>
    public partial class winSettings : Window
    {
        public winSettings()
        {
            InitializeComponent();
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Precent = int.Parse(tbxPrecent.Text);
            Properties.Settings.Default.Save();
            MessageBox.Show("מעכשיו הלקוחות יקבלו : " + Properties.Settings.Default.Precent.ToString() + "% , מהסכום הכולל לנקודות");
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbxPrecent.Text = Properties.Settings.Default.Precent.ToString();
            ckDohShnatiOpenClose.IsChecked = Properties.Settings.Default.DohShnatiOpen;
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
           if(ckDohShnatiOpenClose.IsChecked == true)
            {
                Properties.Settings.Default.DohShnatiOpen = true;
            }
            else
            {
                Properties.Settings.Default.DohShnatiOpen = false;
            }
            Properties.Settings.Default.Save();
        }
    }
}
