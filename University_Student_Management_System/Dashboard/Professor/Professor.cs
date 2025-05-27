using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace University_Student_Management_System.Dashboard.Professor
{
    public partial class Professor : Form
    {
        public Professor()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Professor_Load(object sender, EventArgs e)
        {
         
            //dgvInformation.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            //dgvInformation.DefaultCellStyle.Font = new Font("Khmer os system", 10);
            //dgvInformation.Columns["ID"].Width = 50;
            //dgvInformation.Columns["Name"].Width = 107;
            //dgvInformation.Columns["Sex"].Width = 50;
            //dgvInformation.Columns["Date Of Birth"].Width = 100;
            //dgvInformation.Columns["Position"].Width = 75;
            //dgvInformation.Columns["Salary"].Width = 80;
            //dgvInformation.Columns["Hired Date"].Width = 80;
            //dgvInformation.Columns["Address"].Width = 70;
            //dgvInformation.Columns["Contact"].Width = 85;
            //dgvInformation.Columns["Photo"].Width = 75;
            //dgvInformation.Columns["Date Of Birth"].DefaultCellStyle.Format = "dd/MM/yy";
            //dgvInformation.Columns["Hired Date"].DefaultCellStyle.Format = "dd/MM/yy";
            //dgvInformation.Columns["Salary"].DefaultCellStyle.Format = "c";
            //DataGridViewImageColumn img = new DataGridViewImageColumn();
            //img = (DataGridViewImageColumn)dgvInformation.Columns["Photo"];
            //img.ImageLayout = DataGridViewImageCellLayout.Stretch;
            //foreach (DataGridViewColumn col in dgvInformation.Columns)
            //{
            //    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }
    }
}
