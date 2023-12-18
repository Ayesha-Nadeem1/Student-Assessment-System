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

namespace studentAssessmentProject
{
    public partial class classAttendance : Form
    {
        public classAttendance()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void classAttendance_Load(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [ClassAttendance]", con1);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);

            int count = 0;
            String ConnectionStr = @"Data Source=(local);Initial Catalog=ProjectB;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(ConnectionStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM ClassAttendance", con);
                count = (Int32)cmd.ExecuteScalar();
                con.Close();
            }
            count++;
            txtID.Text = count.ToString();
            txtID.ReadOnly = true;

            txtAttendanceDate.Text = DateTime.Now.ToString();
            txtAttendanceDate.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id,AttendanceDate FROM ClassAttendance", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Id"].ToString() == txtID.Text || reader["AttendanceDate"].ToString() == txtAttendanceDate.Text)
                {
                    MessageBox.Show("This Id already exists!");
                    txtID.ReadOnly = false;
                    reader.Close();
                    return;
                }

                //    if (txtAttendanceDate.Text == string.Empty)
                //    {
                //        MessageBox.Show("No field shoud be empty!");
                //        reader.Close();
                //        return;
                //    }
            }
        reader.Close();
            cmd.Connection = con;


            cmd.CommandText = "SET IDENTITY_INSERT ClassAttendance ON";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "Insert into ClassAttendance (Id,AttendanceDate) VALUES (@Id, @AttendanceDate)";
            cmd.Parameters.AddWithValue("@Id", Int32.Parse(txtID.Text));
            cmd.Parameters.AddWithValue("@AttendanceDate", DateTime.Parse(txtAttendanceDate.Text));
           
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SET IDENTITY_INSERT ClassAttendance OFF";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");
            cmd = new SqlCommand("Select * FROM [ClassAttendance]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();
        }
    }
}
