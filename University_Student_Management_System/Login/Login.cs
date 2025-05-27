﻿using Project3;
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
using University_Student_Management_System.Dashboard;

namespace University_Student_Management_System.Login
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            var (staffname, role) = storeAuthorization.AuthenticateUser(username, password);
            if (role != null)
            {
                this.Hide();
                storeAuthorization.name = staffname;
                storeAuthorization.role = role;
                Dashboard.Dashboard dashboard = new Dashboard.Dashboard();
                dashboard.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
