using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ejercicio_4
{
    /// <summary>
    /// Lógica de interacción para la ventana principal MainWindow.xaml.
    /// Permite gestionar la conexión a la base de datos y la navegación a otras ventanas.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Cadena de conexión que contiene las credenciales y ubicación del servidor MySQL.
        /// </summary>
        private string conexion = "server=localhost; port=3306; database=pruebawpf; uid=root; pwd=password";

        /// <summary>
        /// Constructor de la clase MainWindow. Incializa los componentes de la interfaz
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Abre la conexión con la base de datos MySQL y muestra los datos de la tabla
        /// </summary>
        /// <param name="sender">Disparador del botón (cuando lo pulsamos)</param>
        /// <param name="e">Argumentos del evento de clic</param>
        private void btnMostrarDatos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(conexion);

                string sql = "select * from clientes";

                MySqlCommand command = new MySqlCommand(sql, connection);

                DataTable table =  new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(table);

                dgDatos.ItemsSource = table.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se ha producido un error: " + ex);
            }

        }


        /// <summary>Handles the Click event of the btnAgregarCliente control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        private void btnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            AgregarCliente agregarCliente = new AgregarCliente(conexion);
            agregarCliente.Show();
        }



        /// <summary>
        /// Handles the Click event of the btnEditarCliente control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnEditarCliente_Click(object sender, RoutedEventArgs e)
        {
            EditarCliente editarCliente = new EditarCliente(conexion);
            editarCliente.Show();
        }
    }
}