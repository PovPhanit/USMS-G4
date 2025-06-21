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
using University_Student_Management_System.Dashboard.DashboardItem.ScoreExamType;
namespace University_Student_Management_System.Dashboard.DashboardItem
{
    public partial class DashboardItem : Form
    {
        public Panel panelsContainer;
        public DashboardItem()
        {
            InitializeComponent();
            
        }
    

 
        private void btnScoreExamType_Click_1(object sender, EventArgs e)
        {

            storeAuthorization.PermissionNavigate("ScoreExamType", panelContainerScore);
        }

        private void btnScoreSubject_Click(object sender, EventArgs e)
        {
            storeAuthorization.PermissionNavigate("ScoreSubject", panelContainerScore);
        }

        private void btnScoreAllSubject_Click(object sender, EventArgs e)
        {
            storeAuthorization.PermissionNavigate("ScoreAllSubject", panelContainerScore);
        }

        private void btnScheduleStudy_Click(object sender, EventArgs e)
        {
            storeAuthorization.PermissionNavigate("ScheduleStudy", panelContainerScore);
        }

        private void btnListStudent_Click(object sender, EventArgs e)
        {
            storeAuthorization.PermissionNavigate("ListStudent", panelContainerScore);
        }

        private void btnSubjectInDepartment_Click(object sender, EventArgs e)
        {
            storeAuthorization.PermissionNavigate("SubjectInDepartment", panelContainerScore);
        }
    }
}
