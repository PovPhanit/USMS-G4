using Project3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using University_Student_Management_System.Dashboard.Class;
using University_Student_Management_System.Dashboard.ExamType;
using University_Student_Management_System.Dashboard.Professor;
using University_Student_Management_System.Dashboard.Semester;
using University_Student_Management_System.Dashboard.Subject;

namespace University_Student_Management_System.Dashboard.Exam
{
    public partial class Exam : Form
    {
        public Exam()
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

        bool isLoadSubject = false;
        public void Fillcbx(ComboBox cbx, string fd1, string fd2, string TB2)
        {
            DA = new SqlDataAdapter("select " + fd1 + "," + fd2 + " From " + TB2, Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            cbx.DataSource = TB;
            cbx.DisplayMember = fd2;
            cbx.ValueMember = fd1;
        }
        public void FillSubject(int level,int department,int generation,int semester)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("showSubjectCreateSchedule", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@LevelID", level);
            DA.SelectCommand.Parameters.AddWithValue("@DepartmentID", department);
            DA.SelectCommand.Parameters.AddWithValue("@GenerationID", generation);
            DA.SelectCommand.Parameters.AddWithValue("@SemesterID", semester);
            TB = new DataTable();
            DA.Fill(TB);
            cbxSubject.DataSource = TB;
            cbxSubject.DisplayMember = "SubjectTitle";
            cbxSubject.ValueMember = "SubjectID";
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


            panelClassContainer1.Controls.Clear();


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
                    cs = Cursors.Hand;
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
                    cs = Cursors.Hand;
                }


                PictureBox pic = new PictureBox();
                pic.Image = image;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(50, 50);
                pic.Cursor = cs;
                pic.Location = new Point(25, 0);






                pic.Click += (s, e1) =>
                {
                    labelClassName.Tag = dr["timesSlot"].ToString().ToLower().Trim();
                    txtClassName.Tag = dr["ClassID"].ToString();

                    cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                    cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                    cbxLevel.SelectedValue = int.Parse(dr["LevelID"].ToString());

                    txtClassName.Text = dr["ClassName"].ToString();
                };


                Label lblClassName = new Label();
                lblClassName.Text = dr["ClassName"].ToString();

                lblClassName.ForeColor = colorClose;
                lblClassName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblClassName.TextAlign = ContentAlignment.MiddleCenter;
                lblClassName.Size = new Size(100, 20);
                lblClassName.Cursor = cs;
                lblClassName.Location = new Point(0, 48);


                lblClassName.Click += (s, e2) =>
                {
                    labelClassName.Tag = dr["timesSlot"].ToString().ToLower().Trim();
                    txtClassName.Tag = dr["ClassID"].ToString();

                    cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                    cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                    cbxLevel.SelectedValue = int.Parse(dr["LevelID"].ToString());

                    txtClassName.Text = dr["ClassName"].ToString();
                };


                Label lblRoomNumber = new Label();
                lblRoomNumber.Text = dr["BuildingName"].ToString().Split(' ')[1] + " - " + dr["RoomNumber"].ToString();
                lblRoomNumber.ForeColor = Color.Black;
                lblRoomNumber.Cursor = cs;
                lblRoomNumber.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblRoomNumber.TextAlign = ContentAlignment.MiddleCenter;
                lblRoomNumber.Size = new Size(100, 20);
                lblRoomNumber.Location = new Point(0, 65);


                lblRoomNumber.Click += (s, e3) =>
                {
                    labelClassName.Tag = dr["timesSlot"].ToString().ToLower().Trim();
                    txtClassName.Tag = dr["ClassID"].ToString();

                    cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                    cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                    cbxLevel.SelectedValue = int.Parse(dr["LevelID"].ToString());

                    txtClassName.Text = dr["ClassName"].ToString();
                };

                Label lbltotal = new Label();
                lbltotal.Text = textshow;
                lbltotal.ForeColor = colors;
                lbltotal.Cursor = cs;
                lbltotal.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lbltotal.TextAlign = ContentAlignment.MiddleCenter;
                lbltotal.Size = new Size(100, 20);
                lbltotal.Location = new Point(0, 80);


                lbltotal.Click += (s, e3) =>
                {
                    labelClassName.Tag = dr["timesSlot"].ToString().ToLower().Trim();
                    txtClassName.Tag = dr["ClassID"].ToString();

                    cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                    cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                    cbxLevel.SelectedValue = int.Parse(dr["LevelID"].ToString());

                    txtClassName.Text = dr["ClassName"].ToString();
                };



                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblClassName);
                itemPanel.Controls.Add(lblRoomNumber);
                itemPanel.Controls.Add(lbltotal);
                flow.Controls.Add(itemPanel);
            }

