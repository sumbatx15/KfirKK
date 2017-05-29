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
    /// Interaction logic for winReport.xaml
    /// </summary>
    public partial class winReport : Window
    {
        public winReport()
        {
            InitializeComponent();
            reportViewer.Load += ReportViewer_Load;
        }
        private bool _isReportViewerLoaded;
        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
                Database1DataSet dataset = new Database1DataSet();

                dataset.BeginInit();

                reportDataSource1.Name = "DataSet1"; 
                reportDataSource1.Value = dataset.Work;
                this.reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                this.reportViewer.LocalReport.ReportEmbeddedResource = "KFIRKK.Report1.rdlc";

                dataset.EndInit();

                //fill data into adventureWorksDataSet
                Database1DataSetTableAdapters.WorkTableAdapter salesOrderDetailTableAdapter = new Database1DataSetTableAdapters.WorkTableAdapter();
                salesOrderDetailTableAdapter.ClearBeforeFill = true;
                salesOrderDetailTableAdapter.Fill(dataset.Work);

                reportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
