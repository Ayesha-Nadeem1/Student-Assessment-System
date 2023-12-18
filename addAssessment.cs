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
    public partial class addAssessment : Form
    {
        public addAssessment()
        {
            InitializeComponent();
        }

        private void addAssessment_Load(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Assessment]", con1);
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
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Assessment", con);
                count = (Int32)cmd.ExecuteScalar();
                con.Close();
            }
            count++;
            txtID.Text = count.ToString();
            //txtID.ReadOnly = true;

            txtDate.Text = DateTime.Now.ToString();
            txtDate.ReadOnly = true; 
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            /*
            var con = Configuration.getInstance().getConnection();
            int status = 0;
            SqlCommand cmd = new SqlCommand("SELECT Title FROM Assessment", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Title"].ToString() == txtTitle.Text)
                {
                    MessageBox.Show("This Title already exists!");
                    reader.Close();
                    return;
                }
                if (txtTitle.Text == string.Empty || txtMarks.Text == string.Empty || txtWeightage.Text == string.Empty)
                {
                    MessageBox.Show("No field shoud be empty!");
                    reader.Close();
                    return;
                }
                else
                {
                    cmd = new SqlCommand("SET IDENTITY_INSERT Assessment ON INSERT INTO [ProjectB].[dbo].[Assessment] (Id,Title, DateCreated, TotalMarks, TotalWeightage) VALUES (@Id,@Title, @DateCreated, @TotalMarks, @TotalWeightage) SET IDENTITY_INSERT Assessment OFF", con);
                    cmd.Parameters.AddWithValue("@Id", Int32.Parse(txtID.Text));
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TotalMarks,", Int32.Parse(txtMarks.Text));
                    cmd.Parameters.AddWithValue("@TotalWeightage", Int32.Parse(txtWeightage.Text));
                }

            }
            reader.Close();

            cmd = new SqlCommand("Select * FROM [Assessment]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");
            */
            
             var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id,Title FROM Assessment", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Title"].ToString() == txtTitle.Text)
                {
                    MessageBox.Show("This Title already exists!");
                    reader.Close();
                    return;
                }
                if (reader["Id"].ToString() == txtID.Text)
                {
                    MessageBox.Show("This ID already exists!");
                    reader.Close();
                    return;
                }
                if (txtTitle.Text == string.Empty || txtMarks.Text == string.Empty || txtWeightage.Text == string.Empty || txtID.Text == string.Empty)
                {
                    MessageBox.Show("No field shoud be empty!");
                    reader.Close();
                    return;
                }
            }
            reader.Close();
            //SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;


            cmd.CommandText = "SET IDENTITY_INSERT Assessment ON";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "Insert into Assessment (Id,Title,DateCreated,TotalMarks,TotalWeightage ) VALUES (@Id, @Title,@DateCreated,@TotalMarks,@TotalWeightage)";
            cmd.Parameters.AddWithValue("@Id", txtID.Text);
            cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
            cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
            cmd.Parameters.AddWithValue("@TotalMarks", Int32.Parse(txtMarks.Text));
            cmd.Parameters.AddWithValue("@TotalWeightage", Int32.Parse(txtWeightage.Text));
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SET IDENTITY_INSERT Assessment OFF";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully saved");
            cmd = new SqlCommand("Select * FROM [Assessment]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();


            /*
             var con = Configuration.getInstance().getConnection();

             SqlCommand cmd = new SqlCommand();
             cmd.Connection = con;


             cmd.CommandText = "SET IDENTITY_INSERT Clo ON";
             cmd.ExecuteNonQuery();

             cmd.CommandText = "Insert into Clo (Id,Name,DateCreated,DateUpdated ) VALUES (@Id, @Name,@DateCreated,@DateUpdated)";
             cmd.Parameters.AddWithValue("@Id", textBox1.Text);
             cmd.Parameters.AddWithValue("@Name", textBox3.Text);
             cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
             cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
             cmd.ExecuteNonQuery();

             cmd.CommandText = "SET IDENTITY_INSERT Clo OFF";
             cmd.ExecuteNonQuery();
             MessageBox.Show("Successfully saved");
             */



        }


    }
}
