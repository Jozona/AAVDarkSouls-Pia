using AAVD.Base_de_datos;
using AAVD.Entidades;
using AAVD.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAVD
{
    public partial class Form1 : Form
    {
        public static string currentUser;
        public static Guid currentUserId;
        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void btnNewUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register registracion = new Register();
            registracion.Closed += (object sender2,EventArgs e2) => this.Close();
            registracion.ShowDialog();
        }


        /// <summary>
        /// Buscamos a el usuario en cassandra y dependiendoo de su user_type abrimos 
        /// la ventana correspondiente.
        /// 0 = ADMIN.
        /// 1 = EMPLEADO
        /// 2 = CLIENTE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Falta:
            //Poder recuperar contrase;a. Si se ingresa un usuario 3 veces con la contrase;a incorrecta, bloquearlo.
            //Comentar hasta que acabe el foreach si cassandra no esta onectado. Para poder abrir las ventanas asi mero.
            List<Users> users = new List<Users>();
            users = DatabaseManagement.getInstance().getLogin(txtUser.Text, txtPassword.Text);
            foreach (var data in users) {
                if (data.user_type == 0)
                {
                    currentUser = data.user_name;
                    currentUserId = data.user_id;
                    this.Hide();
                    Admin admin = new Admin();
                    admin.Closed += (object sender2, EventArgs e2) => this.Close();
                    admin.ShowDialog();
                    break;
                }
                else if (data.user_type == 1)
                {
                    currentUser = data.user_name;
                    currentUserId = data.user_id;
                    this.Hide();
                    Employee employee = new Employee();
                    employee.Closed += (object sender2, EventArgs e2) => this.Close();
                    employee.ShowDialog();
                    break;
                }
                else if (data.user_type == 2) {
                    currentUser = data.user_name;
                    currentUserId = data.user_id;
                    this.Hide();
                    Client client = new Client();
                    client.Closed += (object sender2, EventArgs e2) => this.Close();
                    client.ShowDialog();
                    break;
                }
            }

            //Usuarios de prueba. 
            //cliente, empleado, admin.
            if (txtUser.Text == "cliente" && txtPassword.Text == "") {
                Client client = new Client();
                client.ShowDialog();
            }

            if (txtUser.Text == "empleado" && txtPassword.Text == "")
            {
                Employee employee = new Employee();
                employee.ShowDialog();
            }

            if (txtUser.Text == "admin" && txtPassword.Text == "")
            {
                
                Admin admin = new Admin();
                admin.ShowDialog();
            }

            //Si esta ventana aun no esta cerrada, es porque no se encontro al usuario.
            MessageBox.Show("No existes");
        }

        //Esta se genera cuando le das click al background de la ventana.
        //Por el nombre asumo que es lo que se ejecuta al abrir la ventana.
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
