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

namespace University_Student_Management_System.Dashboard.Professor
{
    public partial class Professor : Form
    {
        public Professor()
        {
            InitializeComponent();
        }
        SqlDataAdapter DA;
        DataTable TB;
        SqlCommand com;
        byte[] Photo;
        string filepath;
        bool isCreateUPdate = false;
        private void professorImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "JPEG FILE|*.jpg; *.jpeg |PNG FILE|*.png";
            fd.Title = "Open an image...";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                filepath = fd.FileName;
                imageProfessor.Image = Image.FromFile(filepath);
            }
        }
        private void loadData()
        {
            DA = new SqlDataAdapter("select * from viewProfessor order by ProfessorID desc", Operation.con);
            TB = new DataTable();
            DA.Fill(TB);
            dgvProfessor.DataSource = TB;
            dgvProfessor.ColumnHeadersHeight = 40;
            dgvProfessor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvProfessor.RowTemplate.Height = 100;
            foreach (DataGridViewRow row in dgvProfessor.Rows)
            {
                row.Height = 100;
            }
            dgvProfessor.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 14, FontStyle.Bold);
            dgvProfessor.DefaultCellStyle.Font = new Font("Khmer os system", 12);
            dgvProfessor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProfessor.Columns["ProfessorID"].Visible = false;
            dgvProfessor.Columns["Name KH"].Width = 100;
            dgvProfessor.Columns["Name EN"].Width = 100;
            DataGridViewImageColumn img = new DataGridViewImageColumn();
            img = (DataGridViewImageColumn)dgvProfessor.Columns["Image"];
            img.ImageLayout = DataGridViewImageCellLayout.Stretch;

            foreach (DataGridViewColumn col in dgvProfessor.Columns)
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
        private void Professor_Load(object sender, EventArgs e)
        {
            fieldGender();
            txtNameKH.Focus();
            txtSearch.Text = "Search professor hear...";
            txtSearch.ForeColor = Color.Gray;
            loadData();

        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search professor hear...")
            {
                txtSearch.ForeColor = Color.Black;
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                txtSearch.Text = "Search professor hear...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            DA = new SqlDataAdapter();
            DA.SelectCommand = new SqlCommand("SearchpProfessorByNameOrIDCard", Operation.con);
            DA.SelectCommand.CommandType = CommandType.StoredProcedure;
            DA.SelectCommand.Parameters.AddWithValue("@Keyword", txtSearch.Text.Trim());
            TB = new DataTable();
            DA.Fill(TB);
            dgvProfessor.DataSource = TB;
        }

        private void dgvProfessor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isCreateUPdate)
            {
                if (e.RowIndex >= 0) // Make sure it's not a header
                {
                    DataGridViewRow row = dgvProfessor.Rows[e.RowIndex];
                    txtNameKH.Tag = row.Cells["ProfessorID"].Value.ToString();
                    txtNameKH.Text = row.Cells["Name KH"].Value.ToString();
                    txtNameEN.Text = row.Cells["Name EN"].Value.ToString();
                    if (row.Cells["Gender"].Value.ToString() == "Male")
                    {
                        cbxGender.SelectedIndex = 0;
                    }
                    else
                    {
                        cbxGender.SelectedIndex = 1;
                    }
                    txtPhnoe.Text = row.Cells["Phone"].Value.ToString();
                    dpDOB.Text = row.Cells["DOB"].Value.ToString();
                    txtVillage.Text = row.Cells["Village"].Value.ToString();
                    txtSongkat.Text = row.Cells["Sangkat_Khum"].Value.ToString();
                    txtKhan.Text = row.Cells["Khan_Srok"].Value.ToString();
                    txtCity.Text = row.Cells["Province_City"].Value.ToString();

                    Photo = (byte[])row.Cells["Image"].Value;
                    MemoryStream ms = new MemoryStream(Photo);
                    imageProfessor.Image = Image.FromStream(ms);

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
                ControlForm.ClearData(this);
                txtNameKH.Text = "";
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

                if (isCreateUPdate)
                {
                    SqlCommand com = new SqlCommand("CreateProfessor", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ProfessorNameKH", txtNameKH.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorNameEN", txtNameEN.Text.ToString());
                    String Gender;
                    if (cbxGender.SelectedIndex == 0)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }
                    com.Parameters.AddWithValue("@ProfessorGender", Gender);
                    com.Parameters.AddWithValue("@ProfessorPhoneNumber", txtPhnoe.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorDOB", dpDOB.Value);
                    com.Parameters.AddWithValue("@ProfessorVillage", txtVillage.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorSangkatKhum", txtSongkat.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorKhanSrok", txtKhan.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorProvinceCity", txtCity.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorStoppedWork", 0);
                    if (filepath != null)
                    {
                        Photo = File.ReadAllBytes(filepath);   //using system io    
                    }
                    else
                    {
                        Photo = File.ReadAllBytes("../../Resources/bookDisplay.png");
                    }
                    com.Parameters.AddWithValue("@ProfessorImage", Photo);
                    filepath = null;
                    ControlForm.ClearData(this);
                    txtNameKH.Focus();
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();
                }
                else
                {
                    SqlCommand com = new SqlCommand("UpdateProfessor", Operation.con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@ProfessorID", int.Parse(txtNameKH.Tag.ToString()));
                    com.Parameters.AddWithValue("@ProfessorNameKH", txtNameKH.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorNameEN", txtNameEN.Text.ToString());
                    String Gender;
                    if (cbxGender.SelectedIndex == 0)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }
                    com.Parameters.AddWithValue("@ProfessorGender", Gender);
                    com.Parameters.AddWithValue("@ProfessorPhoneNumber", txtPhnoe.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorDOB", dpDOB.Value);
                    com.Parameters.AddWithValue("@ProfessorVillage", txtVillage.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorSangkatKhum", txtSongkat.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorKhanSrok", txtKhan.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorProvinceCity", txtCity.Text.ToString());
                    com.Parameters.AddWithValue("@ProfessorStoppedWork", 0);
                    if (filepath != null)
                    {
                        Photo = File.ReadAllBytes(filepath);   //using system io    
                    }
                    else
                    {
                        Photo = File.ReadAllBytes("../../Resources/bookDisplay.png");
                    }
                    com.Parameters.AddWithValue("@ProfessorImage", Photo);
                    filepath = null;
                    int rowEffect = com.ExecuteNonQuery();
                    loadData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int stop;
            if (CheckboxStopWork.Checked.ToString() == "True")
            {
                stop = 1;
            }
            else
            {
                stop = 0;
            }
            if (string.IsNullOrEmpty(txtNameKH.Tag.ToString().Trim()))
            {
                MessageBox.Show("Please select list for delete", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to delete it ?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (re == DialogResult.OK)
            {
                SqlCommand com = new SqlCommand("StopProfessorWorkByID", Operation.con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProfessorID", int.Parse(txtNameKH.Tag.ToString()));
                com.Parameters.AddWithValue("@StopWork", stop);
                int rowEffect = com.ExecuteNonQuery();
                CheckboxStopWork.Checked = false;
                ControlForm.ClearData(this);
                txtNameKH.Tag = "";
                loadData();
            }
        }

        private void txtNameKH_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtCity, txtNameEN);
        }

        private void txtNameEN_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtNameKH, cbxGender);
        }

        private void cbxGender_KeyUp(object sender, KeyEventArgs e)
        {
            ControlForm.KeyControl(this, sender, e, txtNameEN, txtPhnoe);
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
            ControlForm.KeyControl(this, sender, e, txtKhan, txtNameKH);
        }
    }
}
