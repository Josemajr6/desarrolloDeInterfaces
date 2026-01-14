using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient; // Asegúrate de tener esta referencia
using System;
using System.Data;
using System.IO;
using System.Windows;

namespace PR4_Stock
{
    public partial class MainWindow : Window
    {
        private ReportViewer _reportViewer;

        public MainWindow()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            _reportViewer = new ReportViewer();
            _reportViewer.ProcessingMode = ProcessingMode.Local;

            string reportPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PR4_Stock", "Report1.rdlc");


            if (!File.Exists(reportPath))
            {
                reportPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report1.rdlc");
            }

            _reportViewer.LocalReport.ReportPath = reportPath;


            DataSet1 datos = ObtenerDatosMySQL();

            _reportViewer.LocalReport.DataSources.Clear();


            ReportDataSource rds = new ReportDataSource("DataSet1", datos.Tables["stock"]);
            _reportViewer.LocalReport.DataSources.Add(rds);


            _reportViewer.RefreshReport();


            _host.Child = _reportViewer;
        }

        private DataSet1 ObtenerDatosMySQL()
        {
            DataSet1 ds = new DataSet1();


            string connectionString = "server=localhost; port=3306; database=testdb; uid=root; pwd=password";

            string query = "SELECT id, descripcion, unidades, precio_venta FROM stock";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
   
                        adapter.Fill(ds.stock);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar con MySQL: " + ex.Message);
                }
            }

            return ds;
        }
    }
}