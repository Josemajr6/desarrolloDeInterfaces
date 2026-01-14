using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Ejercicio_4
{
    public partial class EditarCliente : Window
    {
        private string conexion;
        private DataTable dtClientes;


        /// <summary>
        /// Initializes a new instance of the <see cref="EditarCliente"/> class.
        /// </summary>
        /// <param name="conexion">The conexion.</param>
        public EditarCliente(string conexion)
        {
            InitializeComponent();
            this.conexion = conexion;
            CargarClientes();
        }


        /// <summary>
        /// Cargars the clientes.
        /// </summary>
        private void CargarClientes()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(conexion))
                {
                    connection.Open();
                    string sql = "SELECT * FROM CLIENTES";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    dtClientes = new DataTable();
                    adapter.Fill(dtClientes);
                    dgClientes.ItemsSource = dtClientes.DefaultView;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }


        /// <summary>
        /// Handles the CellEditEnding event of the dgClientes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridCellEditEndingEventArgs"/> instance containing the event data.</param>
        private void dgClientes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction != DataGridEditAction.Commit) return;


            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                DataRowView rowView = (DataRowView)e.Row.Item;
                string dni = rowView["DNI"].ToString();
                string nombre = rowView["Nombre"].ToString();
                string saldoStr = rowView["Saldo"].ToString();

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    MessageBox.Show("Error: el nombre no puede estar vacío");
                    CargarClientes(); 
                    return;
                }

                if (!double.TryParse(saldoStr, out double saldo) || saldo <= 0)
                {
                    MessageBox.Show("Error: el saldo debe ser mayor que 0");
                    CargarClientes(); 
                    return;
                }

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(conexion))
                    {
                        connection.Open();
                        string sqlActualizar = "UPDATE CLIENTES SET Nombre=@nombre, Saldo=@saldo WHERE DNI=@dni";
                        MySqlCommand cmdActualizar = new MySqlCommand(sqlActualizar, connection);
                        cmdActualizar.Parameters.AddWithValue("@nombre", nombre);
                        cmdActualizar.Parameters.AddWithValue("@saldo", saldo);
                        cmdActualizar.Parameters.AddWithValue("@dni", dni);
                        cmdActualizar.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error al actualizar cliente: " + ex.Message);
                }

            }));
        }
    }
}
