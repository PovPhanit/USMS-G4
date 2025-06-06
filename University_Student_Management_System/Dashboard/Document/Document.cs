using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace University_Student_Management_System.Dashboard.Document
{
    public partial class Document : Form
    {
        public Document()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        bool isCreateUPdate = false;
        bool isLoadBuilding = false;

        public void Fillcbx(ComboBox cbx, string fd1, string fd2, string TB2)
        {
            DA = new SqlDataAdapter("select " + fd1 + "," + fd2 + " From " + TB2, Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            cbx.DataSource = TB;
            cbx.DisplayMember = fd2;
            cbx.ValueMember = fd1;
        }

        private void loadData()
        {
            DA = new SqlDataAdapter("", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;

            TB = new DataTable();
            DA.Fill(TB);
            dgvDocument.DataSource = TB;

            dgvDocument.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvDocument.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDocument.Columns["DocumnetID"].Visible = false;
            dgvDocument.Columns["DocumentTypeID"].Visible = false;

            dgvDocument.Columns["DocumentName"].DisplayIndex = 0;
            dgvDocument.Columns["DocumentDescription"].DisplayIndex = 1;

            foreach (DataGridViewColumn col in dgvDocument.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Document_Load(object sender, EventArgs e)
        {

        }
    }
}
