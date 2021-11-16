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
        //Init
        public Employee()
        {
            InitializeComponent();

            edc_nacimiento.CustomFormat = "yyyy-MM-dd";
            edc_nacimiento.Format = DateTimePickerFormat.Custom;
            c_nacimiento.CustomFormat = "yyyy-MM-dd";
            c_nacimiento.Format = DateTimePickerFormat.Custom;
            this.CenterToScreen();
            updateDataGrid();
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
            if (!(database.registerUser(c_usuario.Text, c_password.Text, 2, c_pregunta.Text, c_respuesta.Text))) {
                MessageBox.Show("No se pueden repetir usuarios");
                return;
            }
            List<Users> user = new List<Users>();
            user = database.getLogin(c_usuario.Text, c_password.Text);
            Guid new_user_id;
            foreach (var data in user)
            {
                new_user_id = data.user_id;
                database.registerClient(c_nombre.Text, c_apellidoP.Text, c_apellidoM.Text, c_email.Text, c_curp.Text, c_genero.Text, c_nacimiento.Value.ToString("yyyy-MM-dd"), c_ciudad.Text, c_calle.Text, c_colonia.Text, c_estado.Text, c_contratoTipo.Text, c_usuario.Text, c_password.Text, new_user_id);
            }
            MessageBox.Show("Cliente registrado con exito.");
            updateDataGrid();
        }

        //Funcion que envia los datos al datagrid
        public void updateDataGrid()
        {

            List<Clientes> clientes = new List<Clientes>();
            clientes = DatabaseManagement.getInstance().GetClients();

            List<ClientesDTG> cntDataGrid = new List<ClientesDTG>();
            foreach (var cliente in clientes)
            {
                ClientesDTG clienteDTG = new ClientesDTG();
                clienteDTG.name = cliente.name;
                clienteDTG.last_name = cliente.last_name;
                clienteDTG.mother_last_name = cliente.mother_last_name;
                clienteDTG.user = cliente.user;
                clienteDTG.password = cliente.password;
                clienteDTG.date_of_birth = cliente.date_of_birth.ToString();
                clienteDTG.gender = cliente.gender;
                clienteDTG.contract_type = cliente.contract_type;
                clienteDTG.street = cliente.street;
                clienteDTG.colony = cliente.colony;
                clienteDTG.city = cliente.city;
                clienteDTG.state = cliente.state;
                clienteDTG.email = cliente.email;
                clienteDTG.curp = cliente.curp;
                clienteDTG.user_id = cliente.user_id;
                clienteDTG.client_id = cliente.client_id;
                cntDataGrid.Add(clienteDTG);

            }

            clientesDTGWN.DataSource = cntDataGrid;
            ClientesBorrarDTG.DataSource = cntDataGrid;

            int i = 0;
        }

        private void btn_buscar_Click(object sender, EventArgs e)
        {
            List<Clientes> clientes = new List<Clientes>();
            clientes = DatabaseManagement.getInstance().GetClients();
            updateDataGrid();
            int i = 0;
        }

        private void clientesDTGWN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        string id_seleccionado;
        string id_seleccionado_borrar;
        string user;
        string password;
        //Envia los datos del datagrid a la ventana
        //Los indices de Cells[] son dependiendo del orden en el que fueron declaradas las variables en ClientesDTG.cs
        private void clientesDTGWN_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            DataGridViewRow row = clientesDTGWN.Rows[indexRow];
            if (row == null)
                return;

            edc_nombre.Text = row.Cells[2].Value.ToString();
            edc_apellidoP.Text = row.Cells[3].Value.ToString();
            edc_apellidoM.Text = row.Cells[4].Value.ToString();
            edc_nacimiento.Text = row.Cells[5].Value.ToString();
            edc_genero.Text = row.Cells[6].Value.ToString();
            edc_ciudad.Text = row.Cells[7].Value.ToString();
            edc_calle.Text = row.Cells[8].Value.ToString();
            edc_colonia.Text = row.Cells[9].Value.ToString();
            edc_estado.Text = row.Cells[10].Value.ToString();
            edc_contrato.Text = row.Cells[11].Value.ToString();
            edc_usuario.Text = row.Cells[12].Value.ToString();
            edc_password.Text = row.Cells[13].Value.ToString();
            edc_email.Text = row.Cells[14].Value.ToString();
            edc_curp.Text = row.Cells[15].Value.ToString();
            id_seleccionado = row.Cells[1].Value.ToString();
        }

        //Click al boton de actualizar empleado
        private void btn_actualizar_Click(object sender, EventArgs e)
        {
            if (id_seleccionado == null)
            {
                MessageBox.Show("No has seleccionando ningun cliente.");
                return;
            }
            DatabaseManagement.getInstance().updateClient(edc_nombre.Text, edc_apellidoP.Text, edc_apellidoM.Text, edc_email.Text, edc_curp.Text, edc_genero.Text, edc_nacimiento.Value.ToString("yyyy-MM-dd"), edc_ciudad.Text, edc_calle.Text, edc_colonia.Text, edc_estado.Text, edc_contrato.Text, edc_usuario.Text, edc_password.Text, id_seleccionado);
            id_seleccionado = null;
            MessageBox.Show("Cliente actualizado con exito.");
            updateDataGrid();
        }

        //Seleccionar un cliente para borrar
        private void ClientesBorrarDTG_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            DataGridViewRow row = clientesDTGWN.Rows[indexRow];
            if (row == null)
                return;
            id_seleccionado_borrar = row.Cells[1].Value.ToString();
            user = row.Cells[12].Value.ToString();
            password = row.Cells[14].Value.ToString();
        }

        private void btn_borrar_Click(object sender, EventArgs e)
        {
            if (id_seleccionado_borrar == null)
            {
                MessageBox.Show("No has seleccionando ningun cliente.");
                return;
            }
            DatabaseManagement.getInstance().eraseClient(id_seleccionado_borrar, user, password);
            id_seleccionado_borrar = null;
            MessageBox.Show("Cliente borrado con exito.");
            updateDataGrid();
        }
    }
}
