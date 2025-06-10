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
using University_Student_Management_System.Dashboard.Building;
using University_Student_Management_System.Dashboard.Department;
using static System.ComponentModel.Design.ObjectSelectorEditor;

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
            DA.SelectCommand = new SqlCommand("SearchSubjectbyDepartment", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@DepartmentID", departmentID);
            TB = new DataTable();
            Image image = Image.FromFile("../../Resources/bookDisplay.png"); 
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
                itemPanel.Size = new Size(170, 72);
                itemPanel.Margin = new Padding(5);

                PictureBox pic = new PictureBox();
                pic.Image = image;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(70, 50);
                pic.Location = new Point(50, 0);

                if (!isCreateUPdate)
                {
                    pic.Click += (s, e1) =>
                    {
                        txtSubjectDesc.Text = dr["SubjectDescription"].ToString();
                        cbxSubject.SelectedValue = int.Parse(dr["SubjectID"].ToString());
                        cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                    };
                }

                Label lblSubjectTitle = new Label(); 
                lblSubjectTitle.Text = dr["SubjectTitle"].ToString();
                lblSubjectTitle.ForeColor = Color.Black;
                lblSubjectTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblSubjectTitle.TextAlign = ContentAlignment.MiddleCenter;
                lblSubjectTitle.Size = new Size(170, 20);
                lblSubjectTitle.Location = new Point(0, 52);

                if (!isCreateUPdate)
                {
                    lblSubjectTitle.Click += (s, e2) =>
                    {
                
                    };
                }



                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblSubjectTitle);

                flow.Controls.Add(itemPanel);
            }

            subjectContainer.Controls.Add(flow);
        }

        private void loadData()
        {
            DA = new SqlDataAdapter("select * from viewSubjectofDepartment", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            dgvSubject.DataSource = TB;

            dgvSubject.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvSubject.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvSubject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvSubject.Columns["SubjectID"].Visible = false;
            dgvSubject.Columns["DepartmentID"].Visible = false;

            foreach (DataGridViewColumn col in dgvSubject.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            
        }
        
        private void Subject_Load(object sender, EventArgs e)
        {
            Fillcbx(cbxDepartment, "departmentID", "departmentName", "department");
            isLoadBuilding = true;
         
            Fillcbx(cbxSubject, "subjectID", "subjectTitle", "subject");
            cbxSubject.Text = null;
            cbxSubject.Focus();
            txtSearch.Text = "Search subject here...";
            txtSearch.ForeColor = Color.Gray;
            cbxSubject.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxSubject.AutoCompleteSource = AutoCompleteSource.ListItems;

            DisplaySubject(int.Parse(cbxDepartment.SelectedValue.ToString()));
            loadData();
        }

        




        // Create new subject or cancel operation
        private void btnNew_Click(object sender, EventArgs e)
        {

           

            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                txtSubjectDesc.Text = "";
                cbxSubject.Text = null;
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




        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បោះបង់")
            {
                if (string.IsNullOrEmpty(cbxSubject.Text.Trim()))
                {
                    MessageBox.Show("Please Input room number...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbxSubject.Focus();
                    return;
                }
                int subjectID;
                if (cbxSubject.SelectedValue != null)
                {
                    subjectID = int.Parse(cbxSubject.SelectedValue.ToString());
                }
                else
                {
                    subjectID = 0;
                }
                SqlCommand com = new SqlCommand("insertSubjectDepartment", Operation.con);
                 com.CommandType = CommandType.StoredProcedure;
                 com.Parameters.AddWithValue("@DepartmentID", int.Parse(cbxDepartment.SelectedValue.ToString()));
                 com.Parameters.AddWithValue("@SubjectID", subjectID);
                 com.Parameters.AddWithValue("@SubjectTitle", cbxSubject.Text.ToString());
                 com.Parameters.AddWithValue("@SubjectDescription", txtSubjectDesc.Text.ToString());
                 int rowEffect = com.ExecuteNonQuery();
                 DisplaySubject(int.Parse(cbxDepartment.SelectedValue.ToString()));
                 loadData();
                 Fillcbx(cbxSubject, "subjectID", "subjectTitle", "subject");
                 txtSubjectDesc.Text = "";
                 cbxSubject.Text = null;
                 cbxSubject.Focus();


            }
        }
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (cbxSubject.SelectedValue == null)
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbxDepartment.SelectedValue == null)
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("deleteSubjectDepartment", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@DepartmentID", int.Parse(cbxDepartment.SelectedValue.ToString()));
                com.Parameters.AddWithValue("@SubjectID", int.Parse(cbxSubject.SelectedValue.ToString()));
                int rowEffect = com.ExecuteNonQuery();
                DisplaySubject(int.Parse(cbxDepartment.SelectedValue.ToString()));
                loadData();
                Fillcbx(cbxSubject, "subjectID", "subjectTitle", "subject");
                txtSubjectDesc.Text = "";
                cbxSubject.Text = null;
                cbxSubject.Focus();
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

          

            DA = new SqlDataAdapter();
            TB = new DataTable();
            DA.SelectCommand = new SqlCommand("SearchDepartmentBySubject", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.Add("@Keyword", SqlDbType.NVarChar, 100).Value = txtSearch.Text.Trim();
            DA.Fill(TB);
            dgvSubject.DataSource = TB;

        }

        private void dgvRoom_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvSubject.Rows[e.RowIndex];
                    txtSubjectDesc.Text = row.Cells["Description"].Value.ToString();
                    cbxSubject.SelectedValue = int.Parse(row.Cells["SubjectID"].Value.ToString());
                    cbxDepartment.SelectedValue = int.Parse(row.Cells["DepartmentID"].Value.ToString());
                }
            }
        }

  
   

        private void cbxDepartment_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!isLoadBuilding) return;

            DisplaySubject(int.Parse(cbxDepartment.SelectedValue.ToString()));
        }

      
    }
}
