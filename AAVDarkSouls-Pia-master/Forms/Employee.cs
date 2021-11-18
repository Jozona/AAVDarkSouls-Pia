﻿using AAVD.Base_de_datos;
using AAVD.Entidades;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            updateDataGridTarifa();
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

        //Click al boton de actualizar cliente
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


        //Definir las tarifas
        private void btn_agregar_tarifa_Click(object sender, EventArgs e)
        {
            DatabaseManagement.getInstance().crearTarifa(eb_tipoTarifa.Text, eb_TarifaBasica.Text, eb_TarifaIntermedia.Text, eb_TarifaExcedente.Text);
            MessageBox.Show("Tarifa actualizada");
            updateDataGridTarifa();
        }

        //Poner las tarifas en ventana
        public void updateDataGridTarifa() {

            List<Tarifas> tarifas = new List<Tarifas>();
            tarifas = DatabaseManagement.getInstance().GetTarifas();

            List<Tarifas> tfDTG = new List<Tarifas>();
            foreach (var tarifa in tarifas)
            {
                Tarifas tarifaDTG = new Tarifas();
                tarifaDTG.tipo = tarifa.tipo ;
                tarifaDTG.basico = tarifa.basico;
                tarifaDTG.intermedio = tarifa.intermedio;
                tarifaDTG.excedente = tarifa.excedente;
                tfDTG.Add(tarifaDTG);

            }
            TarifasFTG_WN.DataSource = tfDTG;
        }

        private void csv_tarifas_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            string csvPath = "";
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Archivos de informacion excel (*.csv)|*.csv";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                csvPath = openFileDialog1.FileName;
            }
            var result = csvPath.Substring(csvPath.Length - 3);
            if (result != "csv") {
                MessageBox.Show("Escoge un direccion valida");
                return;
            }

            var reader = File.OpenText(csvPath);
            var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture);
            var tarifasCSV = csvReader.GetRecords<TarifasCSV>();
            foreach (var tarifa in tarifasCSV)
            {
                DatabaseManagement.getInstance().crearTarifa(tarifa.tipo, tarifa.basico.ToString(),tarifa.intermedio.ToString(),tarifa.excedente.ToString());
            }
            MessageBox.Show("Listo, tarifas cargadas...");
        }


        //Generar el pdf del recibo
        private void button7_Click(object sender, EventArgs e)
        {
            string id_usuario = "";
            List<Users> users = new List<Users>();
            users = DatabaseManagement.getInstance().getRemember(eb_user_recibo.Text);
            foreach (var usuarioSimple in users)
            {
                List<Users> usersFull = new List<Users>();
                usersFull = DatabaseManagement.getInstance().getLogin(usuarioSimple.user_name, usuarioSimple.password);
                foreach (var fullUser in usersFull)
                {
                    id_usuario = fullUser.user_id.ToString();
                }
            }

            List<Clientes> cliente = new List<Clientes>();
            cliente = DatabaseManagement.getInstance().getClientWithUserId(id_usuario);
            foreach (var dato in cliente)
            {
                //Prepara todas las variables para el pdf
                Document pdfDocument = new Document();
                Page page1 = pdfDocument.Pages.Add();

                BackgroundArtifact bg = new BackgroundArtifact();
                bg.BackgroundImage = File.OpenRead("CFE.png");

                page1.Artifacts.Add(bg);

                TextFragment txtName = new TextFragment(dato.name);
                txtName.Position = new Position(117, 715);
                txtName.TextState.FontSize = 12;
                txtName.TextState.Font = FontRepository.FindFont("Arial");
                txtName.TextState.ForegroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Black);

                TextBuilder txtBuild = new TextBuilder(page1);
                txtBuild.AppendText(txtName);

                String pdfName = "LmadEstadoDeCuenta_.pdf";
                pdfDocument.Save(pdfName);
            }
        }

        private void ValidarClientes_Click(object sender, EventArgs e)
        {
            bool validaruser = true;
            bool validarpassword = true;
            bool validarnombre = true;
            bool validarAP = true;
            bool validarAM = true;
            //bool validarRFC = true;
            //bool validarfecha = false;
            bool validarCURP = true;

            bool validarCiudad = true;
            bool validarCalle = true;
            bool validarColonia = true;
            bool validarEstado = true;

            String username = this.c_usuario.Text;
            String password = this.c_password.Text;
            String vnombre = this.c_nombre.Text;
            String vAP = this.c_apellidoP.Text;
            String vAM = this.c_apellidoM.Text;
            //Domicilio
            String vCiudad = this.c_ciudad.Text;
            String vCalle = this.c_calle.Text;
            String vColonia = this.c_colonia.Text;
            String vEstado = this.c_estado.Text;

            //String vRFC = this.eb_rfc_emp.Text;
            //String vFecha = this.dtp_nac_emp.Text;
            String vCurp = this.c_curp.Text;


            string regexString = /*@"/^[A-Za-z]$/"*/ "[A-Za-z]"; //Permite letras y numeros
            string regexStringPass = "[0-9]";
            string regexStringRFC = "^[0-9]*$";
            string regexStringnombre = "^[A-Za-z]*$";
            string regexStringCurp = "[A-Za-z]";


            //Validacion username
            if (Regex.IsMatch(username, regexString))
            {
                MessageBox.Show("Muito bien");
                //validaruser = true;

            }

            else
            {
                validaruser = false;
                MessageBox.Show("Solo se permiten letras y numeros. Ej: Luis123");
            }

            //Validacion password
            if (Regex.IsMatch(password, regexStringPass))
            {

                MessageBox.Show("Muito bien");
            }

            else
            {
                MessageBox.Show("Solo se permiten numeros y letras. Ej: 123Pass");
                validarpassword = false;
            }

            //Validacion nombre
            if (Regex.IsMatch(vnombre, regexStringnombre))
            {

                MessageBox.Show("Muito bien");
            }

            else
            {
                validarnombre = false;
                MessageBox.Show("Solo se permiten letras");

            }

            //Validacion AP
            if (Regex.IsMatch(vAP, regexStringnombre))
            {

                MessageBox.Show("Muito bien");
            }

            else
            {
                validarAP = false;
                MessageBox.Show("Solo se permiten letras");

            }

            //Validacion AM
            if (Regex.IsMatch(vAM, regexStringnombre))
            {

                MessageBox.Show("Muito bien");
            }

            else
            {
                validarAM = false;
                MessageBox.Show("Solo se permiten letras");

            }

            //Validacion RFC
            //if (Regex.IsMatch(vRFC, regexStringRFC))
            //{

            //    MessageBox.Show("Muito bien");
            //}

            //else
            //{
            //    validarRFC = false;
            //    MessageBox.Show("Solo se permiten numeros");

            //}

            //Validacion CURP
            if (Regex.IsMatch(vCurp, regexStringCurp))
            {

                MessageBox.Show("Muito bien");
            }

            else
            {
                validarCURP = false;
                MessageBox.Show("Solo se permiten letras y numeros. Ej: IBP505");

            }

            //validacion Colonia

            if (Regex.IsMatch(vColonia, regexStringnombre))
            {

                MessageBox.Show("Muito bien");
            }

            else
            {
                validarColonia = false;
                MessageBox.Show("Solo se permiten letras");

            }

            //Validar Ciudad

            if (Regex.IsMatch(vCiudad, regexStringnombre))
            {

                MessageBox.Show("Muito bien");
            }

            else
            {
                validarCiudad = false;
                MessageBox.Show("Solo se permiten letras");

            }

            //Validar Calle

            if (Regex.IsMatch(vCalle, regexStringnombre))
            {

                MessageBox.Show("Muito bien");
            }

            else
            {
                validarCalle = false;
                MessageBox.Show("Solo se permiten letras");

            }

            //Validar Estado

            if (Regex.IsMatch(vEstado, regexStringnombre))
            {

                MessageBox.Show("Muito bien");
            }

            else
            {
                validarEstado = false;
                MessageBox.Show("Solo se permiten letras");

            }






            //Validacion de todos los campos
            if (validaruser == false || validarpassword == false || validarnombre == false || validarAP == false 
                || validarAM == false || validarCURP == false || validarCalle == false || validarCiudad == false
                || validarEstado == false || validarColonia == false)
            {
                //Da error
                MessageBox.Show("ESTO DA ERROR");
            }
            else
            {
                //Aqui podemos asignar un booleano global para permitir o no agregar un empleado
                MessageBox.Show("Esto no da error");
            }





        }
    }
}
