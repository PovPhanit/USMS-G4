﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using University_Student_Management_System.Dashboard.Level;

namespace University_Student_Management_System.Dashboard.DocumentType
{
    public partial class DocumentType : Form
    {
        public DocumentType()
        {
            InitializeComponent();
        }

        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        bool isLoaded = false;
        bool isCreateUPdate = false;
        private void DocumentType_Load(object sender, EventArgs e)
        {
            loadData();
            isLoaded = true;
            txtSearch.Text = "Search documenttype hear...";
            txtSearch.ForeColor = Color.Gray;
        }
        private void loadData()
        {
            DA = new SqlDataAdapter("select documentTypeID,documentDetail from documenttype", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            LBDocType.DataSource = null;
            LBDocType.Items.Clear();
            LBDocType.DataSource = TB;
            LBDocType.DisplayMember = "documentDetail";
            LBDocType.ValueMember = "documentTypeID";
        }

        private void LBDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded) return;

            if (!isCreateUPdate)
            {
                txtID.Text = LBDocType.SelectedValue.ToString();
                txtDocType.Text = LBDocType.Text;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                ControlForm.ClearData(this);
         
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បោះបង់")
            {
                if (string.IsNullOrEmpty(txtDocType.Text.Trim()))
                {
                    MessageBox.Show("Please Input ID...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDocType.Focus();
                    return;
                }
                if (isCreateUPdate)
                {
                    com = new SqlCommand("insert into documenttype(documentDetail) values(N'" + txtDocType.Text + "')", Operation.con);
                    int rowEffect = com.ExecuteNonQuery();
                    isLoaded = false;
                    loadData();
                    isLoaded = true;
                    //while (sdr.Read())
                    //{
                    //    MessageBox.Show(sdr["semesterName"].ToString());
                    //}
                    //sdr.Dispose();
                }
                else
                {
                    com = new SqlCommand("update documenttype set documentDetail = N'" + txtDocType.Text + "' where documentTypeID = " + Convert.ToInt32(txtID.Text) + "", Operation.con);
                    int rowEffect = com.ExecuteNonQuery();
                    isLoaded = false;
                    loadData();
                    isLoaded = true;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isCreateUPdate = false;
            btnNew.BackColor = Color.IndianRed;
            btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
            btnNew.Text = "បោះបង់";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text.Trim()))
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                com = new SqlCommand("delete from documenttype where documenttypeID = " + Convert.ToInt32(txtID.Text) + "", Operation.con);
                int rowEffect = com.ExecuteNonQuery();
                isLoaded = false;
                loadData();
                isLoaded = true;
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search documenttype hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search documenttype hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            isLoaded = false;
            DA = new SqlDataAdapter("select documenttypeID,documentDetail from documenttype where LOWER(documentDetail) like '%" + txtSearch.Text.ToLower() + "%'", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            LBDocType.DataSource = null;
            LBDocType.Items.Clear();
            LBDocType.DataSource = TB;
            LBDocType.DisplayMember = "documentDetail";
            LBDocType.ValueMember = "documenttypeID";
            isLoaded = true;
        }
    }
}
