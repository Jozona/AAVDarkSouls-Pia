using AAVD.Base_de_datos;
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
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;

            tarjeta_fecha.Format = DateTimePickerFormat.Custom;
            tarjeta_fecha.CustomFormat = "MM/yyyy";

            pagar_year.Format = DateTimePickerFormat.Custom;
            pagar_year.CustomFormat = "yyyy";

            pagar_mes.Format = DateTimePickerFormat.Custom;
            pagar_mes.CustomFormat = "MM";
            this.CenterToScreen();
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.Text.Equals("Visa")) {
                tarjeta_numero.Visible = tarjeta_fecha.Visible = tarjeta_ccv.Visible = true;
                label14.Visible = label15.Visible = label16.Visible = true;
                label17.Visible = false;
            }

            if (comboBox3.Text.Equals("Visa")){
                tarjeta_numero.Visible = tarjeta_fecha.Visible = tarjeta_ccv.Visible = true;
                label14.Visible = label15.Visible = label16.Visible = true;
                label17.Visible = false;
            }

            if (comboBox3.Text.Equals("Paypal"))
            {
                tarjeta_numero.Visible = tarjeta_fecha.Visible = tarjeta_ccv.Visible = false;
                label14.Visible = label15.Visible = label16.Visible = false;
                label17.Visible = true;
            }

            if (comboBox3.Text.Equals("GPay"))
            {
                tarjeta_numero.Visible = tarjeta_fecha.Visible = tarjeta_ccv.Visible = false;
                label14.Visible = label15.Visible = label16.Visible = false;
                label17.Visible = true;
            }

            if(comboBox3.Text.Equals("Efectivo"))
            {
                tarjeta_numero.Visible = tarjeta_fecha.Visible = tarjeta_ccv.Visible = false;
                label14.Visible = label15.Visible = label16.Visible = false;
                label17.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Procesando el pago...");
            DatabaseManagement.getInstance().pagarRecibo(pagar_medidor.Text, pagar_year.Text, pagar_mes.Text, comboBox3.Text);
            MessageBox.Show("Recibo pagado!");
        }
    }
}
