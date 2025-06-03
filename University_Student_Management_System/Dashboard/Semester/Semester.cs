using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
using System.Xml.Linq;

namespace University_Student_Management_System.Dashboard.Semester
{
    public partial class Semester : Form
    {
        public Semester()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        bool isLoaded = false;
        bool isCreateUPdate = false;
        private void Semester_Load(object sender, EventArgs e)
        {
            loadData();
            isLoaded = true;
            txtSearch.Text = "Search semester hear...";
            txtSearch.ForeColor = Color.Gray;
      
        }
        private void loadData()
        {
            DA = new SqlDataAdapter("select semesterID,semesterName from Semester", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            LBSemester.DataSource = null;
            LBSemester.Items.Clear();
            LBSemester.DataSource = TB;
            LBSemester.DisplayMember = "semesterName";
            LBSemester.ValueMember = "semesterID";
        }
        private void LBSemester_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!isLoaded) return;

            if (!isCreateUPdate)
            {
                txtID.Text = LBSemester.SelectedValue.ToString();
                txtSemester.Text = LBSemester.Text;
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
                txtSearch.Text = "Search semester hear...";
                txtSearch.ForeColor = Color.Gray;
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

        private void Modify(string x)//new
        {
            com = new SqlCommand(x, Operation.con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@i", txtID.Text);
            com.ExecuteNonQuery();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {

          

            if (btnNew.Text == "បោះបង់")
            {
                if (string.IsNullOrEmpty(txtSemester.Text.Trim()))
                {
                    MessageBox.Show("Please Input ID...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSemester.Focus();
                    return;
                }
                if (isCreateUPdate)
                {
                    com = new SqlCommand("insert into semester(semesterName) values(N'" + txtSemester.Text + "')", Operation.con);
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
                    com = new SqlCommand("update semester set semesterName = N'"+ txtSemester.Text + "' where semesterID = "+Convert.ToInt32(txtID.Text) +"", Operation.con);
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
                com = new SqlCommand("delete from semester where semesterID = " + Convert.ToInt32(txtID.Text) + "", Operation.con);
                int rowEffect = com.ExecuteNonQuery();
                isLoaded = false;
                loadData();
                isLoaded = true;
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if(txtSearch.Text== "Search semester hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search semester hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            isLoaded = false;
            DA = new SqlDataAdapter("select semesterID,semesterName from Semester where semesterName like '%"+txtSearch.Text+"%'", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            LBSemester.DataSource = null;
            LBSemester.Items.Clear();
            LBSemester.DataSource = TB;
            LBSemester.DisplayMember = "semesterName";
            LBSemester.ValueMember = "semesterID";
            isLoaded = true;
        }
    }
}
