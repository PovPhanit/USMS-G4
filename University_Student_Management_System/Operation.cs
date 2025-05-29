using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University_Student_Management_System
{
    internal class Operation
    {
        public static SqlConnection con;
        public static void myConnection()
        {
            string conStr = @"Data source=DESKTOP-L6LP4VH\MSSQLSERVER2;Initial catalog=University_Management_System;Integrated Security=true";
            con = new SqlConnection(conStr);
            con.Open();
        }
    }
         
 }
