using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace University_Student_Management_System.Dashboard.Staff
{
    public partial class Staff : Form
    {
        public Staff()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.ShowDialog();
     
            imageStaff.Image = Image.FromFile(fd.FileName);
        }

<<<<<<< HEAD
        private void Staff_Load(object sender, EventArgs e)
        {

        }
=======
>>>>>>> 2320f900918009fb7881e86b09bcb608f312b7df
    }
}
