using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using University_Student_Management_System.Dashboard.Department;
using University_Student_Management_System.Dashboard.Level;
using University_Student_Management_System.Dashboard.Room;
using University_Student_Management_System.Dashboard.Subject;

namespace University_Student_Management_System.Dashboard.Class
{
    public partial class Class : Form
    {
        public Class()
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
                        txtClassName.Text = dr["ClassName"].ToString();
                        txtClassName.Tag = dr["ClassID"].ToString();
                        txtClassCountEnroll.Text = dr["ClassCountEnroll"].ToString();
                        cbxTimesSlot.SelectedValue = int.Parse(dr["timesID"].ToString());
                        if (dr["classAvailable"].ToString() == "available")
                        {
                            cbxClassAvailable.SelectedIndex = 0;
                        }
                        else
                        {
                            cbxClassAvailable.SelectedIndex = 1;
                        }
                        dtpClassStartdate.Text = dr["class_startdate"].ToString();
                        dtpClassEnddate.Text = dr["class_enddate"].ToString();
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxLevelName.SelectedValue = int.Parse(dr["LevelID"].ToString());
                        cbxDepartmentName.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                        txtRoomID.Text = dr["RoomNumber"].ToString();
                        txtRoomID.Tag = dr["RoomID"].ToString();

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
                        txtClassName.Text = dr["ClassName"].ToString();
                        txtClassName.Tag = dr["ClassID"].ToString();
                        txtClassCountEnroll.Text = dr["ClassCountEnroll"].ToString();
                        cbxTimesSlot.SelectedValue = int.Parse(dr["timesID"].ToString());
                        if (dr["classAvailable"].ToString() == "available")
                        {
                            cbxClassAvailable.SelectedIndex = 0;
                        }
                        else
                        {
                            cbxClassAvailable.SelectedIndex = 1;
                        }
                        dtpClassStartdate.Text = dr["class_startdate"].ToString();
                        dtpClassEnddate.Text = dr["class_enddate"].ToString();
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxLevelName.SelectedValue = int.Parse(dr["LevelID"].ToString());
                        cbxDepartmentName.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                        txtRoomID.Text = dr["RoomNumber"].ToString();
                        txtRoomID.Tag = dr["RoomID"].ToString();
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
                        txtClassName.Text = dr["ClassName"].ToString();
                        txtClassName.Tag = dr["ClassID"].ToString();
                        txtClassCountEnroll.Text = dr["ClassCountEnroll"].ToString();
                        cbxTimesSlot.SelectedValue = int.Parse(dr["timesID"].ToString());
                        if (dr["classAvailable"].ToString() == "available")
                        {
                            cbxClassAvailable.SelectedIndex = 0;
                        }
                        else
                        {
                            cbxClassAvailable.SelectedIndex = 1;
                        }
                        dtpClassStartdate.Text = dr["class_startdate"].ToString();
                        dtpClassEnddate.Text = dr["class_enddate"].ToString();
                        cbxGeneration.SelectedValue = int.Parse(dr["GenerationID"].ToString());
                        cbxLevelName.SelectedValue = int.Parse(dr["LevelID"].ToString());
                        cbxDepartmentName.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                        txtRoomID.Text = dr["RoomNumber"].ToString();
                        txtRoomID.Tag = dr["RoomID"].ToString();
                    };
                }


                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblClassName);
                itemPanel.Controls.Add(lblRoomNumber);
                flow.Controls.Add(itemPanel);
            }

            panelClassContainer2.Controls.Add(flow);
        }

        private void DisplayRoomOfBuiling(int buildingID)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("viewAllRoomCreateStatus", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@BuildingID", buildingID);
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

            var groupedData = new Dictionary<(int RoomID, int RoomNumber), List<(int TimesID, string TimesSlot, string Status)>>();
            foreach (DataRow dr in TB.Rows)
            {
                int roomID = Convert.ToInt32(dr["RoomID"]);
                int roomNumber = Convert.ToInt32(dr["RoomNumber"]);
                int timesID = Convert.ToInt32(dr["timesID"]);
                string timesSlot = dr["timesSlot"].ToString();
                string status = dr["Status"].ToString();

                var key = (roomID, roomNumber);

                if (!groupedData.ContainsKey(key))
                {
                    groupedData[key] = new List<(int, string, string)>();
                }
                groupedData[key].Add((timesID, timesSlot, status));
            }




            
            foreach (var kvp in groupedData)
            {
                Console.WriteLine($"{kvp.Key.RoomID} - {kvp.Key.RoomNumber}");
                Panel itemPanel = new Panel();
                itemPanel.Size = new Size(100, 120);
                itemPanel.Margin = new Padding(5);


                PictureBox pic = new PictureBox();
                pic.Image = image;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(50, 50);
                pic.Location = new Point(25, 0);
                itemPanel.Controls.Add(pic);

                Label lblRoomNumber = new Label();
                lblRoomNumber.Text = kvp.Key.RoomNumber.ToString();
                lblRoomNumber.ForeColor = Color.Black;
                lblRoomNumber.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblRoomNumber.TextAlign = ContentAlignment.MiddleCenter;
                lblRoomNumber.Size = new Size(100, 20);
                lblRoomNumber.Location = new Point(0, 48);
                itemPanel.Controls.Add(lblRoomNumber);


                int spaceY = 65;
                foreach (var entry in kvp.Value)
                {
                   
                    Console.WriteLine($"{entry.TimesID} {entry.TimesSlot}  {entry.Status}");
                    Label lblSlot = new Label();
                    lblSlot.Text = entry.TimesSlot;

                    lblSlot.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    lblSlot.TextAlign = ContentAlignment.MiddleCenter;
                    lblSlot.Size = new Size(100, 20);
                  
                    lblSlot.Location = new Point(0, spaceY);
                    spaceY += 17;
                    if (entry.Status.Contains("unavailable"))
                    {
                        lblSlot.Cursor = Cursors.No;
                        lblSlot.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblSlot.Cursor = Cursors.Hand;
                        lblSlot.ForeColor = Color.Green;
                   
                            // Status label click
                            lblSlot.Click += (s, e2) =>
                            {
                                //MessageBox.Show($"{entry.TimesID} {entry.TimesSlot}  {entry.Status}");
                                cbxTimesSlot.SelectedValue=int.Parse(entry.TimesID.ToString());
                                txtRoomID.Text = kvp.Key.RoomNumber.ToString();
                                txtRoomID.Tag = kvp.Key.RoomID.ToString();
                            };
                        
                    }


                   
                    itemPanel.Controls.Add(lblSlot);
                }
                flow.Controls.Add(itemPanel);
            }
            panelClassContainer1.Controls.Add(flow);

        }

        private void loadData()
        {
            DA = new SqlDataAdapter("SELECT * from viewClass", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            dgvClass.DataSource = TB;
            dgvClass.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvClass.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvClass.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClass.Columns["ClassID"].Visible = false;
            dgvClass.Columns["timesID"].Visible = false;
            dgvClass.Columns["LevelID"].Visible = false;
            dgvClass.Columns["DepartmentID"].Visible = false;
            dgvClass.Columns["RoomID"].Visible = false;
            dgvClass.Columns["GenerationID"].Visible = false;
            foreach (DataGridViewColumn col in dgvClass.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void fieldStatus()
        {
            cbxClassAvailable.Items.Add("Available");
            cbxClassAvailable.Items.Add("Unavailable");
            cbxClassAvailable.SelectedIndex = 0;
        }

        private void Class_Load(object sender, EventArgs e)
        {
            Fillcbx(cbxLevelName, "levelID", "levelName", "level");
            Fillcbx(cbxBuilding, "buildingID", "buildingName", "building");
            Fillcbx(cbxTimesSlot, "timesID", "timesSlot", "times");
            Fillcbx(cbxDepartmentName, "departmentID", "departmentName", "department");
            Fillcbx(cbxGeneration, "GenerationID", "GenerationName", "Generation");
            cbxGeneration.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbxGeneration.AutoCompleteSource = AutoCompleteSource.ListItems;
            fieldStatus();
            txtClassName.Focus();
            txtSearch.Text = "Search class hear...";
            txtSearch.ForeColor = Color.Gray;
            loadData();

            isLoadBuilding = true;
            if (cbxBuilding.Items.Count > 0 && cbxBuilding.SelectedValue != null)
            {
                DisplayRoomOfBuiling(Convert.ToInt32(cbxBuilding.SelectedValue));
            }

            isLoadClass = true;
            int levelID = int.Parse(cbxLevelName.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartmentName.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
            
        }
        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search class hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search class hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("SearchClassByDepartment", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@Keyword", txtSearch.Text.Trim());
            TB = new DataTable();
            DA.Fill(TB);
            dgvClass.DataSource = TB;
        }

        private void dgvClass_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate)
            {
                if (e.RowIndex >= 0) // Make sure it's not a header
                {
                    DataGridViewRow row = dgvClass.Rows[e.RowIndex];
                    txtClassName.Tag = row.Cells["ClassID"].Value.ToString();
                    txtClassName.Text = row.Cells["Class Name"].Value.ToString();
                    cbxTimesSlot.SelectedValue = int.Parse(row.Cells["timesID"].Value.ToString());
                    if (row.Cells["Status"].Value.ToString().Trim() == "available")
                    {
                        cbxClassAvailable.SelectedIndex = 0;
                    }
                    else
                    {
                        cbxClassAvailable.SelectedIndex = 1;
                    }
                    txtClassCountEnroll.Text = row.Cells["Total Enroll"].Value.ToString();
                    dtpClassStartdate.Text = row.Cells["Start Date"].Value.ToString();
                    dtpClassEnddate.Text = row.Cells["End Date"].Value.ToString();
                    cbxGeneration.SelectedValue = int.Parse(row.Cells["GenerationID"].Value.ToString());
                    cbxLevelName.SelectedValue = int.Parse(row.Cells["LevelID"].Value.ToString());
                    cbxDepartmentName.SelectedValue = int.Parse(row.Cells["DepartmentID"].Value.ToString());
                    txtRoomID.Text = row.Cells["Room Number"].Value.ToString();
                    txtRoomID.Tag = row.Cells["RoomID"].Value.ToString(); 
                }
            }
        }

        
        //operation show building
        private void cbxBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadBuilding) return;
            DisplayRoomOfBuiling(int.Parse(cbxBuilding.SelectedValue.ToString()));
        }

        //Operation show class
        private void cbxLevelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            int levelID = int.Parse(cbxLevelName.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartmentName.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
        }
        private void cbxDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            int levelID = int.Parse(cbxLevelName.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartmentName.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
        }
        private void cbxGeneration_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadClass) return;
            int levelID = int.Parse(cbxLevelName.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartmentName.SelectedValue.ToString());
            int generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
            DisplayClass(levelID, departmentID, generationID);
            
            if(cbxGeneration.SelectedValue == null)
            {
                DisplayClass(levelID, departmentID, 0);
            }
            else
            {
                DisplayClass(levelID, departmentID, generationID);
            }
        }

        //operation crud
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                txtClassCountEnroll.Text = "0";
                txtClassName.Text = "";
                txtRoomID.Text = "";
                txtRoomID.Tag = "";
                txtClassName.Focus();
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បោះបង់")
            {
                int generationID;

                if (string.IsNullOrEmpty(txtClassName.Text.Trim()))
                {
                    MessageBox.Show("Please Input class Name...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRoomID.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtClassCountEnroll.Text.Trim()))
                {
                    MessageBox.Show("Please Input class count enroll...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtClassCountEnroll.Focus();
                    return;
                }
                else if (txtRoomID.Tag == null || string.IsNullOrEmpty(txtRoomID.Text.Trim()))
                {
                    MessageBox.Show("Please select a room first");
                    return;
                }
                if (isCreateUPdate)
                {
                   
                    if (cbxGeneration.SelectedValue != null)
                    {
                        generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
                    }
                    else
                    {
                        generationID = 0;
                    }
                    SqlCommand com = new SqlCommand("insertClass", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@RoomID", int.Parse(txtRoomID.Tag.ToString()));
                    com.Parameters.AddWithValue("@LevelID", int.Parse(cbxLevelName.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@timesID", int.Parse(cbxTimesSlot.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@DepartmentID", int.Parse(cbxDepartmentName.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@GenerationID", generationID);
                    com.Parameters.AddWithValue("@GenerationName", cbxGeneration.Text.ToString());
                    com.Parameters.AddWithValue("@ClassName", txtClassName.Text.ToString());
                    com.Parameters.AddWithValue("@class_startdate", dtpClassStartdate.Value);
                    com.Parameters.AddWithValue("@class_enddate", dtpClassEnddate.Value);
                    com.Parameters.AddWithValue("@ClassCountEnroll", int.Parse(txtClassCountEnroll.Text.ToString()));
                    com.Parameters.AddWithValue("@ClassDescription", DBNull.Value);
                    com.Parameters.AddWithValue("@classAvailable", cbxClassAvailable.Text.ToString().ToLower());
                    int rowEffect = com.ExecuteNonQuery();
                    txtClassName.Text = "";
                    txtRoomID.Text = "";
                    txtRoomID.Tag = "";
                    txtClassName.Focus(); 
                }
                else
                {
                  
                    if (cbxGeneration.SelectedValue != null)
                    {
                        generationID = int.Parse(cbxGeneration.SelectedValue.ToString());
                    }
                    else
                    {
                        generationID = 0;
                    }
                    SqlCommand com = new SqlCommand("UpdateClass", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ClassID", int.Parse(txtClassName.Tag.ToString()));
                    com.Parameters.AddWithValue("@RoomID", int.Parse(txtRoomID.Tag.ToString()));
                    com.Parameters.AddWithValue("@LevelID", int.Parse(cbxLevelName.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@timesID", int.Parse(cbxTimesSlot.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@DepartmentID", int.Parse(cbxDepartmentName.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@GenerationID", generationID);
                    com.Parameters.AddWithValue("@GenerationName", cbxGeneration.Text.ToString());
                    com.Parameters.AddWithValue("@ClassName", txtClassName.Text.ToString());
                    com.Parameters.AddWithValue("@class_startdate", dtpClassStartdate.Value);
                    com.Parameters.AddWithValue("@class_enddate", dtpClassEnddate.Value);
                    com.Parameters.AddWithValue("@ClassCountEnroll", int.Parse(txtClassCountEnroll.Text.ToString()));
                    com.Parameters.AddWithValue("@ClassDescription", DBNull.Value);
                    com.Parameters.AddWithValue("@classAvailable", cbxClassAvailable.Text.ToString().ToLower());
                    int rowEffect = com.ExecuteNonQuery();
                }


                if (generationID == 0)
                {
                    Fillcbx(cbxGeneration, "GenerationID", "GenerationName", "Generation");
                }
                int levelID = int.Parse(cbxLevelName.SelectedValue.ToString());
                int departmentID = int.Parse(cbxDepartmentName.SelectedValue.ToString());
                int generationsID = int.Parse(cbxGeneration.SelectedValue.ToString());
                DisplayClass(levelID, departmentID, generationsID);
                DisplayRoomOfBuiling(int.Parse(cbxBuilding.SelectedValue.ToString()));
                loadData();
            }  

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtClassName.Tag == null)
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("DeleteClass", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ClassID", int.Parse(txtClassName.Tag.ToString()));
                int rowEffect = com.ExecuteNonQuery();

                txtRoomID.Text = "";
                txtClassName.Text = "";
                txtClassName.Tag = null;
                int levelID = int.Parse(cbxLevelName.SelectedValue.ToString());
                int departmentID = int.Parse(cbxDepartmentName.SelectedValue.ToString());
                int generationsID = int.Parse(cbxGeneration.SelectedValue.ToString());
                DisplayClass(levelID, departmentID, generationsID);
                DisplayRoomOfBuiling(int.Parse(cbxBuilding.SelectedValue.ToString()));
                loadData();
            }
        }
    }
}
