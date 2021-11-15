using AAVD.Base_de_datos;
using AAVD.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAVD.Forms
{
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void Employee_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        //Boton para dar de alta un cliente
        private void button6_Click(object sender, EventArgs e)
        {
            //Registramos al cliente en la tbala de usuarios
            DatabaseManagement database = DatabaseManagement.getInstance();
            database.registerUser(c_usuario.Text, c_password.Text, 2);
            List<Users> user = new List<Users>();
            user = database.getLogin(c_usuario.Text, c_password.Text);
            Guid new_user_id;
            foreach (var data in user)
            {
                new_user_id = data.user_id;
                database.registerClient(c_nombre.Text, c_apellidoP.Text, c_apellidoM.Text, c_email.Text, c_curp.Text, c_genero.Text, c_nacimiento.Value.ToString("yyyy-MM-dd"), c_ciudad.Text, c_calle.Text, c_colonia.Text, c_estado.Text, c_contratoTipo.Text, c_usuario.Text, c_password.Text, new_user_id);
            }
            MessageBox.Show("Cliente registrado con exito.");
        }
    }
}
