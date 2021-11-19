using AAVD.Base_de_datos;
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
            consumo_year.CustomFormat = "yyyy";
            consumo_year.Format = DateTimePickerFormat.Custom;
            consumo_month.CustomFormat = "MM";
            consumo_month.Format = DateTimePickerFormat.Custom;
            recibo_year.CustomFormat = "yyyy";
            recibo_year.Format = DateTimePickerFormat.Custom;
            recibo_mes.CustomFormat = "MM";
            recibo_mes.Format = DateTimePickerFormat.Custom;
            edc_nacimiento.CustomFormat = "yyyy-MM-dd";
            edc_nacimiento.Format = DateTimePickerFormat.Custom;
            c_nacimiento.CustomFormat = "yyyy-MM-dd";
            c_nacimiento.Format = DateTimePickerFormat.Custom;
            this.CenterToScreen();
            updateDataGrid();
            updateDataGridTarifa();
            updateDataGridConsumos();
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
                database.registerClient(c_nombre.Text, c_apellidoP.Text, c_apellidoM.Text, c_email.Text, c_curp.Text, c_genero.Text, c_nacimiento.Value.ToString("yyyy-MM-dd"), c_ciudad.Text, c_calle.Text, c_colonia.Text, c_estado.Text, c_contratoTipo.Text, c_usuario.Text, c_password.Text, new_user_id, c_noMedidor.Text, c_noServicio.Text, c_numCliente.Text);
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
                clienteDTG.num_cliente = cliente.num_cliente.ToString();
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
            if (indexRow < 0)
            {
                MessageBox.Show("Selecciona a un empleado");
                return;
            }
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
            edc_numCliente.Text = row.Cells[16].Value.ToString();
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
            DatabaseManagement.getInstance().updateClient(edc_nombre.Text, edc_apellidoP.Text, edc_apellidoM.Text, edc_email.Text, edc_curp.Text, edc_genero.Text, edc_nacimiento.Value.ToString("yyyy-MM-dd"), edc_ciudad.Text, edc_calle.Text, edc_colonia.Text, edc_estado.Text, edc_contrato.Text, edc_usuario.Text, edc_password.Text, id_seleccionado, edc_numCliente.Text);
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

        //Cargar un consumo
        private void btn_consumo_Click(object sender, EventArgs e)
        {
            DatabaseManagement.getInstance().insertConsumo(consumo_medidor.Text, consumo_kw.Text, consumo_year.Text, consumo_month.Text);
            MessageBox.Show("Consumo registrado");
            updateDataGridConsumos();
        }

        //Poner los consumos en el data grid
        public void updateDataGridConsumos()
        {

            List<Consumos> consumos = new List<Consumos>();
            consumos = DatabaseManagement.getInstance().getConsumos();

            List<Consumos> csmDTG = new List<Consumos>();
            foreach (var consumo in consumos)
            {
                Consumos consumoDTG = new Consumos();
                consumoDTG.num_medidor = consumo.num_medidor;
                consumoDTG.consumo = consumo.consumo;
                consumoDTG.month = consumo.month;
                consumoDTG.year = consumo.year;
                csmDTG.Add(consumo);

            }
            ConsumosDTG_WN.DataSource = csmDTG;
        }
        //Carga masiva de consumos
        private void carga_consumos_Click(object sender, EventArgs e)
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
            if (result != "csv")
            {
                MessageBox.Show("Escoge un direccion valida");
                return;
            }

            var reader = File.OpenText(csvPath);
            var csvReader = new CsvReader(reader, CultureInfo.CurrentCulture);
            var consumosCSV = csvReader.GetRecords<ConsumosCSV>();
            foreach (var consumo in consumosCSV)
            {
                DatabaseManagement.getInstance().insertConsumo(consumo.numMedidor.ToString(), consumo.consumo.ToString(), consumo.year, consumo.month);
            }
            MessageBox.Show("Listo, consumos cargados...");
            updateDataGridConsumos();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            //Se traen las tarifas
            List<Tarifas> tarifas = new List<Tarifas>();
            tarifas = DatabaseManagement.getInstance().GetTarifas();
            double iBasica, iIntermedia, iExcedente;
            double dBasica, dIntermedia, dExcedente;
            iBasica = iIntermedia = iExcedente = 0;
            dBasica = dIntermedia = dExcedente = 0;
            foreach (var tarifa in tarifas) {
                if (tarifa.tipo == "Industrial")
                {
                    iBasica = tarifa.basico;
                    iIntermedia = tarifa.intermedio;
                    iExcedente = tarifa.excedente;
                }

                if (tarifa.tipo == "Domestico") {
                    dBasica = tarifa.basico;
                    dIntermedia = tarifa.intermedio;
                    dExcedente = tarifa.excedente;
                }
            
            }

            double iTotalBasica, iTotalIntermedia, iTotalExcedente;
            double dTotalBasica, dTotalIntermedia, dTotalExcedente;
            iTotalBasica = iTotalIntermedia = iTotalExcedente = 0;
            dTotalBasica = dTotalIntermedia = dTotalExcedente = 0;
            //Se trae los consumos
            List<Consumos> consumo = new List<Consumos>();
            consumo = DatabaseManagement.getInstance().getConsumoEspecifico(tb_medidor.Text, recibo_year.Text, recibo_mes.Text);
            foreach (var row in consumo) {
                double consumoTotal = row.consumo;

                if (consumoTotal > 100 ) {
                    double consumoIntermedio, consumoExcedente;
                    if (consumoTotal > 150)
                    {
                        consumoExcedente = consumoTotal - 150;
                        iTotalExcedente = consumoExcedente * iExcedente;
                        dTotalExcedente = consumoExcedente * dExcedente;
                        consumoIntermedio = consumoExcedente - 100;
                        iTotalIntermedia = consumoIntermedio * iIntermedia;
                        dTotalIntermedia = consumoIntermedio * dIntermedia;
                    }
                    else {
                        consumoExcedente = consumoTotal - 150;
                        consumoIntermedio = consumoExcedente - 100;
                        iTotalIntermedia = consumoIntermedio * iIntermedia;
                        dTotalIntermedia = consumoIntermedio * dIntermedia;
                    }
                    iTotalBasica = iBasica * 100;
                    dTotalBasica = dBasica * 100;
                }

                if (consumoTotal <= 100) {
                    iTotalBasica = consumoTotal * iBasica;
                    dTotalBasica = consumoTotal * dBasica;
                }

                double medidor = row.num_medidor;
                List<Contratos> contratos = new List<Contratos>();
                contratos = DatabaseManagement.getInstance().GetContratosMedidorTipo();
                foreach (var contrato in contratos) {
                    if (contrato.num_medidor == row.num_medidor) {
                        if ((contrato.tipo.ToString()).Equals("Industrial")){
                            tx_totalBasico.Text = iTotalBasica.ToString();
                            tx_totalIntermedio.Text = iTotalIntermedia.ToString();
                            tx_totalExcedente.Text = iTotalExcedente.ToString();
                            tx_totalFinal.Text = ((iTotalBasica + iTotalIntermedia + iTotalExcedente) * 1.16).ToString();
                            double sinIVA = iTotalBasica + iTotalIntermedia + iTotalExcedente;
                            double conIVA = sinIVA * 1.16;
                            DatabaseManagement.getInstance().insertRecibo(tb_medidor.Text, recibo_year.Text, recibo_mes.Text, iTotalBasica, iTotalIntermedia, iTotalExcedente, sinIVA, conIVA);
                        }
                        if ((contrato.tipo.ToString()).Equals("Domestico")){
                            tx_totalBasico.Text = dTotalBasica.ToString();
                            tx_totalIntermedio.Text = dTotalIntermedia.ToString();
                            tx_totalExcedente.Text = dTotalExcedente.ToString();
                            tx_totalFinal.Text = ((dTotalBasica + dTotalIntermedia + dTotalExcedente) * 1.16).ToString();
                            double sinIVA = dTotalBasica + dTotalIntermedia + dTotalExcedente;
                            double conIVA = sinIVA * 1.16;
                            DatabaseManagement.getInstance().insertRecibo(tb_medidor.Text, recibo_year.Text, recibo_mes.Text, dTotalBasica, dTotalIntermedia, dTotalExcedente, sinIVA, conIVA);
                        }
                    }
                }
               
            }
            //string reciboBasicoIn = "Industrial basico: $ " + iTotalBasica;
            //string reciboBasicoDo = "Domestico basico: $ " + dTotalBasica;
            //MessageBox.Show(reciboBasicoDo);
            //MessageBox.Show(reciboBasicoIn);

            //string reciboIntermedioIn = "Industrial intermedio: $ " + iTotalIntermedia;
            //string reciboIntermedioDo = "Domestico intermedio: $ " + dTotalIntermedia;
            //MessageBox.Show(reciboIntermedioIn);
            //MessageBox.Show(reciboIntermedioDo);
            //string reciboExcedenteIn = "Industrial excedente: $ " + iTotalExcedente;
            //string reciboExcedenteDo = "Domestico excedente: $ " + dTotalExcedente;
            //MessageBox.Show(reciboExcedenteIn);
            //MessageBox.Show(reciboExcedenteDo);
            //string totalSinIVAIn = "Total sin IVA = " + (iTotalBasica + iTotalIntermedia + iTotalExcedente);
            //string totalConIVAIn = "Total sin IVA = " + ((iTotalBasica + iTotalIntermedia + iTotalExcedente) * 1.16);
            //MessageBox.Show(totalSinIVAIn);
            //MessageBox.Show(totalConIVAIn);
            //string totalSinIVADo = "Total sin IVA = " + (dTotalBasica + dTotalIntermedia + dTotalExcedente);
            //string totalConIVADo = "Total sin IVA = " + ((dTotalBasica + dTotalIntermedia + dTotalExcedente) * 1.16);
            //MessageBox.Show(totalSinIVADo);
            //MessageBox.Show(totalConIVADo);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        //Asignar un contrato
        private void button9_Click(object sender, EventArgs e)
        {
            List<Contratos> contratos = new List<Contratos>();
            contratos = DatabaseManagement.getInstance().GetContratos();
            foreach (var contrato in contratos) {
                if ((contrato.num_medidor.ToString()).Equals(contrato_numMedidor.Text)) {
                    MessageBox.Show("Ese numero de medidor ya esta en uso");
                    return;
                }
                if ((contrato.num_servicio.ToString()).Equals(contrato_numServicio.Text))
                {
                    MessageBox.Show("Ese numero de servicio ya esta en uso");
                    return;
                }
            }
            DatabaseManagement.getInstance().updateContratos(contrato_numCLiente.Text, contrato_Tipo.Text, contrato_numMedidor.Text, contrato_numServicio.Text);
            MessageBox.Show("Nuevo contrato asociado a el cliente.");
        }
    }
}
