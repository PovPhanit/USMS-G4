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
                lblSubjectTitle.Size = new Size(70, 20);
                lblSubjectTitle.Location = new Point(30, 72);

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
            cbxSubject.Text = null;
            Fillcbx(cbxSubject, "subjectID", "subjectTitle", "subject");
            cbxSubject.Focus();
            txtSearch.Text = "Search subject here...";
            txtSearch.ForeColor = Color.Gray;

            DisplaySubject(int.Parse(cbxDepartment.SelectedValue.ToString()));
            loadData();
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
            
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
          

           
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
    }
}
