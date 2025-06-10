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
using University_Student_Management_System.Dashboard.ExamType;

namespace University_Student_Management_System.Dashboard.Department
{
    public partial class Department : Form
    {
        public Department()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        bool isCreateUPdate = false;

        private void Department_Load(object sender, EventArgs e)
        {
            loadData();
            txtSearch.Text = "Search department hear...";
            txtSearch.ForeColor = Color.Gray;
        }
        private void loadData()
        {
            LSVDepartment.View = View.Details;
            LSVDepartment.FullRowSelect = true;
            LSVDepartment.Clear();
            LSVDepartment.Columns.Add("Department", 200);
            LSVDepartment.Columns.Add("Price", 150);
            LSVDepartment.Columns.Add("Description", 150);

            DA = new SqlDataAdapter("SELECT departmentID, departmentName, departmentPrice,departmentDescription FROM department", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            foreach (DataRow row in TB.Rows)
            {
                ListViewItem item = new ListViewItem(row["departmentName"].ToString());
                item.SubItems.Add(row["departmentPrice"].ToString());
                item.SubItems.Add(row["departmentDescription"].ToString());
                item.Tag = row["departmentID"].ToString();
                LSVDepartment.Items.Add(item);
            }
            foreach (ListViewItem list in LSVDepartment.Items)
            {
                if (list.Index % 2 == 0)
                {
                    list.BackColor = Color.White;
                }
                else
                {
                    list.BackColor = Color.LightSteelBlue;
                }
            }
        }

        private void LSVDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isCreateUPdate)
            {
                if (LSVDepartment.SelectedItems.Count > 0)
                {
                    var item = LSVDepartment.SelectedItems[0];
                    txtID.Text = item.Tag.ToString();
                    txtDepartment.Text = item.SubItems[0].Text;
                    txtPrice.Text = item.SubItems[1].Text;
                    txtDescription.Text = item.SubItems[2].Text;
                }
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
                txtSearch.Text = "Search department hear...";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បោះបង់")
            {
                if (string.IsNullOrEmpty(txtDepartment.Text.Trim()))
                {
                    MessageBox.Show("Please Input Department...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDepartment.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPrice.Text.Trim()))
                {
                    MessageBox.Show("Please Input price...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
                {
                    MessageBox.Show("Please Input Description...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDescription.Focus();
                    return;
                }
                if (isCreateUPdate)
                {
                    SqlCommand com = new SqlCommand("INSERT INTO department (departmentName, departmentPrice, departmentDescription) VALUES (@name, @price, @desc)", Operation.con);
                    com.Parameters.AddWithValue("@name", txtDepartment.Text);
                    com.Parameters.AddWithValue("@price", decimal.Parse(txtPrice.Text));
                    com.Parameters.AddWithValue("@desc", txtDescription.Text);
                    int rowEffect = com.ExecuteNonQuery();

                    loadData();
                  
                }
                else
                {
                    SqlCommand com = new SqlCommand("UPDATE department SET departmentName = @name, departmentPrice = @price, departmentDescription = @desc WHERE departmentID = @id",Operation.con);
                    com.Parameters.AddWithValue("@name", txtDepartment.Text);
                    com.Parameters.AddWithValue("@price", decimal.Parse(txtPrice.Text));
                    com.Parameters.AddWithValue("@desc", txtDescription.Text);
                    com.Parameters.AddWithValue("@id", int.Parse(txtID.Text)); 
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();
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
                SqlCommand com = new SqlCommand("DELETE FROM department WHERE departmentID = @id", Operation.con);
                com.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                int rowsAffected = com.ExecuteNonQuery();
                loadData();
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search department hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search department hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            LSVDepartment.View = View.Details;
            LSVDepartment.FullRowSelect = true;
            LSVDepartment.Clear();
            LSVDepartment.Columns.Add("Department", 200);
            LSVDepartment.Columns.Add("Price", 150);
            LSVDepartment.Columns.Add("Description", 150);

            SqlDataAdapter DA = new SqlDataAdapter("SELECT departmentID, departmentName, departmentPrice, departmentDescription FROM department WHERE LOWER(departmentName) LIKE @name", Operation.con);
            DA.SelectCommand.Parameters.AddWithValue("@name", "%" + txtSearch.Text.ToLower() + "%");
            DataTable TB = new DataTable();
            DA.Fill(TB);
            foreach (DataRow row in TB.Rows)
            {
                ListViewItem item = new ListViewItem(row["departmentName"].ToString());
                item.SubItems.Add(row["departmentPrice"].ToString());
                item.SubItems.Add(row["departmentDescription"].ToString());
                item.Tag = row["departmentID"].ToString();
                LSVDepartment.Items.Add(item);
            }
            foreach (ListViewItem list in LSVDepartment.Items)
            {
                if (list.Index % 2 == 0)
                {
                    list.BackColor = Color.White;
                }
                else
                {
                    list.BackColor = Color.LightSteelBlue;
                }
            }    
        }

        private void txtDepartment_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtDescription, txtPrice);
        }

        private void txtPrice_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtDepartment, txtDescription);
        }

        private void txtDescription_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtPrice, txtDepartment);
        }
    }
}
