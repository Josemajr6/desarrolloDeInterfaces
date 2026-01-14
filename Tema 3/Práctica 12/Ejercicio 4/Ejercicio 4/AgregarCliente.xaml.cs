using MySql.Data.MySqlClient;
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

namespace Ejercicio_4
{
    /// <summary>
    /// Lógica de interacción para AgregarCliente.xaml
    /// </summary>
    public partial class AgregarCliente : Window
    {
        private string conexion;


        /// <summary>
        /// Initializes a new instance of the <see cref="AgregarCliente"/> class.
        /// </summary>
        /// <param name="conexion">The conexion.</param>
        public AgregarCliente(string conexion)
        {
            InitializeComponent();
            this.conexion = conexion;
        }


        /// <summary>
        /// Handles the Click event of the btnAgregar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            string nombreF = tbNombre.Text;
            string dniF = tbDNI.Text;
            double saldoF = double.Parse(tbSaldo.Text);

            if (nombreF.Equals(""))
            {
                MessageBox.Show("Error. Rellena el nombre");
                return;
            }

            if (saldoF < 0)
            {
                MessageBox.Show("Error. Debes ingresar un valor superior 0 en Saldo");
                return;
            }

            try
            {
                MySqlConnection connection = new MySqlConnection(conexion);
                connection.Open();

                string sqlComprobar = "select DNI from clientes";
                MySqlCommand command = new MySqlCommand(sqlComprobar, connection);
                MySqlDataReader reader = command.ExecuteReader();

                bool existeDNI = false;
                while (reader.Read())
                {
                    string dni = reader.GetString("DNI");
                    if (dni == dniF)
                    {
                        existeDNI = true;
                    }
                }

                reader.Close();

                if (existeDNI == true)
                {
                    MessageBox.Show("Error. El DNI ya existe");
                    return;
                }

                string sqlInsertar = "INSERT INTO CLIENTES (Nombre, DNI, Saldo) values ('" + nombreF + "', '" + dniF + "', " + saldoF.ToString().Replace(",",".") + ")";
                MySqlCommand cmdInsertar = new MySqlCommand(sqlInsertar, connection);
                int filas = cmdInsertar.ExecuteNonQuery();

                if (filas > 0)
                {
                    MessageBox.Show("Se ha insertado el usuario");
                }
                else
                {
                    MessageBox.Show("No se ha podido insertar al usario");
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error de MySQL" + ex.Message);
            }



        }
    }
}
