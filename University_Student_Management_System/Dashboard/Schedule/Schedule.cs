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

namespace University_Student_Management_System.Dashboard.Schedule
{
    public partial class Schedule : Form
    {
        public Schedule()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        bool isCreateUPdate = false;
        bool isLoadBuilding = false;
        bool isLoadClass = false;
        public void Fillcbx(ComboBox cbx, string fd1, string fd2, string TB2)
        {
            DA = new SqlDataAdapter("select " + fd1 + "," + fd2 + " From " + TB2, Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            cbx.DataSource = TB;
            cbx.DisplayMember = fd2;
            cbx.ValueMember = fd1;
        }
        
        private void DisplayClass(int levelID, int deID, int generationID)
        {

            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("viewAllRoomCreateByGeneration", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@LevelID", levelID);
            DA.SelectCommand.Parameters.AddWithValue("@DepartmentID", deID);
            DA.SelectCommand.Parameters.AddWithValue("@GenerationID", generationID);

            TB = new DataTable();
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

                PictureBox pic = new PictureBox();
                pic.Image = image;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(50, 50);
                pic.Location = new Point(25, 0);

                if (!isCreateUPdate)
                {
                    pic.Click += (s, e1) =>
                    {
                        txtClassID.Tag = dr["ClassID"].ToString();

                        cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxLevel.SelectedValue = int.Parse(dr["LevelID"].ToString());
                        
                        txtClassID.Text = dr["ClassName"].ToString();
                        DisplaySchedule(int.Parse(dr["ClassID"].ToString()), int.Parse(dr["GenerationID"].ToString()));

                    };
                }

                Label lblClassName = new Label();
                lblClassName.Text =  dr["ClassName"].ToString();
                if (dr["classAvailable"].ToString().StartsWith("available"))
                {
                    lblClassName.ForeColor = Color.Green;
                }
                else
                {
                    lblClassName.ForeColor = Color.Red;
                }

                lblClassName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblClassName.TextAlign = ContentAlignment.MiddleCenter;
                lblClassName.Size = new Size(100, 20);
                lblClassName.Location = new Point(0, 48);

                if (!isCreateUPdate)
                {
                    lblClassName.Click += (s, e2) =>
                    {
                        txtClassID.Tag = dr["ClassID"].ToString();
                        
                        cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxLevel.SelectedValue = int.Parse(dr["LevelID"].ToString());

                        txtClassID.Text = dr["ClassName"].ToString();
                        DisplaySchedule(int.Parse(dr["ClassID"].ToString()), int.Parse(dr["GenerationID"].ToString()));
                    };
                }

                Label lblRoomNumber = new Label();
                lblRoomNumber.Text = dr["BuildingName"].ToString().Split(' ')[1] + " - " + dr["RoomNumber"].ToString();
                lblRoomNumber.ForeColor = Color.Black;
                lblRoomNumber.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblRoomNumber.TextAlign = ContentAlignment.MiddleCenter;
                lblRoomNumber.Size = new Size(100, 20);
                lblRoomNumber.Location = new Point(0, 65);

                if (!isCreateUPdate)
                {
                    lblRoomNumber.Click += (s, e3) =>
                    {
                        txtClassID.Tag = dr["ClassID"].ToString();

                        cbxDepartment.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxLevel.SelectedValue = int.Parse(dr["LevelID"].ToString());

                        txtClassID.Text = dr["ClassName"].ToString();
                        DisplaySchedule(int.Parse(dr["ClassID"].ToString()), int.Parse(dr["GenerationID"].ToString()));
                    };
                }

                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblClassName);
                itemPanel.Controls.Add(lblRoomNumber);
                flow.Controls.Add(itemPanel);
            }

            panelClassContainer1.Controls.Add(flow);
        }

