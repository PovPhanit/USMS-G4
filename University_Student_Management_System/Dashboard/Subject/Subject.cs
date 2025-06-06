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
using University_Student_Management_System.Dashboard.Department;

namespace University_Student_Management_System.Dashboard.Subject
{
    public partial class Subject : Form
    {
        public Subject()
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

        private void DisplaySubject(int departmentID)   
        {
            DA = new SqlDataAdapter("SELECT DepartmentID, SubjectID, SubjectTitle, SubjectDescription, DepartmentName " +
                            "FROM Department_Detail " +
                            "WHERE DepartmentID = " + departmentID, Operation.con);
            TB = new DataTable();
            Image image = Image.FromFile("../../Resources/roomDisplay.png"); 
            DA.Fill(TB); 
            subjectContainer.Controls.Clear(); 

            FlowLayoutPanel flow = new FlowLayoutPanel(); 
            flow.Dock = DockStyle.Fill;
            flow.WrapContents = true;
            flow.AutoScroll = true;
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.Padding = new Padding(3);

            foreach (DataRow dr in TB.Rows) 
            {
                Panel itemPanel = new Panel(); 
                itemPanel.Size = new Size(130, 110);
                itemPanel.Margin = new Padding(5);

                PictureBox pic = new PictureBox();
                pic.Image = image;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(70, 70);
                pic.Location = new Point(30, 0);

                if (!isCreateUPdate)
                {
                    pic.Click += (s, e1) =>
                    {
                        txtSubjectTitle.Tag = dr["SubjectID"].ToString();
                        txtSubjectTitle.Text = dr["SubjectTitle"].ToString();
                        txtSubjectDescription.Text = dr["SubjectDescription"].ToString();
                        cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                    };
                }

                Label lblSubjectTitle = new Label(); 
                lblSubjectTitle.Text = dr["SubjectTitle"].ToString();
                lblSubjectTitle.ForeColor = Color.Black;
                lblSubjectTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblSubjectTitle.TextAlign = ContentAlignment.MiddleCenter;
                lblSubjectTitle.Size = new Size(70, 20);
                lblSubjectTitle.Location = new Point(30, 72);

                if (!isCreateUPdate)
                {
                    lblSubjectTitle.Click += (s, e2) =>
                    {
                        txtSubjectTitle.Tag = dr["SubjectID"].ToString();
                        txtSubjectTitle.Text = dr["SubjectTitle"].ToString();
                        txtSubjectDescription.Text = dr["SubjectDescription"].ToString();
                        cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                    };
                }

                Label lblDepartment = new Label();
                lblDepartment.Text = dr["DepartmentName"].ToString();
                lblDepartment.ForeColor = Color.Black;
                lblDepartment.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblDepartment.TextAlign = ContentAlignment.MiddleCenter;
                lblDepartment.Size = new Size(70, 20);
                lblDepartment.Location = new Point(30, 90);

                if (!isCreateUPdate)
                {
                    lblDepartment.Click += (s, e3) =>
                    {
                        txtSubjectTitle.Tag = dr["SubjectID"].ToString();
                        txtSubjectTitle.Text = dr["SubjectTitle"].ToString();
                        txtSubjectDescription.Text = dr["SubjectDescription"].ToString();
                        cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                    };
                }

                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblSubjectTitle);
                itemPanel.Controls.Add(lblDepartment);

                flow.Controls.Add(itemPanel);
            }

            subjectContainer.Controls.Add(flow);
        }

        private void loadData()
        {
            DA = new SqlDataAdapter("getAllSubjectsWithDepartments", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;

            TB = new DataTable();
            DA.Fill(TB);
            dgvSubject.DataSource = TB;

            dgvSubject.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvSubject.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvSubject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvSubject.Columns["SubjectID"].Visible = false;
            dgvSubject.Columns["DepartmentID"].Visible = false;

            dgvSubject.Columns["DepartmentName"].DisplayIndex = 0;
            dgvSubject.Columns["SubjectTitle"].DisplayIndex = 1;
            dgvSubject.Columns["SubjectDescription"].DisplayIndex = 2;

            foreach (DataGridViewColumn col in dgvSubject.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        
        private void Subject_Load(object sender, EventArgs e)
        {
            Fillcbx(cbxDepartment, "departmentID", "departmentName", "department");
            isLoadBuilding = true;

            txtSubjectTitle.Focus();
            txtSearch.Text = "Search subject here...";
            txtSearch.ForeColor = Color.Gray;

            DisplaySubject(int.Parse(cbxDepartment.SelectedValue.ToString()));
            loadData();
        }

        private void dgvSubject_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSubject.Rows[e.RowIndex];

                txtSubjectTitle.Tag = row.Cells["SubjectID"].Value.ToString();
                txtSubjectTitle.Text = row.Cells["SubjectTitle"].Value.ToString();
                txtSubjectDescription.Text = row.Cells["SubjectDescription"].Value.ToString();
                cbxDepartment.SelectedValue = int.Parse(row.Cells["DepartmentID"].Value.ToString());
            }
        }
        
        private void cbxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadBuilding) return; 

