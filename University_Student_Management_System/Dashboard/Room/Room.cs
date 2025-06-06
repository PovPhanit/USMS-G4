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
using System.Xml.Linq;
using University_Student_Management_System.Dashboard.Building;
using University_Student_Management_System.Dashboard.ExamType;
using University_Student_Management_System.Dashboard.RoomType;

namespace University_Student_Management_System.Dashboard.Room
{
    public partial class Room : Form
    {
        public Room()
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

        private void DisplayRoom(int buildingID)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("SearchRoomByBuilding", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@BuildingID", buildingID);
            TB = new DataTable();
            Image image = Image.FromFile("../../Resources/roomDisplay.png");
            DA.Fill(TB);
            roomContainer.Controls.Clear();
            // Create FlowLayoutPanel
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

                // PictureBox click event
               
                if (!isCreateUPdate)
                {
                    pic.Click += (s, e1) =>
                    {
                        txtRoomNumber.Tag = dr["RoomID"].ToString();
                        txtRoomNumber.Text = dr["RoomNumber"].ToString();
                        txtroomCapacity.Text = dr["RoomCapacity"].ToString();
                        cbxRoomType.SelectedValue = int.Parse(dr["RoomTypeID"].ToString());
                    };
                }

                    Label lblRoomNumber = new Label();
                lblRoomNumber.Text = dr["RoomNumber"].ToString();
                lblRoomNumber.ForeColor = Color.Black;
                lblRoomNumber.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblRoomNumber.TextAlign = ContentAlignment.MiddleCenter;
                lblRoomNumber.Size = new Size(70, 20);
                lblRoomNumber.Location = new Point(30, 72);

                if (!isCreateUPdate)
                {
                    // Status label click
                        lblRoomNumber.Click += (s, e2) =>
                    {
                        txtRoomNumber.Tag = dr["RoomID"].ToString();
                        txtRoomNumber.Text = dr["RoomNumber"].ToString();
                        txtroomCapacity.Text = dr["RoomCapacity"].ToString();
                        cbxRoomType.SelectedValue = int.Parse(dr["RoomTypeID"].ToString());
                    };
                }

                Label lblRoomType = new Label();
                lblRoomType.Text = dr["RoomTypeName"].ToString();
                lblRoomType.ForeColor = Color.Black;
                lblRoomType.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblRoomType.TextAlign = ContentAlignment.MiddleCenter;
                lblRoomType.Size = new Size(70, 20);
                lblRoomType.Location = new Point(30, 90);

                // Status label click
                if (!isCreateUPdate)
                {
                    lblRoomType.Click += (s, e3) =>
                    {
                        txtRoomNumber.Tag = dr["RoomID"].ToString();
                        txtRoomNumber.Text = dr["RoomNumber"].ToString();
                        txtroomCapacity.Text = dr["RoomCapacity"].ToString();
                        cbxRoomType.SelectedValue = int.Parse(dr["RoomTypeID"].ToString());
                    };
                }
                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblRoomNumber);
                itemPanel.Controls.Add(lblRoomType);
                flow.Controls.Add(itemPanel);
            }

