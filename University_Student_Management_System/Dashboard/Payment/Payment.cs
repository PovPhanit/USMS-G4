using Project3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace University_Student_Management_System.Dashboard.Payment
{
    public partial class Payment : Form
    {
        public Payment()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        bool isCreateUPdate = false;

        private void loadData()
        {
            DA = new SqlDataAdapter("select * from viewPayment order by PaymentID desc", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            dgvInvoice.DataSource = TB;
            dgvInvoice.ColumnHeadersHeight = 40;
            dgvInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvInvoice.RowTemplate.Height = 100;
            foreach (DataGridViewRow row in dgvInvoice.Rows)
            {
                row.Height = 100;
            }
            dgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvInvoice.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInvoice.Columns["PaymentID"].Visible = false;
            dgvInvoice.Columns["InvoiceID"].Visible = false;
            dgvInvoice.Columns["StaffID"].Visible = false;
            dgvInvoice.Columns["Name KH"].Width = 100;
            dgvInvoice.Columns["Name EN"].Width = 100;
            dgvInvoice.Columns["Price"].DefaultCellStyle.Format = "c";

            foreach (DataGridViewColumn col in dgvInvoice.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }
        private void Payment_Load(object sender, EventArgs e)
        {
            txtInvoiceNumber.Focus();
            txtSearch.Text = "Search payment hear...";
            txtIDCard.Text = "Find invoice...";
            txtSearch.ForeColor = Color.Gray;
            txtIDCard.ForeColor = Color.Gray;
            loadData();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search payment hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search payment hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("SearchPaymentStudent", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@Keyword", txtSearch.Text.Trim());
            TB = new DataTable();
            DA.Fill(TB);
            dgvInvoice.DataSource = TB;
        }

        private void dgvInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate)
            {
                if (e.RowIndex >= 0) // Make sure it's not a header
                {
                    DataGridViewRow row = dgvInvoice.Rows[e.RowIndex];
                    var price = decimal.Parse(row.Cells["Price"].Value.ToString(), NumberStyles.Currency);
                    txtInvoiceNumber.Text = row.Cells["Invoice Number"].Value.ToString();
                    txtInvoiceNumber.Tag = row.Cells["PaymentID"].Value.ToString();
                    txtInvoiceDetail.Text = row.Cells["Description"].Value.ToString();
                    txtInvoicePrice.Text = price.ToString("N2");
                    txtIDCard.Text = row.Cells["ID Card"].Value.ToString();

                  

                    SqlCommand com = new SqlCommand("invoiceNumberForDisplayUser", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@InvoiceNumber", txtInvoiceNumber.Text.Trim());
                    SqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        txtNameKH.Text = dr["StudentNameKH"].ToString();
                        txtIDCardShow.Text = dr["StudentICard"].ToString();
                        txtGender.Text = dr["StudentGender"].ToString();
                        txtDepartment.Text = dr["DepartmentName"].ToString();
                        txtLevel.Text = dr["LevelName"].ToString();
                        txtClassName.Text = dr["ClassName"].ToString();

                        //txtIDCard.Text = dr["StudentICard"].ToString();

                    }

                    dr.Close();

                }
            }
        }
        private void hideStudentIDCard(Boolean b)
        {
            Color bgColor = b ? Color.White : Color.Silver;
            txtIDCard.Enabled = b;
            txtIDCard.BackColor = bgColor;
        }
        private void radioCard_CheckedChanged(object sender, EventArgs e)
        {
            if (radioCard.Checked)
            {

                hideStudentIDCard(true);
            }
        }

        private void radioInvoice_CheckedChanged(object sender, EventArgs e)
        {
            if (radioInvoice.Checked)
            {
                hideStudentIDCard(false);
            }
        }

        private void txtIDCard_Enter(object sender, EventArgs e)
        {
            if (txtIDCard.Text == "Find invoice...")
            {
                txtIDCard.ForeColor = Color.Black;
                txtIDCard.Text = "";
            }
        }

        private void txtIDCard_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtIDCard.Text.Trim()))
            {
                txtIDCard.Text = "Find invoice...";
                txtIDCard.ForeColor = Color.Gray;
            }
        }

        private void txtIDCard_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtIDCard.Text.ToString().Trim().Length > 0)
            {
                DA = new SqlDataAdapter();
                DA.SelectCommand = new SqlCommand("showAllInvoiceByIDStudent", Operation.con);
                DA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DA.SelectCommand.Parameters.AddWithValue("@IDCard", txtIDCard.Text.Trim());
                TB = new DataTable();
                DA.Fill(TB);
                dgvListInvoice.DataSource = TB;
                dgvListInvoice.ColumnHeadersHeight = 40;
                dgvListInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                dgvListInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
                dgvListInvoice.DefaultCellStyle.Font = new Font("Khmer os system", 12);
                dgvListInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvListInvoice.Columns["InvoiceID"].Visible = false;
                dgvListInvoice.Columns["ClassroomID"].Visible = false;
                dgvListInvoice.Columns["StaffID"].Visible = false;
                dgvListInvoice.Columns["ID Card"].Visible = false;
                dgvListInvoice.Columns["Date"].Visible = false;
                dgvListInvoice.Columns["Description"].Visible = false;
                dgvListInvoice.Columns["Department"].Visible = false;
                dgvListInvoice.Columns["Level"].Visible = false;
                dgvListInvoice.Columns["Class"].Visible = false;

                dgvListInvoice.Columns["Name KH"].Width = 100;
                dgvListInvoice.Columns["Name EN"].Width = 100;
                dgvListInvoice.Columns["Price"].DefaultCellStyle.Format = "c";
                dgvListInvoice.Columns["Paid"].DefaultCellStyle.Format = "c";

                foreach (DataGridViewColumn col in dgvListInvoice.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            else
            {
                dgvListInvoice.DataSource = null;
            }
        }

        private void txtIDCard_TextChanged(object sender, EventArgs e)
        {
            if (txtIDCard.Text.ToString().Trim().Length > 0 && txtIDCard.Text.ToString().Trim() != "Find invoice...")
            {
                DA = new SqlDataAdapter();
                DA.SelectCommand = new SqlCommand("showAllInvoiceByIDStudent", Operation.con);
                DA.SelectCommand.CommandType = CommandType.StoredProcedure;
                DA.SelectCommand.Parameters.AddWithValue("@IDCard", txtIDCard.Text.Trim());
                TB = new DataTable();
                DA.Fill(TB);
                dgvListInvoice.DataSource = TB;
                dgvListInvoice.ColumnHeadersHeight = 40;
                dgvListInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                dgvListInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
                dgvListInvoice.DefaultCellStyle.Font = new Font("Khmer os system", 12);
                dgvListInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvListInvoice.Columns["InvoiceID"].Visible = false;
                dgvListInvoice.Columns["ClassroomID"].Visible = false;
                dgvListInvoice.Columns["StaffID"].Visible = false;
                dgvListInvoice.Columns["ID Card"].Visible = false;
                dgvListInvoice.Columns["Date"].Visible = false;
                dgvListInvoice.Columns["Description"].Visible = false;
                dgvListInvoice.Columns["Department"].Visible = false;
                dgvListInvoice.Columns["Level"].Visible = false;
                dgvListInvoice.Columns["Class"].Visible = false;

                dgvListInvoice.Columns["Name KH"].Width = 100;
                dgvListInvoice.Columns["Name EN"].Width = 100;
                dgvListInvoice.Columns["Price"].DefaultCellStyle.Format = "c";
                dgvListInvoice.Columns["Paid"].DefaultCellStyle.Format = "c";

                foreach (DataGridViewColumn col in dgvListInvoice.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
            else
            {
                dgvListInvoice.DataSource = null;
            }
        }

        private void txtInvoiceNumber_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtInvoicePrice, txtInvoiceDetail);
            if (txtInvoiceNumber.Text.ToString().Trim().Length > 0)
            {
                SqlCommand com = new SqlCommand("invoiceNumberForDisplayUser", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@InvoiceNumber", txtInvoiceNumber.Text.Trim());
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    txtNameKH.Text = dr["StudentNameKH"].ToString();
                    txtIDCardShow.Text= dr["StudentICard"].ToString();
                    txtGender.Text= dr["StudentGender"].ToString();
                    txtDepartment.Text= dr["DepartmentName"].ToString();
                    txtLevel.Text= dr["LevelName"].ToString();
                    txtClassName.Text = dr["ClassName"].ToString();

                 

                }
                dr.Close();
                txtIDCard.Text = txtIDCardShow.Text.ToString();
            }
            else
            {
                txtIDCard.Text = "Find invoice...";
                txtIDCard.ForeColor = Color.Gray;
                dgvListInvoice.DataSource= null;
                txtNameKH.Text = "";
                txtIDCardShow.Text = "";
                txtGender.Text = "";
                txtDepartment.Text = "";
                txtLevel.Text = "";
                txtClassName.Text = "";
            }

        }

        private void dgvListInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
      
                if (e.RowIndex >= 0) // Make sure it's not a header
                {
                    DataGridViewRow row = dgvListInvoice.Rows[e.RowIndex];
              
                    txtInvoiceNumber.Text = row.Cells["Invoice"].Value.ToString();


                    txtNameKH.Text = row.Cells["Name KH"].Value.ToString();
                    txtIDCardShow.Text = row.Cells["ID Card"].Value.ToString();
                    txtGender.Text = row.Cells["Gender"].Value.ToString();
                    txtDepartment.Text = row.Cells["Department"].Value.ToString();
                    txtLevel.Text = row.Cells["Level"].Value.ToString();
                    txtClassName.Text = row.Cells["Class"].Value.ToString();
                }
            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                txtNameKH.Text = "";
                txtIDCardShow.Text = "";
                txtGender.Text = "";
                txtDepartment.Text = "";
                txtLevel.Text = "";
                txtClassName.Text = "";
                txtInvoiceDetail.Text = "";
                txtInvoiceNumber.Text = "";
                txtInvoicePrice.Text = "";
                txtInvoiceNumber.Focus();
         
                isCreateUPdate = true;
            }
            else
            {
                DialogResult re = new DialogResult();
                re = MessageBox.Show("Do you want to cancel it ?", "Cancel", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (re == DialogResult.OK)
                {
                    btnNew.BackColor = Color.MidnightBlue;
                    btnNew.Image = University_Student_Management_System.Properties.Resources.Add;
                    btnNew.Text = "បង្កើតថ្មី";
                    isCreateUPdate = false;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isCreateUPdate = false;
            btnNew.BackColor = Color.IndianRed;
            btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
            btnNew.Text = "បោះបង់";
            txtNameKH.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បោះបង់")
            {
                if (string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()))
                {
                    MessageBox.Show("Please Input invoice number...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtInvoiceNumber.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtInvoicePrice.Text.Trim()))
                {
                    MessageBox.Show("Please Input price...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtInvoicePrice.Focus();
                    return;
                }
              

                if (isCreateUPdate)
                {
                    Operation.con.InfoMessage -= OnSqlInfoMessage;
                    Operation.con.InfoMessage += OnSqlInfoMessage;
                    // Set this just once too
                    Operation.con.FireInfoMessageEventOnUserErrors = true;
                    // Define the handler once
                    void OnSqlInfoMessage(object sender1, SqlInfoMessageEventArgs e1)
                    {
                        MessageBox.Show(e1.Message, "SQL Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    SqlCommand com = new SqlCommand("insertPayment", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@InvoiceNumber", txtInvoiceNumber.Text.ToString());
                    com.Parameters.AddWithValue("@StaffID",storeAuthorization.id);
                 
                    com.Parameters.AddWithValue("@PaymentDescription", txtInvoiceDetail.Text.ToString());
                    com.Parameters.AddWithValue("@PaymentPrice", decimal.Parse(txtInvoicePrice.Text.ToString()));

                    ControlForm.ClearData(this);
                    txtInvoiceNumber.Focus();
                    txtSearch.Text = "Search payment hear...";
                    txtIDCard.Text = "Find invoice...";
                    txtSearch.ForeColor = Color.Gray;
                    txtIDCard.ForeColor = Color.Gray;
                    dgvListInvoice.DataSource = null;
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();
                }
                else
                {
                    SqlCommand com = new SqlCommand("updatePayment", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@PaymentID", int.Parse(txtInvoiceNumber.Tag.ToString()));
                    com.Parameters.AddWithValue("@InvoiceNumber", txtInvoiceNumber.Text.ToString());
                    com.Parameters.AddWithValue("@StaffID", storeAuthorization.id);

                    com.Parameters.AddWithValue("@PaymentDescription", txtInvoiceDetail.Text.ToString());
                    com.Parameters.AddWithValue("@PaymentPrice", decimal.Parse(txtInvoicePrice.Text.ToString()));

                    ControlForm.ClearData(this);
                    txtInvoiceNumber.Focus();
                    txtSearch.Text = "Search payment hear...";
                    txtIDCard.Text = "Find invoice...";
                    txtSearch.ForeColor = Color.Gray;
                    txtIDCard.ForeColor = Color.Gray;
                    dgvListInvoice.DataSource = null;
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();

                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtInvoiceNumber.Tag == null)
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("deletePayment", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@PaymentID", int.Parse(txtInvoiceNumber.Tag.ToString()));
                com.Parameters.AddWithValue("@InvoiceNumber", txtInvoiceNumber.Text.ToString());

                int rowEffect = com.ExecuteNonQuery();

                ControlForm.ClearData(this);
                txtInvoiceNumber.Focus();
                txtSearch.Text = "Search payment hear...";
                txtIDCard.Text = "Find invoice...";
                txtSearch.ForeColor = Color.Gray;
                txtIDCard.ForeColor = Color.Gray;
                dgvListInvoice.DataSource = null;
                txtInvoiceNumber.Tag = null;
                loadData();
            }
        }

        private void txtInvoiceDetail_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtInvoiceNumber, txtInvoicePrice);
        }

        private void txtInvoicePrice_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtInvoiceDetail, txtInvoiceNumber);
        }
    }
}
