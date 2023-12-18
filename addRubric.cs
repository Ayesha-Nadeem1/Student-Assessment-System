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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace studentAssessmentProject
{
    public partial class addRubric : Form
    {
        public addRubric()
        {
            InitializeComponent();
        }

        private void addRubric_Load(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from [Rubric]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            int count = 0; 
           
            cmd = new SqlCommand("SELECT COUNT(*) FROM [ProjectB].[dbo].[Rubric]", con);
            count = (Int32)cmd.ExecuteScalar();
            count++;
            txtID.Text = count.ToString();

            cmd = new SqlCommand("SELECT Id FROM  Clo", con);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            cbCloID.DataSource = dt;
            cbCloID.ValueMember = "Id";
            dataGridView1.AllowUserToAddRows = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id,Details FROM Rubric", con);
            SqlDataReader reader = cmd.ExecuteReader();
            if (txtID.Text == string.Empty || txtDetails.Text == string.Empty || cbCloID.SelectedIndex == -1)
            {
                MessageBox.Show("No field shoud be empty!");
                reader.Close();
                return;
            }
            while (reader.Read())
            {
                if (reader["Id"].ToString() == txtID.Text || reader["Details"].ToString() == txtDetails.Text)
                {
                    MessageBox.Show("This ID or Details already exists!");
                    reader.Close();
                    return;
                }
                if (txtID.Text == string.Empty || txtDetails.Text == string.Empty || cbCloID.SelectedIndex == -1)
                {
                    MessageBox.Show("No field shoud be empty!");
                    reader.Close();
                    return;
                }
                //else
                //{
                //    SqlCommand cmd2 = new SqlCommand("INSERT INTO Rubric (Id,Details,CloId) VALUES (@Id,@Details,@CloId)", con);
                //    cmd2.Parameters.AddWithValue("@Id", txtID.Text);
                //    cmd2.Parameters.AddWithValue("@Details", txtDetails.Text);
                //    cmd2.Parameters.AddWithValue("@CloId", cbCloID.Text);
                //}
            }
            reader.Close();
            cmd.Connection = con;


            //cmd.CommandText = "SET IDENTITY_INSERT Rubric ON";
            //cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO Rubric (Id,Details,CloId) VALUES (@Id,@Details,@CloId)";
            cmd.Parameters.AddWithValue("@Id", txtID.Text);
            cmd.Parameters.AddWithValue("@Details", txtDetails.Text);
            cmd.Parameters.AddWithValue("@CloId", cbCloID.Text);
            cmd.ExecuteNonQuery();
            //cmd.CommandText = "SET IDENTITY_INSERT Rubric OFF";
            //cmd.ExecuteNonQuery();
            cmd = new SqlCommand("Select * FROM Rubric", con);
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