            panelClassContainer1.Controls.Add(flow);
        }

        private void loadData()
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("viewAllStudentInClass", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            int classID;
            if(txtClassName.Tag == null)
            {
                classID = 0;
            }
            else
            {
                classID= int.Parse(txtClassName.Tag.ToString());
            }
                DA.SelectCommand.Parameters.AddWithValue("@ClassID", classID);
            TB = new DataTable();
            DA.Fill(TB);
            dgvExam.DataSource = TB;
            dgvExam.ColumnHeadersHeight = 40;
            dgvExam.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;


            dgvExam.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvExam.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvExam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExam.Columns["ClassroomID"].Visible = false;
            dgvExam.Columns["EnrollID"].Visible = false;
            dgvExam.Columns["ClassID"].Visible = false;
            dgvExam.Columns["StaffID"].Visible = false;
            dgvExam.Columns["Date"].Visible = false;
            dgvExam.Columns["Staff NameEN"].Visible = false;
            dgvExam.Columns["Staff NameKH"].Visible = false;
            dgvExam.Columns["Staff Gender"].Visible = false;
            dgvExam.Columns["Date"].Visible = false;
            dgvExam.Columns["DOB"].Visible = false;
            dgvExam.Columns["Village"].Visible = false;
            dgvExam.Columns["Sangkat_Khum"].Visible = false;
            dgvExam.Columns["Khan_Srok"].Visible = false;
            dgvExam.Columns["Province_City"].Visible = false;
            dgvExam.Columns["Phone"].Visible = false;
            dgvExam.Columns["Image"].Visible = false;
            dgvExam.Columns["Department"].Visible = false;
            dgvExam.Columns["Class"].Visible = false;
            dgvExam.Columns["Level"].Visible = false;
            dgvExam.Columns["Room"].Visible = false;
            dgvExam.Columns["Name KH"].Width = 100;
            dgvExam.Columns["Name EN"].Width = 100;
               
            foreach (DataGridViewColumn col in dgvExam.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }
        private void loadData_score()
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("viewAllStudentInClassScoreExam", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            int classID;
            if (txtClassName.Tag == null)
            {
                classID = 0;
            }
            else
            {
                classID = int.Parse(txtClassName.Tag.ToString());
            }
            DA.SelectCommand.Parameters.AddWithValue("@ClassID", classID);
            DA.SelectCommand.Parameters.AddWithValue("@SemesterID", int.Parse(cbxSemester.SelectedValue.ToString()));
            DA.SelectCommand.Parameters.AddWithValue("@ExamTypeID", int.Parse(cbxExamType.SelectedValue.ToString()));

            TB = new DataTable();
            DA.Fill(TB);
            dgvExamScore.DataSource = TB;
            dgvExamScore.ColumnHeadersHeight = 40;
            dgvExamScore.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;


            dgvExamScore.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvExamScore.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvExamScore.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExamScore.Columns["ExamID"].Visible = false;
            dgvExamScore.Columns["ExamTypeID"].Visible = false;
            dgvExamScore.Columns["ClassroomID"].Visible = false;
            dgvExamScore.Columns["ClassID"].Visible = false;
            dgvExamScore.Columns["SubjectID"].Visible = false;
            dgvExamScore.Columns["ProfessorID"].Visible = false;
            dgvExamScore.Columns["SemesterID"].Visible = false;
            dgvExamScore.Columns["GenerationID"].Visible = false;

            dgvExamScore.Columns["Professor NameEN"].Visible = false;
            dgvExamScore.Columns["Professor NameKH"].Visible = false;
            dgvExamScore.Columns["Professor Gender"].Visible = false;
            dgvExamScore.Columns["Class"].Visible = false;
            dgvExamScore.Columns["Department"].Visible = false;
            dgvExamScore.Columns["Level"].Visible = false;
            dgvExamScore.Columns["Room"].Visible = false;
            dgvExamScore.Columns["Generation"].Visible = false;

            dgvExamScore.Columns["Name KH"].Width = 100;
            dgvExamScore.Columns["Name EN"].Width = 100;

            foreach (DataGridViewColumn col in dgvExamScore.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


        }
        private void Exam_Load(object sender, EventArgs e)
        {
            Fillcbx(cbxExamType, "ExamTypeID", "ExamTypeDetail", "ExamType");
            Fillcbx(cbxSemester, "SemesterID", "SemesterName", "Semester");
            Fillcbx(cbxDepartment, "DepartmentID", "DepartmentName", "Department");
            Fillcbx(cbxProfessor, "ProfessorID", "ProfessorNameEN", "Professor");
            Fillcbx(cbxLevel, "LevelID", "LevelName", "Level");
            Fillcbx(cbxGeneration, "GenerationID", "GenerationName", "Generation");
            FillSubject(int.Parse(cbxLevel.SelectedValue.ToString()), int.Parse(cbxDepartment.SelectedValue.ToString()), int.Parse(cbxGeneration.SelectedValue.ToString()), int.Parse(cbxSemester.SelectedValue.ToString()));
            cbxDepartment.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxDepartment.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbxProfessor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxProfessor.AutoCompleteSource = AutoCompleteSource.ListItems;

            isLoadSubject = true;


            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
            isLoadClass = true;

            txtScore.Focus();
            txtSearch.Text = "Search exam hear...";
            txtSearch.ForeColor = Color.Gray;
            loadData();
            loadData_score();

        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search exam hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search exam hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            int classID;
            if (txtClassName.Tag == null)
            {
                classID = 0;
            }
            else
            {
                classID = int.Parse(txtClassName.Tag.ToString());
            }
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("SearchStudentInClass", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@ClassID", classID);
            DA.SelectCommand.Parameters.AddWithValue("@Keyword", txtSearch.Text.Trim());
            TB = new DataTable();
            DA.Fill(TB);
            dgvExam.DataSource = TB;
        }

        private void cbxGeneration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);

            if (!isLoadSubject) return;
            FillSubject(int.Parse(cbxLevel.SelectedValue.ToString()), int.Parse(cbxDepartment.SelectedValue.ToString()), int.Parse(cbxGeneration.SelectedValue.ToString()), int.Parse(cbxSemester.SelectedValue.ToString()));

        }

        private void cbxLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);

            if (!isLoadSubject) return;

            FillSubject(int.Parse(cbxLevel.SelectedValue.ToString()), int.Parse(cbxDepartment.SelectedValue.ToString()), int.Parse(cbxGeneration.SelectedValue.ToString()), int.Parse(cbxSemester.SelectedValue.ToString()));

        }

        private void cbxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);

            if (!isLoadSubject) return;
          
            FillSubject(int.Parse(cbxLevel.SelectedValue.ToString()), int.Parse(cbxDepartment.SelectedValue.ToString()), int.Parse(cbxGeneration.SelectedValue.ToString()), int.Parse(cbxSemester.SelectedValue.ToString()));

        }

        private void cbxSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadSubject) return;
     
            FillSubject(int.Parse(cbxLevel.SelectedValue.ToString()), int.Parse(cbxDepartment.SelectedValue.ToString()), int.Parse(cbxGeneration.SelectedValue.ToString()), int.Parse(cbxSemester.SelectedValue.ToString()));
            loadData_score();
        }

        private void txtClassName_TextChanged(object sender, EventArgs e)
        {
            loadData();
            loadData_score();
        }

        private void cbxExamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            loadData_score();
        }

        private void txtIDCard_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtScore, txtScore);

            if (txtIDCard.Text.ToString().Trim().Length > 0)
            {
                SqlCommand com = new SqlCommand("displayStudentByIDCard", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@IDCard", txtIDCard.Text.Trim());
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    txtStudentName.Text = dr["StudentNameKH"].ToString();
                 
                }
                dr.Close();
              
            }
            else
            {
                txtStudentName.Text = "";
            }
        }

        private void dgvExam_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
  
                if (e.RowIndex >= 0) // Make sure it's not a header
                {
                    DataGridViewRow row = dgvExam.Rows[e.RowIndex];
                    
                    txtIDCard.Text = row.Cells["ID Card"].Value.ToString();
                    txtStudentName.Text = row.Cells["Name KH"].Value.ToString();

               }
            
        }

        private void dgvExamScore_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate)
            {
                if (e.RowIndex >= 0) // Make sure it's not a header
                {
                    DataGridViewRow row = dgvExamScore.Rows[e.RowIndex];

                    txtIDCard.Text = row.Cells["ID Card"].Value.ToString();
                    txtStudentName.Text = row.Cells["Name KH"].Value.ToString();
                    txtStudentName.Tag = row.Cells["ExamID"].Value.ToString();
                    txtScore.Text = row.Cells["Score"].Value.ToString();
                    cbxSubject.SelectedValue = int.Parse(row.Cells["SubjectID"].Value.ToString());
                    cbxProfessor.SelectedValue = int.Parse(row.Cells["ProfessorID"].Value.ToString());

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
                txtScore.Text = "";
                txtIDCard.Text = "";
                txtStudentName.Text = "";
                txtScore.Focus();

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
            txtScore.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បោះបង់")
            {
                if (txtClassName.Tag == null)
                {
                    MessageBox.Show("Please choose class...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              
                    return;
                }
                else if (string.IsNullOrEmpty(txtScore.Text.Trim()))
                {
                    MessageBox.Show("Please Input score...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtScore.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtIDCard.Text.Trim()))
                {
                    MessageBox.Show("Please Input id card...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtIDCard .Focus();
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

                    SqlCommand com = new SqlCommand("insertExam", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ExamScore", txtScore.Text.ToString());
                    com.Parameters.AddWithValue("@ExamTypeID",int.Parse(cbxExamType.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@SubjectID", int.Parse(cbxSubject.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@ProfessorID", int.Parse(cbxProfessor.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@SemesterID", int.Parse(cbxSemester.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@StudentIDCard", txtIDCard.Text.ToString());
                    com.Parameters.AddWithValue("@ClassID", int.Parse(txtClassName.Tag.ToString()));
                    com.Parameters.AddWithValue("@GenerationID", int.Parse(cbxGeneration.SelectedValue.ToString()));

                    txtScore.Text = "";
                    txtIDCard.Text = "";
                    txtStudentName.Text = "";
                    txtScore.Focus();

                    int rowEffect = com.ExecuteNonQuery();
                    loadData_score();
                }
                else
                {

                    SqlCommand com = new SqlCommand("updateExam", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ExamID", int.Parse(txtStudentName.Tag.ToString()));
                    com.Parameters.AddWithValue("@ExamScore", txtScore.Text.ToString());
                    com.Parameters.AddWithValue("@ExamTypeID", int.Parse(cbxExamType.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@SubjectID", int.Parse(cbxSubject.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@ProfessorID", int.Parse(cbxProfessor.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@SemesterID", int.Parse(cbxSemester.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@StudentIDCard", txtIDCard.Text.ToString());
                    com.Parameters.AddWithValue("@ClassID", int.Parse(txtClassName.Tag.ToString()));
                    com.Parameters.AddWithValue("@GenerationID", int.Parse(cbxGeneration.SelectedValue.ToString()));

                    txtScore.Text = "";
                    txtIDCard.Text = "";
                    txtStudentName.Text = "";
                    txtScore.Focus();

                    int rowEffect = com.ExecuteNonQuery();
                    loadData_score();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtStudentName.Tag == null)
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("deleteExam", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ExamID", int.Parse(txtStudentName.Tag.ToString()));

                int rowEffect = com.ExecuteNonQuery();

                txtScore.Text = "";
                txtIDCard.Text = "";
                txtStudentName.Text = "";
                txtScore.Focus();
                txtStudentName.Tag = null;
                loadData_score();
            }
        }

        private void txtScore_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtIDCard, txtIDCard);
        }
    }
}