        private void DisplaySchedule(int classID, int generationIDs)
        {

            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("viewScheduleByClass", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@ClassID", classID);
            DA.SelectCommand.Parameters.AddWithValue("@GenerationID", generationIDs);

            TB = new DataTable();
            Image image = Image.FromFile("../../Resources/bookDisplay.png");
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

                PictureBox pic = new PictureBox();
                pic.Image = image;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(50, 50);
                pic.Location = new Point(25, 0);

                if (!isCreateUPdate)
                {
                    pic.Click += (s, e1) =>
                    {
                        txtClassID.Tag = dr["ClassID"].ToString();
                        btnEdit.Tag = dr["ScheduleID"].ToString();

                        cbxProfessor.SelectedValue = int.Parse(dr["ProfessorID"].ToString());
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxSubject.SelectedValue = int.Parse(dr["SubjectID"].ToString());
                        cbxSemester.SelectedValue = int.Parse(dr["SemesterID"].ToString());
                        
                        dtpScheduleStartdate.Text = dr["ScheduleStart"].ToString();
                        dtpScheduleEnddate.Text = dr["ScheduleEnd"].ToString();
                        cbxScheduleDayOfWeek.Text = dr["ScheduleDayOfWeek"].ToString();
                    };
                }

                Label lblSubjectTitle = new Label();
                lblSubjectTitle.Text = dr["SubjectTitle"].ToString();
                lblSubjectTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblSubjectTitle.TextAlign = ContentAlignment.MiddleCenter;
                lblSubjectTitle.Size = new Size(100, 20);
                lblSubjectTitle.Location = new Point(0, 48);

                if (!isCreateUPdate)
                {
                    lblSubjectTitle.Click += (s, e2) =>
                    {
                        txtClassID.Tag = dr["ClassID"].ToString();
                        btnEdit.Tag = dr["ScheduleID"].ToString();

                        cbxProfessor.SelectedValue = int.Parse(dr["ProfessorID"].ToString());
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxSubject.SelectedValue = int.Parse(dr["SubjectID"].ToString());
                        cbxSemester.SelectedValue = int.Parse(dr["SemesterID"].ToString());

                        dtpScheduleStartdate.Text = dr["ScheduleStart"].ToString();
                        dtpScheduleEnddate.Text = dr["ScheduleEnd"].ToString();
                        cbxScheduleDayOfWeek.Text = dr["ScheduleDayOfWeek"].ToString();
                    };
                }

                Label lblProfessorNameKH = new Label();
                lblProfessorNameKH.Text = dr["ProfessorNameKH"].ToString();
                lblProfessorNameKH.ForeColor = Color.Black;
                lblProfessorNameKH.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblProfessorNameKH.TextAlign = ContentAlignment.MiddleCenter;
                lblProfessorNameKH.Size = new Size(100, 20);
                lblProfessorNameKH.Location = new Point(0, 65);

                if (!isCreateUPdate)
                {
                    lblProfessorNameKH.Click += (s, e3) =>
                    {
                        txtClassID.Tag = dr["ClassID"].ToString();
                        btnEdit.Tag = dr["ScheduleID"].ToString();

                        cbxProfessor.SelectedValue = int.Parse(dr["ProfessorID"].ToString());
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxSubject.SelectedValue = int.Parse(dr["SubjectID"].ToString());
                        cbxSemester.SelectedValue = int.Parse(dr["SemesterID"].ToString());

                        dtpScheduleStartdate.Text = dr["ScheduleStart"].ToString();
                        dtpScheduleEnddate.Text = dr["ScheduleEnd"].ToString();
                        cbxScheduleDayOfWeek.Text = dr["ScheduleDayOfWeek"].ToString();
                    };
                }

                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblSubjectTitle);
                itemPanel.Controls.Add(lblProfessorNameKH);
                flow.Controls.Add(itemPanel);
            }

            panelClassContainer2.Controls.Add(flow);
        }


        private void Schedule_Load(object sender, EventArgs e)
        {
            string[] days = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            cbxScheduleDayOfWeek.DataSource = days;

            Fillcbx(cbxLevel, "levelID", "levelName", "level");
            Fillcbx(cbxProfessor, "professorID", "professorNameKH", "professor");
            Fillcbx(cbxSubject, "subjectID", "subjectTitle", "subject");
            Fillcbx(cbxSemester, "semesterID", "semesterName", "semester");
            Fillcbx(cbxDepartment, "departmentID", "departmentName", "department");
            Fillcbx(cbxGeneration, "GenerationID", "GenerationName", "Generation");

            dtpScheduleStartdate.Value = DateTime.Today.AddHours(2);
            dtpScheduleEnddate.Value = DateTime.Today.AddHours(3).AddMinutes(30);

            isLoadBuilding = true;

            if (cbxGeneration.Items.Count > 0 && cbxGeneration.SelectedValue != null)
            {
                int generationIDs = Convert.ToInt32(cbxGeneration.SelectedValue);
                int classID = txtClassID.Tag != null ? Convert.ToInt32(txtClassID.Tag) : 0;
                DisplaySchedule(classID, generationIDs);
            }



            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
        }


