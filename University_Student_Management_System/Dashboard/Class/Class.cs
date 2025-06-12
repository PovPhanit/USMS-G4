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
using University_Student_Management_System.Dashboard.Room;

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
        public void Fillcbx(ComboBox cbx, string fd1, string fd2, string TB2)
        {
            DA = new SqlDataAdapter("select " + fd1 + "," + fd2 + " From " + TB2, Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            cbx.DataSource = TB;
            cbx.DisplayMember = fd2;
            cbx.ValueMember = fd1;
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
                itemPanel.Size = new Size(120, 120);
                itemPanel.Margin = new Padding(5);


                PictureBox pic = new PictureBox();
                pic.Image = image;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(50, 50);
                pic.Location = new Point(35, 0);
                itemPanel.Controls.Add(pic);

                Label lblRoomNumber = new Label();
                lblRoomNumber.Text = kvp.Key.RoomNumber.ToString();
                lblRoomNumber.ForeColor = Color.Black;
                lblRoomNumber.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblRoomNumber.TextAlign = ContentAlignment.MiddleCenter;
                lblRoomNumber.Size = new Size(120, 20);
                lblRoomNumber.Location = new Point(0, 48);
                itemPanel.Controls.Add(lblRoomNumber);


                int spaceY = 65;
                foreach (var entry in kvp.Value)
                {
                   
                    Console.WriteLine($"{entry.TimesID} {entry.TimesSlot}  {entry.Status}");
                    Label lblSlot = new Label();
                    lblSlot.Text = entry.TimesSlot;
                    lblSlot.ForeColor = Color.Black;
                    lblSlot.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    lblSlot.Cursor = Cursors.Hand;
                    lblSlot.TextAlign = ContentAlignment.MiddleCenter;
                    lblSlot.Size = new Size(120, 20);
                    lblSlot.Location = new Point(0, spaceY);
                    spaceY += 17;

                    if (!isCreateUPdate)
                    {
                        // Status label click
                        lblSlot.Click += (s, e2) =>
                        {

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

        private void DisplayClassOfLevel(int levelID,int departmentID, string classGeneration)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("viewAllRoomCreateByGeneration", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@LevelID", levelID);
            DA.SelectCommand.Parameters.AddWithValue("@DepartmentID", departmentID);
            DA.SelectCommand.Parameters.AddWithValue("@ClassGeneration", classGeneration);
            TB = new DataTable();
            Image image = Image.FromFile("../../Resources/roomDisplay.png");
            DA.Fill(TB);
            //panelClassContainer2.Controls.Clear();

            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.WrapContents = true;
            flow.AutoScroll = true;
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.Padding = new Padding(3);

            foreach (DataRow dr in TB.Rows)
            {
                Panel itemPanel = new Panel();
                itemPanel.Size = new Size(180, 200);
                itemPanel.Margin = new Padding(5);

                PictureBox pic = new PictureBox();
                pic.Image = image;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(100, 100);
                pic.Location = new Point(40, 10);

                if (!isCreateUPdate)
                {
                    pic.Click += (s, e1) =>
                    {
                        txtClassName.Tag = new Tuple<string, string>(dr["ClassID"].ToString(), dr["RoomID"].ToString());

                        cbxLevelName.SelectedValue = int.Parse(dr["LevelID"].ToString());
                        cbxDepartmentName.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                        cbxTimesSlot.SelectedValue = int.Parse(dr["timesID"].ToString());

                        txtClassName.Text = dr["ClassName"].ToString();
                        dtpClassStartdate.Value = Convert.ToDateTime(dr["class_startdate"].ToString());
                        dtpClassEnddate.Value = Convert.ToDateTime(dr["class_enddate"].ToString());
                        //txtClassGeneration.Text = dr["classGeneration"].ToString();
                        cbxClassAvailable.Text = dr["classAvailable"].ToString();
                        txtClassCountEnroll.Text = dr["ClassCountEnroll"].ToString();
                        
                        txtRoomID.Text = dr["RoomNumber"].ToString();
                    };
                }

                Label lblClassName = new Label();
                lblClassName.Text = dr["ClassName"].ToString();
                lblClassName.ForeColor = Color.Black;
                lblClassName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblClassName.TextAlign = ContentAlignment.MiddleCenter;
                lblClassName.Size = new Size(160, 20);
                lblClassName.Location = new Point(10, 120);

                if (!isCreateUPdate)
                {
                    lblClassName.Click += (s, e2) =>
                    {
                        txtClassName.Tag = new Tuple<string, string>(dr["ClassID"].ToString(), dr["RoomID"].ToString());

                        cbxLevelName.SelectedValue = int.Parse(dr["LevelID"].ToString());
                        cbxDepartmentName.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                        cbxTimesSlot.SelectedValue = int.Parse(dr["timesID"].ToString());

                        txtClassName.Text = dr["ClassName"].ToString();
                        dtpClassStartdate.Value = Convert.ToDateTime(dr["class_startdate"].ToString());
                        dtpClassEnddate.Value = Convert.ToDateTime(dr["class_enddate"].ToString());
                        //txtClassGeneration.Text = dr["classGeneration"].ToString();
                        cbxClassAvailable.Text = dr["classAvailable"].ToString();
                        txtClassCountEnroll.Text = dr["ClassCountEnroll"].ToString();

                        txtRoomID.Text = dr["RoomNumber"].ToString();
                    };
                }

                Label lblRoomNumber = new Label();
                lblRoomNumber.Text = dr["RoomNumber"].ToString();
                lblRoomNumber.ForeColor = Color.Black;
                lblRoomNumber.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblRoomNumber.TextAlign = ContentAlignment.MiddleCenter;
                lblRoomNumber.Size = new Size(160, 20);
                lblRoomNumber.Location = new Point(10, 140);

                if (!isCreateUPdate)
                {
                    lblRoomNumber.Click += (s, e3) =>
                    {
                        txtClassName.Tag = new Tuple<string, string>(dr["ClassID"].ToString(), dr["RoomID"].ToString());

                        cbxLevelName.SelectedValue = int.Parse(dr["LevelID"].ToString());
                        cbxDepartmentName.SelectedValue = int.Parse(dr["DepartmentID"].ToString());
                        cbxTimesSlot.SelectedValue = int.Parse(dr["timesID"].ToString());

                        txtClassName.Text = dr["ClassName"].ToString();
                        dtpClassStartdate.Value = Convert.ToDateTime(dr["class_startdate"].ToString());
                        dtpClassEnddate.Value = Convert.ToDateTime(dr["class_enddate"].ToString());
                        //txtClassGeneration.Text = dr["classGeneration"].ToString();
                        cbxClassAvailable.Text = dr["classAvailable"].ToString();
                        txtClassCountEnroll.Text = dr["ClassCountEnroll"].ToString();

                        txtRoomID.Text = dr["RoomNumber"].ToString();
                    };
                }

               
                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblClassName);
                itemPanel.Controls.Add(lblRoomNumber);
                flow.Controls.Add(itemPanel);
            }

            //panelClassContainer2.Controls.Add(flow);
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

            isLoadBuilding = true;

            if (cbxBuilding.Items.Count > 0 && cbxBuilding.SelectedValue != null)
            {
                DisplayRoomOfBuiling(Convert.ToInt32(cbxBuilding.SelectedValue));
            }

            loadData();
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
                    if (row.Cells["Status"].Value.ToString() == "available")
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
                }
            }
        }

        
        //operation show building
        private void cbxBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadBuilding) return;
            //DisplayClassOfBuiling(int.Parse(cbxBuilding.SelectedValue.ToString()));
        }

        //Operation show class
        private void cbxLevelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadBuilding) return;
            int levelID = int.Parse(cbxLevelName.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartmentName.SelectedValue.ToString());
            //string classGeneration = txtClassGeneration.Text;

            //DisplayClassOfLevel(levelID, departmentID, classGeneration);
        }
        private void cbxDepartmentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadBuilding) return;
            int levelID = int.Parse(cbxLevelName.SelectedValue.ToString());
            int departmentID = int.Parse(cbxDepartmentName.SelectedValue.ToString());
            //string classGeneration = txtClassGeneration.Text;

            //DisplayClassOfLevel(levelID, departmentID, classGeneration);
        }

        //operation crud
        private void btnNew_Click(object sender, EventArgs e)
        {
           
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(txtRoomID.Tag.ToString());
            //if (btnNew.Text == "បោះបង់")
            //{
            //    // Validate RoomID first
            //    if (txtRoomID.Tag == null)
            //    {
            //        MessageBox.Show("Please select a room first");
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(txtClassName.Text.Trim()))
            //    {
            //        MessageBox.Show("Please Input class Name...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        txtRoomID.Focus();
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(txtClassCountEnroll.Text.Trim()))
            //    {
            //        MessageBox.Show("Please Input class count enroll...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        txtClassCountEnroll.Focus();
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(txtClassGeneration.Text.Trim()))
            //    {
            //        MessageBox.Show("Please Input class generation...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        txtClassGeneration.Focus();
            //        return;
            //    }
            //    if (txtRoomID.Tag == null)
            //    {
            //        MessageBox.Show("Please select a room first");
            //        return;
            //    }
            //    if (isCreateUPdate)
            //    {
            //        string classAvailable = cbxClassAvailable.SelectedValue?.ToString() ?? "available";

            //        SqlCommand com = new SqlCommand("insertClass", Operation.con);
            //        com.CommandType = CommandType.StoredProcedure;
            //        // com.Parameters.AddWithValue("@RoomID", int.Parse(txtRoomID.Text));
            //        com.Parameters.AddWithValue("@RoomID", int.Parse(txtRoomID.Tag.ToString()));

            //        com.Parameters.AddWithValue("@LevelID", int.Parse(cbxLevelName.SelectedValue.ToString()));
            //        com.Parameters.AddWithValue("@timesID", int.Parse(cbxTimesSlot.SelectedValue.ToString()));
            //        com.Parameters.AddWithValue("@DepartmentID", int.Parse(cbxDepartmentName.SelectedValue.ToString()));
            //        com.Parameters.AddWithValue("@ClassName", txtClassName.Text);
            //        com.Parameters.AddWithValue("@class_startdate", dtpClassStartdate.Value);
            //        com.Parameters.AddWithValue("@class_enddate", dtpClassEnddate.Value);
            //        com.Parameters.AddWithValue("@ClassCountEnroll", int.Parse(txtClassCountEnroll.Text));
            //        com.Parameters.AddWithValue("@ClassDescription", DBNull.Value);
            //        com.Parameters.AddWithValue("@classAvailable", classAvailable);
            //        com.Parameters.AddWithValue("@classGeneration", int.Parse(txtClassGeneration.Text));
            //        // com.Parameters.AddWithValue("@timesSlot", cbxTimesSlot.Text);
            //        int rowEffect = com.ExecuteNonQuery();
            //        if (rowEffect > 0)
            //        {
            //            loadData();
            //            MessageBox.Show("insert class is success");
            //            DisplayClassOfBuiling(int.Parse(cbxBuilding.SelectedValue.ToString()));
            //        }
            //        else
            //        {
            //            MessageBox.Show("No rows were affected. Check if room is a Lecture Hall or if class already exists.");
            //        }
            //    }
            //    else
            //    {
            //        SqlCommand com = new SqlCommand("updateClass", Operation.con);
            //        com.CommandType = CommandType.StoredProcedure;
            //        com.Parameters.AddWithValue("@LevelID", int.Parse(cbxLevelName.SelectedValue.ToString()));
            //        com.Parameters.AddWithValue("@timesID", int.Parse(cbxTimesSlot.SelectedValue.ToString()));
            //        com.Parameters.AddWithValue("@DepartmentID", int.Parse(cbxDepartmentName.SelectedValue.ToString()));
            //        com.Parameters.AddWithValue("@ClassName", txtClassName.Text.ToString());
            //        com.Parameters.AddWithValue("@class_startdate", dtpClassStartdate.Text.ToString());
            //        com.Parameters.AddWithValue("@class_enddate", dtpClassEnddate.Text.ToString());
            //        com.Parameters.AddWithValue("@ClassCountEnroll", txtClassCountEnroll.Text.ToString());
            //        com.Parameters.AddWithValue("@classAvailable", int.Parse(cbxClassAvailable.SelectedValue.ToString()));
            //        com.Parameters.AddWithValue("@ClassDescription", DBNull.Value);
            //        com.Parameters.AddWithValue("@classGeneration", txtClassGeneration.Text.ToString());
            //        com.Parameters.AddWithValue("@timesSlot", int.Parse(cbxTimesSlot.SelectedValue.ToString()));
            //        int rowEffect = com.ExecuteNonQuery();
            //        loadData();
            //        int levelID = int.Parse(cbxLevelName.SelectedValue.ToString());
            //        int departmentID = int.Parse(cbxDepartmentName.SelectedValue.ToString());
            //        string classGeneration = txtClassGeneration.Text;

            //        DisplayClassOfLevel(levelID, departmentID, classGeneration);
            //    }
            //}

        }


 
    }
}
