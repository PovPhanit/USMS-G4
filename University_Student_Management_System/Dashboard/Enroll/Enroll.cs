using Project3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using University_Student_Management_System.Dashboard.Class;
using University_Student_Management_System.Dashboard.Department;
using University_Student_Management_System.Dashboard.Level;
using University_Student_Management_System.Dashboard.Staff;

namespace University_Student_Management_System.Dashboard.Enroll
{
    public partial class Enroll : Form
    {
        public Enroll()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        byte[] Photo;
        string filepath;
        bool isCreateUPdate = false;
        bool isLoadClass = false;
        bool preventDisplay = false;
        private void btnImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "JPEG FILE|*.jpg; *.jpeg |PNG FILE|*.png";
            fd.Title = "Open an image...";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                filepath = fd.FileName;
                imageStudent.Image = Image.FromFile(filepath);
            }
        }
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
            DA = new SqlDataAdapter("select * from viewClassEnroll order by ClassroomID desc", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            dgvEnroll.DataSource = TB;
            dgvEnroll.ColumnHeadersHeight = 40;
            dgvEnroll.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvEnroll.RowTemplate.Height = 100;
            foreach (DataGridViewRow row in dgvEnroll.Rows)
            {
                row.Height = 100;
            }
            dgvEnroll.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvEnroll.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvEnroll.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEnroll.Columns["ClassroomID"].Visible = false;
            dgvEnroll.Columns["ClassID"].Visible = false;
            dgvEnroll.Columns["EnrollID"].Visible = false;
            dgvEnroll.Columns["StaffID"].Visible = false;
            dgvEnroll.Columns["Staff NameKH"].Visible = false;
            dgvEnroll.Columns["Staff Gender"].Visible = false;
            dgvEnroll.Columns["Department"].Width = 100;
            dgvEnroll.Columns["Class"].Width = 50;
            dgvEnroll.Columns["Level"].Width = 60;
            dgvEnroll.Columns["Room"].Width = 50;
            dgvEnroll.Columns["Name KH"].Width = 100;
            dgvEnroll.Columns["Name EN"].Width = 100;
            DataGridViewImageColumn img = new DataGridViewImageColumn();
            img = (DataGridViewImageColumn)dgvEnroll.Columns["Image"];
            img.ImageLayout = DataGridViewImageCellLayout.Stretch;

            foreach (DataGridViewColumn col in dgvEnroll.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }
        private void fieldGender()
        {
            cbxGender.Items.Add("Male");
            cbxGender.Items.Add("Female");
            cbxGender.SelectedIndex = 0;
        }
        private void fieldType()
        {
            cbxType.Items.Add("Student-paid");
            cbxType.Items.Add("Student-scholarship");
            cbxType.SelectedIndex = 0;
        }
        private void fieldStatus()
        {
            cbxStatus.Items.Add("Pending");
            cbxStatus.Items.Add("Partially Paid");
            cbxStatus.Items.Add("Confirm");
            cbxStatus.SelectedIndex = 0;
        }

        private void DisplayClass(int levelID, int deID, int generationID)
        {

            SqlDataAdapter DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("viewAllRoomCreateByGeneration", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@LevelID", levelID);
            DA.SelectCommand.Parameters.AddWithValue("@DepartmentID", deID);
            DA.SelectCommand.Parameters.AddWithValue("@GenerationID", generationID);


            DataTable TB = new DataTable();
            Image image = Image.FromFile("../../Resources/roomDisplay.png");
            DA.Fill(TB);


            panelClassContainer2.Controls.Clear();


            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.WrapContents = true;
            flow.AutoScroll = true;
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.Padding = new Padding(3);

            foreach (DataRow dr in TB.Rows)
            {
                Panel itemPanel = new Panel();
                itemPanel.Size = new Size(100, 120);
                itemPanel.Margin = new Padding(5);


                string textshow = "";
                Color colors;
                Cursor cs;
                if (int.Parse(dr["ClassCountEnroll"].ToString()) >= int.Parse(dr["RoomCapacity"].ToString()))
                {
                    textshow = "Class Full";
                    colors = Color.Red;
                    cs = Cursors.No;
                }
                else
                {
                    textshow = dr["ClassCountEnroll"].ToString() + " / " + dr["RoomCapacity"].ToString();
                    colors = Color.Black;
                    cs = Cursors.Hand;
                }
                Color colorClose;
                if (dr["classAvailable"].ToString().StartsWith("available"))
                {
                    colorClose = Color.Green;
                }
                else
                {
                    colorClose = Color.Red;
                    textshow = "Class Close";
                    colors = Color.Red;
                    cs = Cursors.No;
                }


                PictureBox pic = new PictureBox();
                pic.Image = image;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(50, 50);
                pic.Cursor = cs;
                pic.Location = new Point(25, 0);





                if (int.Parse(dr["ClassCountEnroll"].ToString()) < int.Parse(dr["RoomCapacity"].ToString()) && dr["classAvailable"].ToString().StartsWith("available"))
                {
                    pic.Click += (s, e1) =>
                    {
                        txtClassName.Text = dr["ClassName"].ToString();
                        txtClassName.Tag = dr["ClassID"].ToString();
                    };
                }

                Label lblClassName = new Label();
                lblClassName.Text = dr["ClassName"].ToString();

                lblClassName.ForeColor = colorClose;
                lblClassName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblClassName.TextAlign = ContentAlignment.MiddleCenter;
                lblClassName.Size = new Size(100, 20);
                lblClassName.Cursor = cs;
                lblClassName.Location = new Point(0, 48);

                if (int.Parse(dr["ClassCountEnroll"].ToString()) < int.Parse(dr["RoomCapacity"].ToString()) && dr["classAvailable"].ToString().StartsWith("available"))
                {
                    lblClassName.Click += (s, e2) =>
                    {
                        txtClassName.Text = dr["ClassName"].ToString();
                        txtClassName.Tag = dr["ClassID"].ToString();
                    };
                }

                Label lblRoomNumber = new Label();
                lblRoomNumber.Text = dr["BuildingName"].ToString().Split(' ')[1] + " - " + dr["RoomNumber"].ToString();
                lblRoomNumber.ForeColor = Color.Black;
                lblRoomNumber.Cursor = cs;
                lblRoomNumber.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblRoomNumber.TextAlign = ContentAlignment.MiddleCenter;
                lblRoomNumber.Size = new Size(100, 20);
                lblRoomNumber.Location = new Point(0, 65);

                if (int.Parse(dr["ClassCountEnroll"].ToString()) < int.Parse(dr["RoomCapacity"].ToString()) && dr["classAvailable"].ToString().StartsWith("available"))
                {
                    lblRoomNumber.Click += (s, e3) =>
                    {
                        txtClassName.Text = dr["ClassName"].ToString();
                        txtClassName.Tag = dr["ClassID"].ToString();
                    };
                }
                Label lbltotal = new Label();
                lbltotal.Text = textshow;
                lbltotal.ForeColor = colors;
                lbltotal.Cursor = cs;
                lbltotal.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lbltotal.TextAlign = ContentAlignment.MiddleCenter;
                lbltotal.Size = new Size(100, 20);
                lbltotal.Location = new Point(0, 80);

                if (int.Parse(dr["ClassCountEnroll"].ToString()) < int.Parse(dr["RoomCapacity"].ToString()) && dr["classAvailable"].ToString().StartsWith("available"))
                {
                    lbltotal.Click += (s, e3) =>
                    {
                        txtClassName.Text = dr["ClassName"].ToString();
                        txtClassName.Tag = dr["ClassID"].ToString();
                    };
                }


                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblClassName);
                itemPanel.Controls.Add(lblRoomNumber);
                itemPanel.Controls.Add(lbltotal);
                flow.Controls.Add(itemPanel);
            }

            panelClassContainer2.Controls.Add(flow);
        }
       
        private void Enroll_Load(object sender, EventArgs e)
        {
            fieldGender();
            fieldStatus();
            fieldType();
            Fillcbx(cbxLevel, "LevelID", "LevelName", "Level");
            Fillcbx(cbxDepartment, "DepartmentID", "DepartmentName", "Department");
            Fillcbx(cbxGeneration, "GenerationID", "GenerationName", "Generation");
            txtNameKH.Focus();
            txtSearch.Text = "Search enroll hear...";
            txtSearch.ForeColor = Color.Gray;
 
            isLoadClass = true;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
            loadData();

        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search enroll hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search enroll hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("SearchStudentEnrollInclass", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@Keyword", txtSearch.Text.Trim());
            TB = new DataTable();
            DA.Fill(TB);
            dgvEnroll.DataSource = TB;
        }

        private void dgvEnroll_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate)
            {
                if (e.RowIndex >= 0) // Make sure it's not a header
                {
                    preventDisplay = true;
                    DataGridViewRow row = dgvEnroll.Rows[e.RowIndex];
                    txtNameKH.Tag = row.Cells["ClassroomID"].Value.ToString();
                    txtNameEN.Tag = row.Cells["EnrollID"].Value.ToString();
                    txtNameKH.Text = row.Cells["Name KH"].Value.ToString();
                    txtNameEN.Text = row.Cells["Name EN"].Value.ToString();
                    if (row.Cells["Gender"].Value.ToString().Trim() == "Male")
                    {
                        cbxGender.SelectedIndex = 0;

                    }
                    else
                    {
                        cbxGender.SelectedIndex = 1;
                    }
                    if (row.Cells["Type"].Value.ToString().Trim() == "Student-paid")
                    {
                        cbxType.SelectedIndex = 0;

                    }
                    else
                    {
                        cbxType.SelectedIndex = 1;
                    }
                    if (row.Cells["Status"].Value.ToString().Trim() == "Pending")
                    {
                        cbxStatus.SelectedIndex = 0;

                    }
                    else if (row.Cells["Status"].Value.ToString().Trim() == "Partially Paid")
                    {
                        cbxStatus.SelectedIndex = 1;

                    }
                    else
                    {
                        cbxStatus.SelectedIndex = 2;
                    }
                    txtPhnoe.Text = row.Cells["Phone"].Value.ToString();
                    dpDOB.Text = row.Cells["DOB"].Value.ToString();
                    txtVillage.Text = row.Cells["Village"].Value.ToString();
                    txtSongkat.Text = row.Cells["Sangkat_Khum"].Value.ToString();
                    txtKhan.Text = row.Cells["Khan_Srok"].Value.ToString();
                    txtCity.Text = row.Cells["Province_City"].Value.ToString();
                    txtClassName.Text = row.Cells["Class"].Value.ToString();
                    txtClassName.Tag = row.Cells["ClassID"].Value.ToString();
                    Photo = (byte[])row.Cells["Image"].Value;
                    MemoryStream ms = new MemoryStream(Photo);
                    imageStudent.Image = Image.FromStream(ms);

                    SqlCommand com = new SqlCommand("selectLevelGenDepartment", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ClassID", int.Parse(row.Cells["ClassID"].Value.ToString()));
                    SqlDataReader dr = com.ExecuteReader();
                    while (dr.Read()) {
                        cbxLevel.SelectedValue = int.Parse(dr["LevelID"].ToString());
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                    }
                    dr.Close();

                    int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
                    int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
                    int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
                    DisplayClass(levelID, departmentID, generationID);
                    preventDisplay = false;
                }
            }
        }
        private void hideStudentInput(bool b)
        {
            Color bgColor = b ? Color.White : Color.Silver;

            txtNameKH.Enabled = b;
            txtNameKH.BackColor = bgColor;

            txtNameEN.Enabled = b;
            txtNameEN.BackColor = bgColor;

            cbxGender.Enabled = b;
            cbxGender.BackColor = bgColor;

            txtPhnoe.Enabled = b;
            txtPhnoe.BackColor = bgColor;

            dpDOB.Enabled = b;
            dpDOB.CalendarMonthBackground = bgColor; // For DateTimePicker

            txtVillage.Enabled = b;
            txtVillage.BackColor = bgColor;

            txtSongkat.Enabled = b;
            txtSongkat.BackColor = bgColor;

            txtKhan.Enabled = b;
            txtKhan.BackColor = bgColor;

            txtCity.Enabled = b;
            txtCity.BackColor = bgColor;

            imageStudent.Enabled = b;
           

            btnImage.Enabled = b;
          
        }

        private void hideStudentIDCard(Boolean b)
        {
            Color bgColor = b ? Color.White : Color.Silver;
            txtIDCard.Enabled = b;
            txtIDCard.BackColor = bgColor;
        }
        private void radioNewStudent_CheckedChanged(object sender, EventArgs e)
        {
            if (radioNewStudent.Checked)
            {
                hideStudentInput(true);
                hideStudentIDCard(false);
            }
        }

        private void radioOldStudent_CheckedChanged(object sender, EventArgs e)
        {
            if (radioOldStudent.Checked)
            {
                hideStudentInput(false);
                hideStudentIDCard(true);
            }
        }

        private void radioUpClass_CheckedChanged(object sender, EventArgs e)
        {
            if (radioUpClass.Checked)
            {
                hideStudentInput(false);
                hideStudentIDCard(true);
            }
        }

        private void cbxGeneration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            if (preventDisplay) return;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());

            if (cbxGeneration.SelectedValue == null)
            {
                DisplayClass(levelID, departmentID, 0);
            }
            else
            {
                DisplayClass(levelID, departmentID, generationID);
            }
        }

        private void cbxLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            if (preventDisplay) return;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
        }

        private void cbxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            if (preventDisplay) return;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                txtNameEN.Text = "";
                txtNameKH.Text = "";
                txtPhnoe.Text = "";
                txtVillage.Text = "";
                txtSongkat.Text = "";
                txtKhan.Text = "";
                txtCity.Text = "";
                txtClassName.Text = "";
                txtNameKH.Focus();
                txtSearch.Text = "Search examtype hear...";
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
              

                if (isCreateUPdate)
                {
                    if (radioNewStudent.Checked)
                    {
                        if (string.IsNullOrEmpty(txtNameKH.Text.Trim()))
                        {
                            MessageBox.Show("Please Input Name KH...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtNameKH.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtNameEN.Text.Trim()))
                        {
                            MessageBox.Show("Please Input Name EN...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtNameEN.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtPhnoe.Text.Trim()))
                        {
                            MessageBox.Show("Please Input Phone number...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtPhnoe.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtVillage.Text.Trim()))
                        {
                            MessageBox.Show("Please Input Village...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtVillage.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtSongkat.Text.Trim()))
                        {
                            MessageBox.Show("Please Input Sangkat...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtSongkat.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtKhan.Text.Trim()))
                        {
                            MessageBox.Show("Please Input Khan...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtKhan.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtCity.Text.Trim()))
                        {
                            MessageBox.Show("Please Input City...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtCity.Focus();
                            return;
                        }
                        else if (string.IsNullOrEmpty(txtClassName.Text.Trim()))
                        {
                            MessageBox.Show("Please Choose class...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtCity.Focus();
                            return;
                        }
                        SqlCommand com = new SqlCommand("InsertEnrollWithStudent", Operation.con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@StudentNameKH", txtNameKH.Text.ToString());
                        com.Parameters.AddWithValue("@StudentNameEN", txtNameEN.Text.ToString());
                        String Gender;
                        if (cbxGender.SelectedIndex == 0)
                        {
                            Gender = "Male";
                        }
                        else
                        {
                            Gender = "Female";
                        }
                        com.Parameters.AddWithValue("@StudentGender", Gender);
                        com.Parameters.AddWithValue("@StudentPhoneNumber", txtPhnoe.Text.ToString());
                        com.Parameters.AddWithValue("@StudentDOB", dpDOB.Value);
                        com.Parameters.AddWithValue("@StudentVillage", txtVillage.Text.ToString());
                        com.Parameters.AddWithValue("@StudentSangkatKhum", txtSongkat.Text.ToString());
                        com.Parameters.AddWithValue("@StudentKhanSrok", txtKhan.Text.ToString());
                        com.Parameters.AddWithValue("@StudentProvinceCity", txtCity.Text.ToString());
                        com.Parameters.AddWithValue("@StudentStoppedWork", 0);
                        if (filepath != null)
                        {
                            Photo = File.ReadAllBytes(filepath);   //using system io    
                        }
                        else
                        {
                            Photo = File.ReadAllBytes("../../Resources/bookDisplay.png");
                        }
                        com.Parameters.AddWithValue("@StudentImage", Photo);
                        com.Parameters.AddWithValue("@departmentID", int.Parse(cbxDepartment.SelectedValue.ToString()));
                        com.Parameters.AddWithValue("@StaffID", storeAuthorization.id);
                        com.Parameters.AddWithValue("@LevelID", int.Parse(cbxLevel.SelectedValue.ToString()));
                        com.Parameters.AddWithValue("@EnrollStatus", cbxStatus.Text.ToString());
                        com.Parameters.AddWithValue("@EnrollType", cbxType.Text.ToString());
                        com.Parameters.AddWithValue("@idCard", DBNull.Value);
                        com.Parameters.AddWithValue("@ClassID", int.Parse(txtClassName.Tag.ToString()));
                        int rowEffect = com.ExecuteNonQuery();
                    }
                    else if (radioOldStudent.Checked) {
                        Operation.con.InfoMessage -= OnSqlInfoMessage;
                        Operation.con.InfoMessage += OnSqlInfoMessage;
                        // Set this just once too
                        Operation.con.FireInfoMessageEventOnUserErrors = true;
                        // Define the handler once
                        void OnSqlInfoMessage(object sender1, SqlInfoMessageEventArgs e1)
                        {
                            MessageBox.Show(e1.Message, "SQL Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        if (string.IsNullOrEmpty(txtIDCard.Text.Trim()))
                        {
                            MessageBox.Show("Please Input ID Card...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtNameKH.Focus();
                            return;
                        }

                        SqlCommand com = new SqlCommand("InsertEnrollWithStudent", Operation.con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@StudentNameKH", txtNameKH.Text.ToString());
                        com.Parameters.AddWithValue("@StudentNameEN", txtNameEN.Text.ToString());
                        String Gender;
                        if (cbxGender.SelectedIndex == 0)
                        {
                            Gender = "Male";
                        }
                        else
                        {
                            Gender = "Female";
                        }
                        com.Parameters.AddWithValue("@StudentGender", Gender);
                        com.Parameters.AddWithValue("@StudentPhoneNumber", txtPhnoe.Text.ToString());
                        com.Parameters.AddWithValue("@StudentDOB", dpDOB.Value);
                        com.Parameters.AddWithValue("@StudentVillage", txtVillage.Text.ToString());
                        com.Parameters.AddWithValue("@StudentSangkatKhum", txtSongkat.Text.ToString());
                        com.Parameters.AddWithValue("@StudentKhanSrok", txtKhan.Text.ToString());
                        com.Parameters.AddWithValue("@StudentProvinceCity", txtCity.Text.ToString());
                        com.Parameters.AddWithValue("@StudentStoppedWork", 0);
                        if (filepath != null)
                        {
                            Photo = File.ReadAllBytes(filepath);   //using system io    
                        }
                        else
                        {
                            Photo = File.ReadAllBytes("../../Resources/bookDisplay.png");
                        }
                        com.Parameters.AddWithValue("@StudentImage", Photo);
                        com.Parameters.AddWithValue("@departmentID", int.Parse(cbxDepartment.SelectedValue.ToString()));
                        com.Parameters.AddWithValue("@StaffID", storeAuthorization.id);
                        com.Parameters.AddWithValue("@LevelID", int.Parse(cbxLevel.SelectedValue.ToString()));
                        com.Parameters.AddWithValue("@EnrollStatus", cbxStatus.Text.ToString());
                        com.Parameters.AddWithValue("@EnrollType", cbxType.Text.ToString());
                        com.Parameters.AddWithValue("@idCard", txtIDCard.Text.ToString());
                        com.Parameters.AddWithValue("@ClassID", int.Parse(txtClassName.Tag.ToString()));
                        int rowEffect = com.ExecuteNonQuery();
                    }
                    else
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


                        if (string.IsNullOrEmpty(txtIDCard.Text.Trim()))
                        {
                            MessageBox.Show("Please Input ID Card...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtNameKH.Focus();
                            return;
                        }
                        SqlCommand com = new SqlCommand("InsertEnrollWithOldStudent", Operation.con);
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@departmentID", int.Parse(cbxDepartment.SelectedValue.ToString()));
                        com.Parameters.AddWithValue("@StaffID", storeAuthorization.id);
                        com.Parameters.AddWithValue("@LevelID", int.Parse(cbxLevel.SelectedValue.ToString()));
                        com.Parameters.AddWithValue("@EnrollStatus", cbxStatus.Text.ToString());
                        com.Parameters.AddWithValue("@EnrollType", cbxType.Text.ToString());
                        com.Parameters.AddWithValue("@idCard", txtIDCard.Text.ToString());
                        com.Parameters.AddWithValue("@ClassID", int.Parse(txtClassName.Tag.ToString()));
                        com.Parameters.AddWithValue("@GenerationID", int.Parse(cbxGeneration.SelectedValue.ToString()));
                        com.Parameters.AddWithValue("@permission","active");
                        int rowEffect = com.ExecuteNonQuery();
                    }
                   
                    filepath = null;
                    txtNameEN.Text = "";
                    txtNameKH.Text = "";
                    txtPhnoe.Text = "";
                    txtVillage.Text = "";
                    txtSongkat.Text = "";
                    txtKhan.Text = "";
                    txtCity.Text = "";
                    txtClassName.Text = "";
                    //ControlForm.ClearData(this);
                    txtNameKH.Focus();
                    loadData();
                    int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
                    int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
                    int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
                    DisplayClass(levelID, departmentID, generationID);
                }
                else
                {
                    SqlCommand com = new SqlCommand("UpdateEnrollWithStudent", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ClassroomID",int.Parse(txtNameKH.Tag.ToString()));
                    com.Parameters.AddWithValue("@EnrollID", int.Parse(txtNameEN.Tag.ToString()));
                    com.Parameters.AddWithValue("@StudentNameKH", txtNameKH.Text.ToString());
                    com.Parameters.AddWithValue("@StudentNameEN", txtNameEN.Text.ToString());
                    String Gender;
                    if (cbxGender.SelectedIndex == 0)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }
                    com.Parameters.AddWithValue("@StudentGender", Gender);
                    com.Parameters.AddWithValue("@StudentPhoneNumber", txtPhnoe.Text.ToString());
                    com.Parameters.AddWithValue("@StudentDOB", dpDOB.Value);
                    com.Parameters.AddWithValue("@StudentVillage", txtVillage.Text.ToString());
                    com.Parameters.AddWithValue("@StudentSangkatKhum", txtSongkat.Text.ToString());
                    com.Parameters.AddWithValue("@StudentKhanSrok", txtKhan.Text.ToString());
                    com.Parameters.AddWithValue("@StudentProvinceCity", txtCity.Text.ToString());
                    com.Parameters.AddWithValue("@StudentStoppedWork", 0);
                    if (filepath != null)
                    {
                        Photo = File.ReadAllBytes(filepath);   //using system io    
                    }
                    else
                    {
                        Photo = File.ReadAllBytes("../../Resources/bookDisplay.png");
                    }
                    com.Parameters.AddWithValue("@StudentImage", Photo);
                    com.Parameters.AddWithValue("@departmentID", int.Parse(cbxDepartment.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@StaffID", storeAuthorization.id);
                    com.Parameters.AddWithValue("@LevelID", int.Parse(cbxLevel.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@EnrollStatus", cbxStatus.Text.ToString());
                    com.Parameters.AddWithValue("@EnrollType", cbxType.Text.ToString());
                    com.Parameters.AddWithValue("@idCard", DBNull.Value);
                    com.Parameters.AddWithValue("@ClassID", int.Parse(txtClassName.Tag.ToString()));
                    filepath = null;
                    txtNameKH.Focus();
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();
                    int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
                    int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
                    int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
                    DisplayClass(levelID, departmentID, generationID);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtNameKH.Tag == null)
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("deleteEnrollWithStudent", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ClassroomID", int.Parse(txtNameKH.Tag.ToString()));
                com.Parameters.AddWithValue("@EnrollID", int.Parse(txtNameEN.Tag.ToString()));
                int rowEffect = com.ExecuteNonQuery();
                txtNameEN.Text = "";
                txtNameKH.Tag = null;
                txtNameKH.Text = "";
                txtPhnoe.Text = "";
                txtVillage.Text = "";
                txtSongkat.Text = "";
                txtKhan.Text = "";
                txtCity.Text = "";
                txtClassName.Text = "";
                txtNameKH.Focus();
                loadData();
                int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
                int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
                int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
                DisplayClass(levelID, departmentID, generationID);
            }
        }



        private void txtIDCard_KeyUp(object sender, KeyEventArgs e)
        {
            SqlCommand com = new SqlCommand("SearchStudentbyIDCard", Operation.con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Keyword", txtIDCard.Text.ToString().Trim());
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                txtNameKH.Text = dr["StudentNameKH"].ToString();
                txtNameEN.Text = dr["StudentNameEN"].ToString();
                if (dr["StudentGender"].ToString().Trim() == "Male")
                {
                    cbxGender.SelectedIndex = 0;

                }
                else
                {
                    cbxGender.SelectedIndex = 1;
                }
                txtPhnoe.Text = dr["StudentPhoneNumber"].ToString();
                dpDOB.Text = dr["StudentDOB"].ToString();
                txtVillage.Text = dr["StudentVillage"].ToString();
                txtSongkat.Text = dr["StudentSangkatKhum"].ToString();
                txtKhan.Text = dr["StudentKhanSrok"].ToString();
                txtCity.Text = dr["StudentProvinceCity"].ToString();
            }
            dr.Close();
        }

        private void txtNameKH_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e,cbxGeneration, txtNameEN);
        }

        private void txtNameEN_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtNameKH, cbxGender);
        }

        private void cbxGender_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e,txtNameEN, txtPhnoe);
        }

        private void txtPhnoe_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, cbxGender, dpDOB);
        }

        private void dpDOB_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtPhnoe, txtVillage);
        }

        private void txtVillage_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, dpDOB, txtSongkat);
        }

        private void txtSongkat_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtVillage, txtKhan);
        }

        private void txtKhan_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtSongkat, txtCity);
        }

        private void txtCity_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e,txtKhan, cbxStatus);
        }

        private void cbxStatus_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtCity, cbxType);
        }

        private void cbxType_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, cbxStatus, cbxDepartment);
        }

        private void cbxDepartment_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, cbxType, cbxLevel);
        }

        private void cbxLevel_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, cbxDepartment, cbxGeneration);
        }

        private void cbxGeneration_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, cbxLevel, txtNameKH);
        }
    }
}
