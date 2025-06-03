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
        private void Room_Load(object sender, EventArgs e)
        {
            Fillcbx(cbxBuilding,"buildingID","buildingName","building");
            isLoadBuilding = true;
            Fillcbx(cbxRoomType, "roomTypeID", "roomTypeName", "roomType");
            txtRoomNumber.Focus();






            string[] names = { "Phanit1", "Phanit2", "Phanit3", "Phanit4", "Phanit5", "Phanit6" };
            string status = "No available";
            Image houseImage = Image.FromFile("C:/Phanit/2. C#/2.C-Sharp-Advance/Biggest Project C#/Project3/Project3/Project3/Resources/2.png");

            // Clear existing controls
            panel1.Controls.Clear();

            // Create FlowLayoutPanel
            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.WrapContents = true;
            flow.AutoScroll = true;
            flow.FlowDirection = FlowDirection.LeftToRight;
            flow.Padding = new Padding(10);
            flow.BackColor = Color.Blue;

            // Loop through data
            foreach (string name in names)
            {
                Panel itemPanel = new Panel();
                itemPanel.Size = new Size(150, 170);
                itemPanel.Margin = new Padding(10);
                itemPanel.BackColor = Color.Red;

                PictureBox pic = new PictureBox();
                pic.Image = houseImage;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Size = new Size(130, 100);
                pic.Location = new Point(10, 10);

                // PictureBox click event
                pic.Click += (s, e1) =>
                {
                    MessageBox.Show($"You clicked the image of: {name}", "Picture Click");
                };

                Label lblStatus = new Label();
                lblStatus.Text = status;
                lblStatus.ForeColor = Color.Yellow;
                lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                lblStatus.TextAlign = ContentAlignment.MiddleCenter;
                lblStatus.Size = new Size(130, 20);
                lblStatus.Location = new Point(10, 110);

                // Status label click
                lblStatus.Click += (s, e2) =>
                {
                    MessageBox.Show($"Status: {status} for {name}", "Status Click");
                };

                Label lblName = new Label();
                lblName.Text = name;
                lblName.ForeColor = Color.Blue;
                lblName.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                lblName.TextAlign = ContentAlignment.MiddleCenter;
                lblName.Size = new Size(130, 20);
                lblName.Location = new Point(10, 130);

                // Name label click
                lblName.Click += (s, e3) =>
                {
                    MessageBox.Show($"Name: {name}", "Name Click");
                };

                itemPanel.Controls.Add(pic);
                itemPanel.Controls.Add(lblStatus);
                itemPanel.Controls.Add(lblName);
                flow.Controls.Add(itemPanel);
            }

            // Add flow panel to the main panel
            panel1.Controls.Add(flow);













            DA = new SqlDataAdapter("SELECT RoomID ,BuildingID, RoomTypeID ,RoomNumber, RoomCapacity,BuildingName, RoomTypeName FROM Room", Operation.con);
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

        private void dgvRoom_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Make sure it's not a header
            {
                DataGridViewRow row = dgvRoom.Rows[e.RowIndex];
                txtRoomNumber.Tag = row.Cells["RoomID"].Value.ToString();
                txtRoomNumber.Text = row.Cells["RoomNumber"].Value.ToString();
                txtroomCapacity.Text = row.Cells["RoomCapacity"].Value.ToString();
                cbxBuilding.SelectedValue = int.Parse(row.Cells["BuildingID"].Value.ToString());
                cbxRoomType.SelectedValue = int.Parse(row.Cells["RoomTypeID"].Value.ToString());

            }
        }

        private void cbxBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoadBuilding) return;
            MessageBox.Show("Building : "+cbxBuilding.Text.ToString()+" ID : "+cbxBuilding.SelectedValue.ToString());
        }
    }
}
