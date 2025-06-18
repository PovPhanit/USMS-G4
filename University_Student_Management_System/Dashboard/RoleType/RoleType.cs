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
using University_Student_Management_System.Dashboard.Level;

namespace University_Student_Management_System.Dashboard.RoleType
{
    public partial class RoleType : Form
    {
        public RoleType()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        bool isLoaded = false;
        bool isCreateUPdate = false;
        private void RoleType_Load(object sender, EventArgs e)
        {
            loadData();
            isLoaded = true;
            txtSearch.Text = "Search roletype hear...";
            txtSearch.ForeColor = Color.Gray;
        }
        private void loadData()
        {
            DA = new SqlDataAdapter("select roleID,roleType from role", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            LBRoleType.DataSource = null;
            LBRoleType.Items.Clear();
            LBRoleType.DataSource = TB;
            LBRoleType.DisplayMember = "roleType";
            LBRoleType.ValueMember = "roleID";
        }

        private void LBRoleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded) return;

            if (!isCreateUPdate)
            {
                txtID.Text = LBRoleType.SelectedValue.ToString();
                txtRoleType.Text = LBRoleType.Text;
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
                if (string.IsNullOrEmpty(txtRoleType.Text.Trim()))
                {
                    MessageBox.Show("Please Input ID...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRoleType.Focus();
                    return;
                }
                if (isCreateUPdate)
                {
                    com = new SqlCommand("insert into role(roleType) values(N'" + txtRoleType.Text + "')", Operation.con);
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
                    com = new SqlCommand("update role set roleType = N'" + txtRoleType.Text + "' where roleID = " + Convert.ToInt32(txtID.Text) + "", Operation.con);
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
                com = new SqlCommand("delete from role where roleID = " + Convert.ToInt32(txtID.Text) + "", Operation.con);
                int rowEffect = com.ExecuteNonQuery();
                isLoaded = false;
                loadData();
                isLoaded = true;
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search roletype hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search roletype hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            isLoaded = false;
            DA = new SqlDataAdapter("select roleID,roleType from role where LOWER(roleType) like '%" + txtSearch.Text.ToLower() + "%'", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            LBRoleType.DataSource = null;
            LBRoleType.Items.Clear();
            LBRoleType.DataSource = TB;
            LBRoleType.DisplayMember = "roleType";
            LBRoleType.ValueMember = "roleID";
            isLoaded = true;
        }
    }
}
