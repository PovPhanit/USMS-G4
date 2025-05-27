using Project3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using University_Student_Management_System.Login;

namespace University_Student_Management_System.Dashboard
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

 

        private void Dashboard_Load(object sender, EventArgs e)
        {
            Welcome.Text = "Welcome, " + storeAuthorization.name;
            storeAuthorization.LockIcon(this, storeAuthorization.role);
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "dashboard")
            {
                storeAuthorization.activeMenu("dashboard", panelMenus);
                storeAuthorization.PermissionNavigate("dashboard", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "semester")
            {
                storeAuthorization.activeMenu("semester", panelMenus);
                storeAuthorization.PermissionNavigate("semester", panelContainerForm);
            }
           
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "building")
            {
                storeAuthorization.activeMenu("building", panelMenus);
                storeAuthorization.PermissionNavigate("building", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "roomtype")
            {
                storeAuthorization.activeMenu("roomtype", panelMenus);
                storeAuthorization.PermissionNavigate("roomtype", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "room")
            {
                storeAuthorization.activeMenu("room", panelMenus);
                storeAuthorization.PermissionNavigate("room", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "level")
            {
                storeAuthorization.activeMenu("level", panelMenus);
                storeAuthorization.PermissionNavigate("level", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "department")
            {
                storeAuthorization.activeMenu("department", panelMenus);
                storeAuthorization.PermissionNavigate("department", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "subject")
            {
                storeAuthorization.activeMenu("subject", panelMenus);
                storeAuthorization.PermissionNavigate("subject", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "roletype")
            {
                storeAuthorization.activeMenu("roletype", panelMenus);
                storeAuthorization.PermissionNavigate("roletype", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "staff")
            {
                storeAuthorization.activeMenu("staff", panelMenus);
                storeAuthorization.PermissionNavigate("staff", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "documenttype")
            {
                storeAuthorization.activeMenu("documenttype", panelMenus);
                storeAuthorization.PermissionNavigate("documenttype", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "document")
            {
                storeAuthorization.activeMenu("document", panelMenus);
                storeAuthorization.PermissionNavigate("document", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "class")
            {
                storeAuthorization.activeMenu("class", panelMenus);
                storeAuthorization.PermissionNavigate("class", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "enroll")
            {
                storeAuthorization.activeMenu("enroll", panelMenus);
                storeAuthorization.PermissionNavigate("enroll", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "enrollup")
            {
                storeAuthorization.activeMenu("enrollup", panelMenus);
                storeAuthorization.PermissionNavigate("enrollup", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "enroll professor")
            {
                storeAuthorization.activeMenu("enroll professor", panelMenus);
                storeAuthorization.PermissionNavigate("enroll professor", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "payment")
            {
                storeAuthorization.activeMenu("payment", panelMenus);
                storeAuthorization.PermissionNavigate("payment", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "schedule")
            {
                storeAuthorization.activeMenu("schedule", panelMenus);
                storeAuthorization.PermissionNavigate("schedule", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "examtype")
            {
                storeAuthorization.activeMenu("examtype", panelMenus);
                storeAuthorization.PermissionNavigate("examtype", panelContainerForm);
            }
            else if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "exam")
            {
                storeAuthorization.activeMenu("exam", panelMenus);
                storeAuthorization.PermissionNavigate("exam", panelContainerForm);
            }
        }
     

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult re = new DialogResult();
            re = MessageBox.Show("Do you want to logout account ?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (re == DialogResult.Yes)
            {
                this.Close();
                Login.Login Logout = new Login.Login();
                Logout.Show();
                storeAuthorization.name = "";
                storeAuthorization.role = "";
            }
        }


 



        private void btnProfessor_Click_1(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "enroll professor")
            {
                storeAuthorization.activeMenu("enroll professor", panelMenus);
                storeAuthorization.PermissionNavigate("enroll professor", panelContainerForm);
            }
        }

        private void btnSubject_Click_1(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "subject")
            {
                storeAuthorization.activeMenu("subject", panelMenus);
                storeAuthorization.PermissionNavigate("subject", panelContainerForm);
            }
        }

        private void btnSemester_Click_1(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "semester")
            {
                storeAuthorization.activeMenu("semester", panelMenus);
                storeAuthorization.PermissionNavigate("semester", panelContainerForm);
            }
        }

        private void btnBuilding_Click_1(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "building")
            {
                storeAuthorization.activeMenu("building", panelMenus);
                storeAuthorization.PermissionNavigate("building", panelContainerForm);
            }
        }

        private void btnRoomType_Click_1(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "roomtype")
            {
                storeAuthorization.activeMenu("roomtype", panelMenus);
                storeAuthorization.PermissionNavigate("roomtype", panelContainerForm);
            }
        }

 

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "dashboard")
            {
                storeAuthorization.activeMenu("dashboard", panelMenus);
                storeAuthorization.PermissionNavigate("dashboard", panelContainerForm);
            }
        }

        private void btnRoom_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "room")
            {
                storeAuthorization.activeMenu("room", panelMenus);
                storeAuthorization.PermissionNavigate("room", panelContainerForm);
            }
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "level")
            {
                storeAuthorization.activeMenu("level", panelMenus);
                storeAuthorization.PermissionNavigate("level", panelContainerForm);
            }
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "department")
            {
                storeAuthorization.activeMenu("department", panelMenus);
                storeAuthorization.PermissionNavigate("department", panelContainerForm);
            }
        }

        private void btnRoleType_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "roletype")
            {
                storeAuthorization.activeMenu("roletype", panelMenus);
                storeAuthorization.PermissionNavigate("roletype", panelContainerForm);
            }
        }

        private void btnDocumentType_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "documenttype")
            {
                storeAuthorization.activeMenu("documenttype", panelMenus);
                storeAuthorization.PermissionNavigate("documenttype", panelContainerForm);
            }
        }

        private void btnDocument_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "document")
            {
                storeAuthorization.activeMenu("document", panelMenus);
                storeAuthorization.PermissionNavigate("document", panelContainerForm);
            }
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "staff")
            {
                storeAuthorization.activeMenu("staff", panelMenus);
                storeAuthorization.PermissionNavigate("staff", panelContainerForm);
            }
        }

        private void btnClass_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "class")
            {
                storeAuthorization.activeMenu("class", panelMenus);
                storeAuthorization.PermissionNavigate("class", panelContainerForm);
            }
        }

        private void btnEnroll_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "enroll")
            {
                storeAuthorization.activeMenu("enroll", panelMenus);
                storeAuthorization.PermissionNavigate("enroll", panelContainerForm);
            }
        }

        private void btnEnrollUp_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "enrollup")
            {
                storeAuthorization.activeMenu("enrollup", panelMenus);
                storeAuthorization.PermissionNavigate("enrollup", panelContainerForm);
            }
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "payment")
            {
                storeAuthorization.activeMenu("payment", panelMenus);
                storeAuthorization.PermissionNavigate("payment", panelContainerForm);
            }
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "schedule")
            {
                storeAuthorization.activeMenu("schedule", panelMenus);
                storeAuthorization.PermissionNavigate("schedule", panelContainerForm);
            }
        }

        private void btnExamType_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "examtype")
            {
                storeAuthorization.activeMenu("examtype", panelMenus);
                storeAuthorization.PermissionNavigate("examtype", panelContainerForm);
            }
        }

        private void btnExam_Click(object sender, EventArgs e)
        {
            if (storeAuthorization.role.ToLower() == "admin" || storeAuthorization.role.ToLower() == "exam")
            {
                storeAuthorization.activeMenu("exam", panelMenus);
                storeAuthorization.PermissionNavigate("exam", panelContainerForm);
            }
        }
    }
}
