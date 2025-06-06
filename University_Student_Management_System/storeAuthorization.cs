using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using University_Student_Management_System;
using University_Student_Management_System.Dashboard.Building;
using University_Student_Management_System.Dashboard.Professor;
using University_Student_Management_System.Dashboard.RoomType;
using University_Student_Management_System.Dashboard.Semester;
using University_Student_Management_System.Dashboard.Subject;
using University_Student_Management_System.Dashboard.DashboardItem;
using University_Student_Management_System.Dashboard.Room;
using University_Student_Management_System.Dashboard.Level;
using University_Student_Management_System.Dashboard.Department;
using University_Student_Management_System.Dashboard.RoleType;
using University_Student_Management_System.Dashboard.Staff;
using University_Student_Management_System.Dashboard.DocumentType;
using University_Student_Management_System.Dashboard.Document;
using University_Student_Management_System.Dashboard.Class;
using University_Student_Management_System.Dashboard.Enroll;
using University_Student_Management_System.Dashboard.Payment;
using University_Student_Management_System.Dashboard.Schedule;
using University_Student_Management_System.Dashboard.ExamType;
using University_Student_Management_System.Dashboard.Exam;
using University_Student_Management_System.Dashboard.Student;


namespace Project3
{
    internal class storeAuthorization
    {
        public static String name { get; set; }
        public static String role { get; set; }
        public static int id { get; set; }
    



        public static void PermissionNavigate(String navigate, Panel panelContainer)
        {
            panelContainer.Controls.Clear();
            Form childForm = null;
            if ((role.ToLower() == "admin" || role.ToLower() == "dashboard") && navigate == "dashboard")
            {
                childForm = new DashboardItem();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "semester") && navigate == "semester")
            {
                childForm = new Semester();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "building") && navigate == "building")
            {
                childForm = new Building();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "roomtype") && navigate == "roomtype")
            {
                childForm = new RoomType();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "room") && navigate == "room")
            {
                childForm = new Room();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "level") && navigate == "level")
            {
                childForm = new Level();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "department") && navigate == "department")
            {
                childForm = new Department();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "subject") && navigate == "subject")
            {
                childForm = new Subject();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "roletype") && navigate == "roletype")
            {
                childForm = new RoleType();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "staff") && navigate == "staff")
            {
                childForm = new Staff();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "documenttype") && navigate == "documenttype")
            {
                childForm = new DocumentType();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "document") && navigate == "document")
            {
                childForm = new Document();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "class") && navigate == "class")
            {
                childForm = new Class();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "enroll") && navigate == "enroll")
            {
                childForm = new Enroll();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "student") && navigate == "student")
            {
                childForm = new Student();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "enroll professor") && navigate == "enroll professor")
            {
                childForm = new Professor();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "payment") && navigate == "payment")
            {
                childForm = new Payment();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "schedule") && navigate == "schedule")
            {
                childForm = new Schedule();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "examtype") && navigate == "examtype")
            {
                childForm = new ExamType();
            }
            else if ((role.ToLower() == "admin" || role.ToLower() == "exam") && navigate == "exam")
            {
                childForm = new Exam();
            }
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(childForm);
            childForm.Show();
        }
       
        public static void activeMenu(String typeMenu,Panel panelMenu)
        {
            foreach (Control buttonMenu in panelMenu.Controls)
            {
                if (buttonMenu.GetType() == typeof(Button))
                {

                        if (buttonMenu.Tag?.ToString() == typeMenu)
                        {
                            buttonMenu.BackColor = Color.DarkGray;
                        }
                        else
                        {
                            buttonMenu.BackColor = Color.MidnightBlue;
                        }
                    
                  

                }
            }
        }

        public static (string Username, string Role , int id) AuthenticateUser(string username, string password)
        {
            Operation.myConnection();
            string query = "select StaffID, RoleType,StaffNameEN from Staff where StaffPassword =@password and StaffEmail =@username ";
            using (SqlCommand cmd = new SqlCommand(query, Operation.con))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password); 

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string role = reader["RoleType"].ToString();
                        string name = reader["StaffNameEN"].ToString();
                        int id = Convert.ToInt32(reader["StaffID"].ToString());
                        return (name, role,id);
                    }
                    else
                    {
                        return (null, null,0);  
                    }
                }
            }
            
        }
        public static void LockIcon(Form frm, string locks)
        {
            ShowTags(frm.Controls,locks);
        }

        private static void ShowTags(Control.ControlCollection controls,string locks)
        {
            foreach (Control ct in controls)
            {
                if(ct is PictureBox)
                {
                    if (ct.Tag?.ToString().StartsWith("Lock") == true)
                    {

                        if (ct.Tag?.ToString().ToLower() == "lock."+locks.ToLower() || locks.ToLower()=="admin")
                        {
                            ct.Visible = false;
                        }
                        else
                        {
                            ct.Visible = true;
                        }
                    }

                    
                   
                }
                // Recursively check child controls
                if (ct.HasChildren)
                {
                    ShowTags(ct.Controls,locks);
                }
            }
        }

    }
}
