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
            DA = new SqlDataAdapter("getDocuments", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;

            TB = new DataTable();
            DA.Fill(TB);

            dgvDocument.DataSource = TB;

            dgvDocument.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvDocument.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDocument.Columns["DocumentID"].Visible = false;
            dgvDocument.Columns["DocumentTypeID"].Visible = false;

            dgvDocument.Columns["DocumentName"].DisplayIndex = 0;
            dgvDocument.Columns["DocumentLink"].DisplayIndex = 1;
            dgvDocument.Columns["DocumentDescription"].DisplayIndex = 2;
            dgvDocument.Columns["DocumentDetail"].DisplayIndex = 3;

            foreach (DataGridViewColumn col in dgvDocument.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void Document_Load(object sender, EventArgs e)
        {
            Fillcbx(cbxDocument, "documentTypeID", "documentDetail", "documentType");
            isLoadBuilding = true;

            txtDocumentName.Focus();
            txtSearch.Text = "Search document here...";
            txtSearch.ForeColor = Color.Gray;

            loadData();
        }

        private void dgvDocument_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDocument.Rows[e.RowIndex];

                txtDocumentName.Tag = row.Cells["DocumentID"].Value.ToString();
                txtDocumentName.Text = row.Cells["DocumentName"].Value.ToString();
                txtDocumentLink.Text = row.Cells["DocumentLink"].Value.ToString();
                txtDocumentDescription.Text = row.Cells["DocumentDescription"].Value.ToString();
                cbxDocument.SelectedValue = int.Parse(row.Cells["DocumentTypeID"].Value.ToString());
            }
        }

        private void cbxDocument_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadBuilding) return;

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            isCreateUPdate = false;
            btnNew.BackColor = Color.IndianRed;
            btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
            btnNew.Text = "បោះបង់";
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                txtDocumentName.Text = "";
                txtDocumentLink.Text = "";
                txtDocumentDescription.Text = "";
                txtSearch.Text = "Search document here...";
                txtSearch.ForeColor = Color.Gray;
                isCreateUPdate = true;
            }
            else
            {
                DialogResult re = MessageBox.Show("Do you want to cancel it ?", "Cancel", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (re == DialogResult.OK)
                {
                    btnNew.BackColor = Color.MidnightBlue;
                    btnNew.Image = University_Student_Management_System.Properties.Resources.Add;
                    btnNew.Text = "បង្កើតថ្មី";
                    isCreateUPdate = false;
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDocumentName.Text.Trim()))
            {
                MessageBox.Show("Please enter document title...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumentName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtDocumentLink.Text.Trim()))
            {
                MessageBox.Show("Please enter document link...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumentLink.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtDocumentDescription.Text.Trim()))
            {
                MessageBox.Show("Please enter document description...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumentDescription.Focus();
                return;
            }

            if (txtDocumentName.Tag == null || string.IsNullOrEmpty(txtDocumentName.Tag.ToString()))
            {
                // INSERT Document
                using (SqlCommand com = new SqlCommand("InsertDocument", Operation.con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@DocumentTypeID", cbxDocument.SelectedValue);
                    com.Parameters.AddWithValue("@StaffID", 1); 
                    com.Parameters.AddWithValue("@DocumentName", txtDocumentName.Text.Trim());
                    com.Parameters.AddWithValue("@DocumentLink", txtDocumentLink.Text.Trim());
                    com.Parameters.AddWithValue("@DocumentDescription", txtDocumentDescription.Text.Trim());
                    com.Parameters.AddWithValue("@DocumentCreate", DateTime.Now.Date);

                    com.ExecuteNonQuery();
                }
            }
            else
            {
                // UPDATE Document
                using (SqlCommand com = new SqlCommand("UpdateDocument", Operation.con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@DocumentID", Convert.ToInt32(txtDocumentName.Tag));
                    com.Parameters.AddWithValue("@DocumentTypeID", cbxDocument.SelectedValue);
                    com.Parameters.AddWithValue("@StaffID", 1); 
                    com.Parameters.AddWithValue("@DocumentName", txtDocumentName.Text.Trim());
                    com.Parameters.AddWithValue("@DocumentLink", txtDocumentLink.Text.Trim());
                    com.Parameters.AddWithValue("@DocumentDescription", txtDocumentDescription.Text.Trim());
                    com.Parameters.AddWithValue("@DocumentCreate", DateTime.Now.Date);

                    com.ExecuteNonQuery();
                    MessageBox.Show("Document updated successfully!", "Success");
                }
            }

            // Refresh UI
            loadData();

            if (isCreateUPdate)
            {
                txtDocumentName.Text = "";
                txtDocumentLink.Text = "";
                txtDocumentDescription.Text = "";
                txtDocumentName.Tag = null;

                btnNew.BackColor = Color.MidnightBlue;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Add;
                btnNew.Text = "បង្កើតថ្មី";
                isCreateUPdate = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDocumentName.Tag?.ToString()))
            {
                MessageBox.Show("Please select a document to delete", "No Selection",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this document?", "Confirm Delete",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using (SqlCommand cmd = new SqlCommand("DeleteDocument", Operation.con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DocumentID", Convert.ToInt32(txtDocumentName.Tag.ToString()));

                if (Operation.con.State != ConnectionState.Open)
                    Operation.con.Open();

                cmd.ExecuteNonQuery();

                // Clear and refresh UI
                txtDocumentName.Text = txtDocumentDescription.Text = "";
                txtDocumentName.Tag = null;
                loadData();
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search document here...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search subject here...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            using (SqlCommand cmd = new SqlCommand("SearchDocumentByName", Operation.con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchTerm", txtSearch.Text);

                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataTable TB = new DataTable();
                DA.Fill(TB);

                dgvDocument.DataSource = TB;

                // Formatting the DataGridView
                dgvDocument.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
                dgvDocument.DefaultCellStyle.Font = new Font("Khmer os system", 12);
                dgvDocument.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDocument.Columns["DocumentID"].Visible = false;
                dgvDocument.Columns["DocumentTypeID"].Visible = false;

                foreach (DataGridViewColumn col in dgvDocument.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }
    }
}
