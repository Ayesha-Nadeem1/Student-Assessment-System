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
    public partial class addCLO : Form
    {
        public addCLO()
        {
            InitializeComponent();
        }

        private void addCLO_Load(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Clo]", con1);
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
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Clo", con);
                count = (Int32)cmd.ExecuteScalar();
                con.Close();
            }
            count++;
            txtID.Text = count.ToString();
            //txtID.ReadOnly = true;

            txtDatecreated.Text = DateTime.Now.ToString();
            txtDatecreated.ReadOnly = true;

            txtDateupdated.Text = DateTime.MinValue.ToString();
            txtDateupdated.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id,Name FROM Clo", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Name"].ToString() == txtName.Text || reader["Id"].ToString() == txtID.Text)
                {
                    MessageBox.Show("This Name or ID already exists!");
                    reader.Close();
                    return;
                }
                //if (reader["Id"].ToString() == txtID.Text)
                //{
                //    MessageBox.Show("This ID already exists!");
                //    reader.Close();
                //    return;
                //}
                if (txtName.Text == string.Empty )
                {
                    MessageBox.Show("No field shoud be empty!");
                    reader.Close();
                    return;
                }
            }
            reader.Close();
            //SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;


            cmd.CommandText = "SET IDENTITY_INSERT Clo ON";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "Insert into Clo (Id,Name,DateCreated,DateUpdated) VALUES (@Id,@Name,@DateCreated,@DateUpdated)";
            cmd.Parameters.AddWithValue("@Id", txtID.Text);
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
            cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SET IDENTITY_INSERT Clo OFF";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");
            cmd = new SqlCommand("Select * FROM [Clo]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();
        }
    }
}
