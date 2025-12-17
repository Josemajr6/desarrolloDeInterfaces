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
        // Declaramos el visor
        private ReportViewer _reportViewer;

        public MainWindow()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Inicializar el ReportViewer
            _reportViewer = new ReportViewer();
            _reportViewer.ProcessingMode = ProcessingMode.Local;

            // 2. Configurar la ruta del informe
            // Asegúrate de que 'Report1.rdlc' se copie a la carpeta de salida (bin/Debug)
            // Si el archivo está en una subcarpeta 'PR4_Stock', ajusta la ruta.
            string reportPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PR4_Stock", "Report1.rdlc");

            // Si el archivo se copia en la raíz del ejecutable, usa solo "Report1.rdlc"
            if (!File.Exists(reportPath))
            {
                reportPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report1.rdlc");
            }

            _reportViewer.LocalReport.ReportPath = reportPath;

            // 3. Obtener los datos de MySQL
            DataSet1 datos = ObtenerDatosMySQL();

            // 4. Limpiar orígenes de datos previos
            _reportViewer.LocalReport.DataSources.Clear();

            // 5. Enlazar los datos
            // IMPORTANTE: El primer parámetro "DataSet1" debe coincidir EXACTAMENTE
            // con el nombre del DataSet que definiste DENTRO del diseño del archivo .rdlc.
            ReportDataSource rds = new ReportDataSource("DataSet1", datos.Tables["stock"]);
            _reportViewer.LocalReport.DataSources.Add(rds);

            // 6. Refrescar el informe
            _reportViewer.RefreshReport();

            // 7. Agregar el visor al host de Windows Forms definido en el XAML
            _host.Child = _reportViewer;
        }

        private DataSet1 ObtenerDatosMySQL()
        {
            DataSet1 ds = new DataSet1();

            // CAMBIA ESTO por tu cadena de conexión real de MySQL
            string connectionString = "server=localhost; port=3306; database=testdb; uid=root; pwd=password";

            // Tu consulta SQL para traer los datos. 
            // Los nombres de columnas (id, descripcion, etc.) deben coincidir con tu tabla en MySQL.
            string query = "SELECT id, descripcion, unidades, precio_venta FROM stock";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        // Llenamos específicamente la tabla 'stock' del DataSet tipado
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