            DisplaySubject(int.Parse(cbxDepartment.SelectedValue.ToString()));
        }



        // Create new subject or cancel operation
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                txtSubjectTitle.Text = "";
                txtSubjectDescription.Text = "";
                txtSearch.Text = "Search subject here...";
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            isCreateUPdate = false;
            btnNew.BackColor = Color.IndianRed;
            btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
            btnNew.Text = "បោះបង់";
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSubjectTitle.Text.Trim()))
            {
                MessageBox.Show("Please enter subject title...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSubjectTitle.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtSubjectDescription.Text.Trim()))
            {
                MessageBox.Show("Please enter subject description...", "Missing",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSubjectDescription.Focus();
                return;
            }

            if (txtSubjectTitle.Tag == null || string.IsNullOrEmpty(txtSubjectTitle.Tag.ToString()))
            {
                // INSERT Subject
                using (SqlCommand com = new SqlCommand("insertSubjectDepartment", Operation.con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@DepartmentID",
                        int.Parse(cbxDepartment.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@SubjectTitle",
                        txtSubjectTitle.Text.Trim());
                    com.Parameters.AddWithValue("@SubjectDescription",
                        txtSubjectDescription.Text.Trim());

                    if (Operation.con.State != ConnectionState.Open)
                        Operation.con.Open();

                    int newSubjectId = Convert.ToInt32(com.ExecuteScalar());
                    txtSubjectTitle.Tag = newSubjectId.ToString();
                }
            }
            else
            {
                // UPDATE Subject
                using (SqlCommand com = new SqlCommand("updateSubjectDepartment", Operation.con))
                {
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@SubjectID",
                        Convert.ToInt32(txtSubjectTitle.Tag.ToString()));
                    com.Parameters.AddWithValue("@DepartmentID",
                        int.Parse(cbxDepartment.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@SubjectTitle",
                        txtSubjectTitle.Text.Trim());
                    com.Parameters.AddWithValue("@SubjectDescription",
                        txtSubjectDescription.Text.Trim());

                    if (Operation.con.State != ConnectionState.Open)
                        Operation.con.Open();

                    com.ExecuteNonQuery();
                }
            }

            // Refresh UI
            loadData();
            DisplaySubject(int.Parse(cbxDepartment.SelectedValue.ToString()));

            if (isCreateUPdate)
            {
                txtSubjectTitle.Text = "";
                txtSubjectDescription.Text = "";
                txtSubjectTitle.Tag = null;

                btnNew.BackColor = Color.MidnightBlue;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Add;
                btnNew.Text = "បង្កើតថ្មី";
                isCreateUPdate = false;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSubjectTitle.Tag?.ToString()))
            {
                MessageBox.Show("Please select a subject to delete", "No Selection",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this subject?", "Confirm Delete",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            using (SqlCommand cmd = new SqlCommand("deleteSubjectDepartment", Operation.con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubjectID", Convert.ToInt32(txtSubjectTitle.Tag.ToString()));

                if (Operation.con.State != ConnectionState.Open)
                    Operation.con.Open();

                cmd.ExecuteNonQuery();

                // Clear and refresh UI
                txtSubjectTitle.Text = txtSubjectDescription.Text = "";
                txtSubjectTitle.Tag = null;
                loadData();
                DisplaySubject(int.Parse(cbxDepartment.SelectedValue.ToString()));
            }
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search subject here...")
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
            using (SqlCommand cmd = new SqlCommand("SearchSubjectDepartment", Operation.con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubjectTitle", txtSearch.Text);

                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataTable TB = new DataTable();
                DA.Fill(TB);

                dgvSubject.DataSource = TB;

                // Formatting the DataGridView
                dgvSubject.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
                dgvSubject.DefaultCellStyle.Font = new Font("Khmer os system", 12);
                dgvSubject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSubject.Columns["DepartmentID"].Visible = false;
                dgvSubject.Columns["SubjectID"].Visible = false;

                foreach (DataGridViewColumn col in dgvSubject.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }
    }
}
