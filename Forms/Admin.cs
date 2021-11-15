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
   
    public partial class Admin : Form
    {
        
        public Admin()
        {
            InitializeComponent(); 
            dtp_nac_emp.CustomFormat = "yyyy-MM-dd";
            dtp_nac_emp.Format = DateTimePickerFormat.Custom;
            this.CenterToScreen();
            updateDataGrid();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //Dar de alta un empleado
        private void button1_Click(object sender, EventArgs e)
        {
            DatabaseManagement database = DatabaseManagement.getInstance();
            database.registerUser(btn_emp_user.Text, btn_emp_pass.Text, 1);
            List<Users> user = new List<Users>();
            user = database.getLogin(btn_emp_user.Text, btn_emp_pass.Text);
            Guid new_user_id;
            foreach (var data in user) {
                new_user_id = data.user_id;
                database.registerEmployee(btn_emp_user.Text, btn_emp_pass.Text, eb_name_emp.Text, eb_ap_emp.Text, eb_am_emp.Text, eb_curp_emp.Text, eb_rfc_emp.Text, dtp_nac_emp.Value.ToString("yyyy-MM-dd"), new_user_id);
            }
            MessageBox.Show("Empleado registrado con exito.");
            updateDataGrid();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
        int indexRow;

        //Si la click a editar al empleado, lo buscamos por medio de su usuario y hacemos un UPDATE.
        private void button1_Click_1(object sender, EventArgs e)
        {
            DatabaseManagement.getInstance().updateEmployee(btn_emp_user.Text, btn_emp_pass.Text, eb_name_emp.Text, eb_ap_emp.Text, eb_am_emp.Text, eb_curp_emp.Text, eb_rfc_emp.Text, dtp_nac_emp.Value.ToString("yyyy-MM-dd"), id_emp.Text);
            MessageBox.Show("Empleado actualizado");
            updateDataGrid();
        }

        private void EmployeesWinDtg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            DataGridViewRow row = EmployeesWinDtg.Rows[indexRow];
            if (row == null)
                return;

            btn_emp_user.Text = row.Cells[2].Value.ToString();
            btn_emp_pass.Text = row.Cells[3].Value.ToString();
            eb_name_emp.Text = row.Cells[4].Value.ToString();
            eb_ap_emp.Text = row.Cells[5].Value.ToString();
            eb_am_emp.Text = row.Cells[6].Value.ToString();
            dtp_nac_emp.Text = row.Cells[8].Value.ToString();
            id_emp.Text = row.Cells[0].Value.ToString();

            //Pasar las claves unicas del map a las editbox
            //Este fix es ridiculo pero funciona
            string claves_unicas = row.Cells[7].Value.ToString();
            string[] claves = claves_unicas.Split(':');
            string[] clavevs2 = claves[1].Split(' ');
            string[] elRfc = claves[2].Split(' ');
            eb_rfc_emp.Text = elRfc[1];
            eb_curp_emp.Text = clavevs2[1];
        }

        private void btn_borrar_emp_Click(object sender, EventArgs e)
        {
            DatabaseManagement.getInstance().eraseEmployee(id_emp.Text, btn_emp_user.Text, btn_emp_pass.Text);
            MessageBox.Show("Empleado eliminado");
            updateDataGrid();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            updateDataGrid();
        }

        public void updateDataGrid() {
            List<Employees> empleados = new List<Employees>();
            empleados = DatabaseManagement.getInstance().GetEmployees();

            List<EmployeesDTG> empDataGrid = new List<EmployeesDTG>();
            foreach (var empleado in empleados)
            {
                EmployeesDTG empleadoDTG = new EmployeesDTG();
                empleadoDTG.employee_id = empleado.employee_id;
                empleadoDTG.user_id = empleado.user_id;
                empleadoDTG.user = empleado.user;
                empleadoDTG.password = empleado.password;
                empleadoDTG.name = empleado.name;
                empleadoDTG.last_name = empleado.last_name;
                empleadoDTG.mother_last_name = empleado.mother_last_name;
                foreach (KeyValuePair<string, string> key in empleado.claves_unicas)
                {
                    empleadoDTG.claves_unicas += key.Key.ToString();
                    empleadoDTG.claves_unicas += " : ";
                    empleadoDTG.claves_unicas += key.Value.ToString();
                    empleadoDTG.claves_unicas += " ";
                }
                empleadoDTG.date_of_birth = empleado.date_of_birth.ToString();
                empDataGrid.Add(empleadoDTG);

            }

            EmployeesWinDtg.DataSource = empDataGrid;

            int i = 0;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
