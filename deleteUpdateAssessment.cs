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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace studentAssessmentProject
{
    public partial class deleteUpdateAssessment : Form
    {
        public deleteUpdateAssessment()
        {
            InitializeComponent();
        }

        private void deleteUpdateAssessment_Load(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Assessment]", con1);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            cmd1.ExecuteNonQuery();

            txtDate.ReadOnly = true;
            txtMarks.ReadOnly = true;
            txtTitle.ReadOnly = true;
            txtWeightage.ReadOnly = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            string value = txtID.Text;
            //if (dataGridView1.Rows.Contains(value))
            // {

            // }
            //if (dataGridView1.Rows.Cast<DataGridViewRow>().Any(row => row.Cells[0].Value.ToString() == value))
            //{
            //    MessageBox.Show("Student Found!");
            //}
            if (dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[0].Value != null && r.Cells[0].Value.ToString() == value))
            {
                MessageBox.Show("Assessment Found!");
                txtID.ReadOnly = true;
                txtDate.ReadOnly = false;
                txtMarks.ReadOnly = false;
                txtTitle.ReadOnly = false;
                txtWeightage.ReadOnly = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                

                DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value != null && r.Cells[0].Value.ToString() == value).First();
                //string name = row.Cells[1].Value.ToString();
                //string address = row.Cells[2].Value.ToString();
                int index = dataGridView1.Rows.IndexOf(row);
                DataGridViewRow dataRow = dataGridView1.Rows[index];
                txtTitle.Text = dataRow.Cells[1].Value.ToString();
                txtDate.Text = dataRow.Cells[2].Value.ToString();
                txtMarks.Text = dataRow.Cells[3].Value.ToString();
                txtWeightage.Text = dataRow.Cells[4].Value.ToString();

                
            }
            else
            {
                MessageBox.Show("Assessment NOT Found!");
                txtID.ReadOnly = false;
                txtDate.ReadOnly = true;
                txtMarks.ReadOnly = true;
                txtTitle.ReadOnly = true;
                txtWeightage.ReadOnly = true;
                btnUpdate.Enabled = false;
                btnUpdate.Enabled = false;
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id,Title FROM Assessment", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Title"].ToString() == txtTitle.Text && reader["Id"].ToString() != txtID.Text)
                {
                    MessageBox.Show("This title already exists!");
                    reader.Close();
                    return;
                }
                if (txtTitle.Text == string.Empty || txtDate.Text == string.Empty || txtMarks.Text == string.Empty || txtWeightage.Text == string.Empty)
                {
                    MessageBox.Show("No field shoud be empty!");
                    reader.Close();
                    return;
                }
                else
                {
                    cmd = new SqlCommand("UPDATE Assessment SET Title = @Title, DateCreated = @DateCreated, TotalMarks = @TotalMarks, TotalWeightage = @TotalWeightage WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", txtID.Text);
                    cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TotalMarks", Int32.Parse(txtMarks.Text));
                    cmd.Parameters.AddWithValue("@TotalWeightage", Int32.Parse(txtWeightage.Text));
                }
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");

            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Assessment]", con1);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            cmd1.ExecuteNonQuery();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Delete from Assessment Where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", txtID.Text);
            cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
            cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
            cmd.Parameters.AddWithValue("@TotalMarks", Int32.Parse(txtMarks.Text));
            cmd.Parameters.AddWithValue("@TotalWeightage", Int32.Parse(txtWeightage.Text));
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Deleted");

            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Assessment]", con1);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            cmd1.ExecuteNonQuery();

            btnUpdate.Enabled = false;
        }
    }
}
