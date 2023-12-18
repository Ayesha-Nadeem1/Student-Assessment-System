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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace studentAssessmentProject
{
    public partial class addStudent : Form
    {
        public addStudent()
        {
            InitializeComponent();
        }

        private void addStudent_Load(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Student]", con1);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            // cmd1.ExecuteNonQuery();


            label4.Dock = DockStyle.Fill;
            label4.TextAlign = ContentAlignment.MiddleCenter;

            label5.Dock = DockStyle.Fill;
            label5.TextAlign = ContentAlignment.MiddleCenter;

            label6.Dock = DockStyle.Fill;
            label6.TextAlign = ContentAlignment.MiddleCenter;

            label7.Dock = DockStyle.Fill;
            label7.TextAlign = ContentAlignment.MiddleCenter;

            label8.Dock = DockStyle.Fill;
            label8.TextAlign = ContentAlignment.MiddleCenter;


            int count = 0;
            String ConnectionStr = @"Data Source=(local);Initial Catalog=ProjectB;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(ConnectionStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [ProjectB].[dbo].[Student]", con);
                count = (Int32)cmd.ExecuteScalar();
                con.Close();
            }
            count++;
            txtID.Text = count.ToString();
            txtID.ReadOnly = true;

            

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            var con = Configuration.getInstance().getConnection();
            int status = 0;
            SqlCommand cmd = new SqlCommand("SELECT Contact, Email, RegistrationNumber FROM Student", con);
            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                if (reader["Contact"].ToString() == txtContact.Text || reader["Email"].ToString() == txtEmail.Text || reader["RegistrationNumber"].ToString() == txtRegNo.Text )
                {
                    MessageBox.Show("This contact, email or registration number already exists!");
                    reader.Close();
                    return;
                }
                if (txtContact.Text== string.Empty || txtEmail.Text== string.Empty || txtRegNo.Text== string.Empty || txtfname.Text==string.Empty || txtlname.Text == string.Empty || cbStatus.Text==string.Empty || txtContact.Text==string.Empty)
                {
                    MessageBox.Show("No field shoud be empty!");
                    reader.Close();
                    return;
                }
                else
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                    //cmd.CommandText = "SET IDENTITY_INSERT Student ON";
                    //cmd.ExecuteNonQuery();
                    //cmd.Dispose();
                    cmd = new SqlCommand("INSERT INTO Student (FirstName, LastName, Contact, Email, RegistrationNumber, Status) VALUES (@FirstName, @LastName, @Contact, @Email, @RegistrationNumber, @Status)", con);
                    cmd.Parameters.AddWithValue("@FirstName", txtfname.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtlname.Text);
                    cmd.Parameters.AddWithValue("@Contact", txtContact.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", txtRegNo.Text);

                    if (cbStatus.Text == "Active")
                    {
                        status = 5;
                    }
                    else
                    {
                        status = 6;
                    }
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.ExecuteNonQuery();
                    //cmd.CommandText = "SET IDENTITY_INSERT Student OFF";
                    //cmd.ExecuteNonQuery();
                    
                }
                reader = cmd.ExecuteReader();
            }
            reader.Close();
            cmd = new SqlCommand("Select * FROM [Student]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");
        }
    }
}