        private void cbxLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
        }

        private void cbxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
        }

        private void cbxGeneration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);

            if (cbxGeneration.SelectedValue == null)
            {
                DisplayClass(levelID, departmentID, 0);
            }
            else
            {
                DisplayClass(levelID, departmentID, generationID);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
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
                int generationID;

                if (string.IsNullOrEmpty(dtpScheduleStartdate.Text.Trim()))
                {
                    MessageBox.Show("Please Input class Name...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpScheduleStartdate.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(dtpScheduleEnddate.Text.Trim()))
                {
                    MessageBox.Show("Please Input class count enroll...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpScheduleEnddate.Focus();
                    return;
                }
                else if (txtClassID.Tag == null || string.IsNullOrEmpty(txtClassID.Text.Trim()))
                {
                    MessageBox.Show("Please select a room first");
                    return;
                }

                if (isCreateUPdate)
                {
                    generationID = cbxGeneration.SelectedValue != null ?
                        int.Parse(cbxGeneration.SelectedValue.ToString()) : 0;

                    SqlCommand com = new SqlCommand("createSchedule", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@ScheduleStart", dtpScheduleStartdate.Value.TimeOfDay);
                    com.Parameters.AddWithValue("@ScheduleEnd", dtpScheduleEnddate.Value.TimeOfDay);
                    com.Parameters.AddWithValue("@ScheduleDayofWeek", cbxScheduleDayOfWeek.SelectedItem.ToString());
                    com.Parameters.AddWithValue("@ScheduleDate", DateTime.Now.Date);
                    com.Parameters.AddWithValue("@GenerationID", generationID);
                    com.Parameters.AddWithValue("@ClassID", Convert.ToInt32(txtClassID.Tag));
                    com.Parameters.AddWithValue("@ProfessorID", cbxProfessor.SelectedValue);
                    com.Parameters.AddWithValue("@SubjectID", cbxSubject.SelectedValue);
                    com.Parameters.AddWithValue("@SemesterID", cbxSemester.SelectedValue);

                    int rowEffect = com.ExecuteNonQuery();
                }
                else
                {
                    generationID = cbxGeneration.SelectedValue != null ?
                        int.Parse(cbxGeneration.SelectedValue.ToString()) : 0;

                    SqlCommand com = new SqlCommand("updateSchedule", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@ScheduleID", Convert.ToInt32(btnEdit.Tag));
                    com.Parameters.AddWithValue("@ScheduleStart", dtpScheduleStartdate.Value.TimeOfDay);
                    com.Parameters.AddWithValue("@ScheduleEnd", dtpScheduleEnddate.Value.TimeOfDay);
                    com.Parameters.AddWithValue("@ScheduleDayofWeek", cbxScheduleDayOfWeek.SelectedItem.ToString());
                    com.Parameters.AddWithValue("@ScheduleDate", DateTime.Now.Date);
                    com.Parameters.AddWithValue("@GenerationID", generationID);
                    com.Parameters.AddWithValue("@ClassID", Convert.ToInt32(txtClassID.Tag));
                    com.Parameters.AddWithValue("@ProfessorID", cbxProfessor.SelectedValue);
                    com.Parameters.AddWithValue("@SubjectID", cbxSubject.SelectedValue);
                    com.Parameters.AddWithValue("@SemesterID", cbxSemester.SelectedValue);
                    
                    int rowEffect = com.ExecuteNonQuery();
                }

                int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
                int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
                int generationsID = int.Parse(cbxGeneration.SelectedValue.ToString());
                DisplayClass(levelID, departmentID, generationsID);

                int generationIDs = Convert.ToInt32(cbxGeneration.SelectedValue);
                int classID = txtClassID.Tag != null ? Convert.ToInt32(txtClassID.Tag) : 0;
                DisplaySchedule(classID, generationIDs);
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
            if (btnEdit.Tag == null)
            {
                MessageBox.Show("Please select a schedule to delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult re = MessageBox.Show("Do you want to delete this schedule?", "Delete Schedule",
                                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("deleteSchedule", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ScheduleID", Convert.ToInt32(btnEdit.Tag));
                int rowEffect = com.ExecuteNonQuery();

                if (rowEffect > 0)
                {
                    MessageBox.Show("Schedule deleted successfully!");

                    int levelID = int.Parse(cbxLevel.SelectedValue.ToString());
                    int departmentID = int.Parse(cbxDepartment.SelectedValue.ToString());
                    int generationsID = int.Parse(cbxGeneration.SelectedValue.ToString());
                    DisplayClass(levelID, departmentID, generationsID);

                    int generationIDs = Convert.ToInt32(cbxGeneration.SelectedValue);
                    int classID = txtClassID.Tag != null ? Convert.ToInt32(txtClassID.Tag) : 0;
                    DisplaySchedule(classID, generationIDs);

                    btnEdit.Tag = null;
                }
                else
                {
                    MessageBox.Show("No schedule was deleted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