            // Add flow panel to the main panel
            roomContainer.Controls.Add(flow);
        }
        private void loadData()
        {
            DA = new SqlDataAdapter("SELECT * from viewRoom", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            dgvRoom.DataSource = TB;
            dgvRoom.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvRoom.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvRoom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoom.Columns["RoomID"].Visible = false;
            dgvRoom.Columns["RoomTypeID"].Visible = false;
            dgvRoom.Columns["BuildingID"].Visible = false;
            foreach (DataGridViewColumn col in dgvRoom.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //dgvRoom.Columns["RoomNumber"].Width = 200;
            //dgvRoom.Columns["Hired Date"].DefaultCellStyle.Format = "dd/MM/yy";
            //dgvRoom.Columns["Salary"].DefaultCellStyle.Format = "c";
        }
        private void Room_Load(object sender, EventArgs e)
        {
            Fillcbx(cbxBuilding,"buildingID","buildingName","building");
            isLoadBuilding = true;
            Fillcbx(cbxRoomType, "roomTypeID", "roomTypeName", "roomType");
            txtRoomNumber.Focus();
            txtSearch.Text = "Search room hear...";
            txtSearch.ForeColor = Color.Gray;

            DisplayRoom(int.Parse(cbxBuilding.SelectedValue.ToString()));

            loadData();

        }

        private void dgvRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate)
            {
                if (e.RowIndex >= 0) // Make sure it's not a header
                {
                    DataGridViewRow row = dgvRoom.Rows[e.RowIndex];
                    txtRoomNumber.Tag = row.Cells["RoomID"].Value.ToString();
                    txtRoomNumber.Text = row.Cells["Room Number"].Value.ToString();
                    txtroomCapacity.Text = row.Cells["Capacity"].Value.ToString();
                    cbxBuilding.SelectedValue = int.Parse(row.Cells["BuildingID"].Value.ToString());
                    cbxRoomType.SelectedValue = int.Parse(row.Cells["RoomTypeID"].Value.ToString());

                }
            }
        }

        private void cbxBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadBuilding) return;
            DisplayRoom(int.Parse(cbxBuilding.SelectedValue.ToString()));
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search room hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search room hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("SearchRoomByNumber", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure; 
            DA.SelectCommand.Parameters.AddWithValue("@RoomNumber", txtSearch.Text.Trim());
            TB = new DataTable();
            DA.Fill(TB);
            dgvRoom.DataSource = TB;
            dgvRoom.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvRoom.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvRoom.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoom.Columns["RoomID"].Visible = false;
            dgvRoom.Columns["RoomTypeID"].Visible = false;
            dgvRoom.Columns["BuildingID"].Visible = false;
            foreach (DataGridViewColumn col in dgvRoom.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បង្កើតថ្មី")
            {
                btnNew.BackColor = Color.IndianRed;
                btnNew.Image = University_Student_Management_System.Properties.Resources.Cancel;
                btnNew.Text = "បោះបង់";
                txtRoomNumber.Text = "";
                txtroomCapacity.Text = "";
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRoomNumber.Tag.ToString().Trim()))
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("DeleteRoom", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@RoomID", int.Parse(txtRoomNumber.Tag.ToString()));
                int rowEffect = com.ExecuteNonQuery();
                loadData();
                DisplayRoom(int.Parse(cbxBuilding.SelectedValue.ToString()));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (btnNew.Text == "បោះបង់")
            {
                if (string.IsNullOrEmpty(txtRoomNumber.Text.Trim()))
                {
                    MessageBox.Show("Please Input room number...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRoomNumber.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtroomCapacity.Text.Trim()))
                {
                    MessageBox.Show("Please Input room capacity...", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtroomCapacity.Focus();
                    return;
                }
                if (isCreateUPdate)
                {
                    SqlCommand com = new SqlCommand("InsertRoom", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@BuildingID", int.Parse(cbxBuilding.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@RoomTypeID", int.Parse(cbxRoomType.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@RoomNumber", txtRoomNumber.Text.ToString());
                    com.Parameters.AddWithValue("@RoomCapacity", txtroomCapacity.Text.ToString());
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();
                    DisplayRoom(int.Parse(cbxBuilding.SelectedValue.ToString()));
                }
                else
                {
                    SqlCommand com = new SqlCommand("UpdateRoom", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@RoomID", int.Parse(txtRoomNumber.Tag.ToString()));
                    com.Parameters.AddWithValue("@BuildingID", int.Parse(cbxBuilding.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@RoomTypeID", int.Parse(cbxRoomType.SelectedValue.ToString()));
                    com.Parameters.AddWithValue("@RoomNumber", txtRoomNumber.Text.ToString());
                    com.Parameters.AddWithValue("@RoomCapacity", txtroomCapacity.Text.ToString());
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();
                    DisplayRoom(int.Parse(cbxBuilding.SelectedValue.ToString()));
                }
            }
        }
    }
}
