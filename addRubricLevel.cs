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
    public partial class addRubricLevel : Form
    {
        public addRubricLevel()
        {
            InitializeComponent();
        }

        private void addRubricLevel_Load(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from [RubricLevel]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            int count = 0;

            cmd = new SqlCommand("SELECT COUNT(*) FROM [ProjectB].[dbo].[RubricLevel]", con);
            count = (Int32)cmd.ExecuteScalar();
            count++;
            txtID.Text = count.ToString();

            cmd = new SqlCommand("SELECT Id FROM  Rubric", con);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            cbRubricID.DataSource = dt;
            cbRubricID.ValueMember = "Id";
            dataGridView1.AllowUserToAddRows = false;
            txtDetails.ReadOnly= true;
        }

        private void cbML_SelectedValueChanged(object sender, EventArgs e)
        {
            int value = int.Parse(cbML.Text);
            if (value == 1)
            {
                txtDetails.Text = "Unsatisfactory";
            }
            else if (value == 2)
            {
                txtDetails.Text = "Fair";
            }
            else if (value == 3)
            {
                txtDetails.Text = "Good";
            }
            else if (value == 4)
            {
                txtDetails.Text = "Exceptional";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM RubricLevel", con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (txtID.Text == string.Empty || txtDetails.Text == string.Empty || cbRubricID.SelectedIndex == -1 || cbML.SelectedIndex == -1)
            {
                MessageBox.Show("No field shoud be empty!");
                reader.Close();
                return;
            }
            while (reader.Read())
            {
                if (reader["Id"].ToString() == txtID.Text)
                {
                    MessageBox.Show("This ID already exists!");
                    reader.Close();
                    return;
                }
                if (reader["RubricId"].ToString() == cbRubricID.Text && reader["MeasurementLevel"].ToString() == cbML.Text)
                {
                    MessageBox.Show("This MeasurementLevel already exists for selected Rubric ID!");
                    reader.Close();
                    return;
                }
            }
            reader.Close();
            cmd.Connection = con;


            cmd.CommandText = "SET IDENTITY_INSERT RubricLevel ON";
            cmd.ExecuteNonQuery();
            //cmd.Connection = con;
            //cmd.CommandText = "INSERT INTO RubricLevel (Id,RubricId,Details,MeasurementLevel) VALUES (Id,RubricId,Details,MeasurementLevel)";

            cmd = new SqlCommand("INSERT INTO RubricLevel (Id,RubricId,Details,MeasurementLevel) VALUES (@Id,@RubricId,@Details,@MeasurementLevel)", con);
            cmd.Parameters.AddWithValue("@Id", txtID.Text);
            cmd.Parameters.AddWithValue("@RubricId", cbRubricID.Text);
            cmd.Parameters.AddWithValue("@Details", txtDetails.Text);
            cmd.Parameters.AddWithValue("@MeasurementLevel", cbML.Text);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SET IDENTITY_INSERT RubricLevel OFF";
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("Select * FROM RubricLevel", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            cmd.ExecuteNonQuery();

        }
    }
}
