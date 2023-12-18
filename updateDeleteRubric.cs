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
    public partial class updateDeleteRubric : Form
    {
        public updateDeleteRubric()
        {
            InitializeComponent();
        }

        private void updateDeleteRubric_Load(object sender, EventArgs e)
        {
            
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from [Rubric]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);

            cmd = new SqlCommand("SELECT Id FROM  Clo", con);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            cbCloID.DataSource = dt;
            cbCloID.ValueMember = "Id";
            dataGridView1.AllowUserToAddRows = false;

            //cbCloID.Enabled= false;
            txtDetails.ReadOnly = true;
            //txtCloID.ReadOnly = true;
            //txtID.ReadOnly = true;
            btnDelete.Enabled= false;
            btnUpdate.Enabled= false;

        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            string value = txtID.Text;
            if (dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[0].Value != null && r.Cells[0].Value.ToString() == value))
            {
                MessageBox.Show("Rubric Found!");
                txtID.ReadOnly = true;
                txtDetails.ReadOnly = false;
                
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;


                DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value != null && r.Cells[0].Value.ToString() == value).First();
                //string name = row.Cells[1].Value.ToString();
                //string address = row.Cells[2].Value.ToString();
                int index = dataGridView1.Rows.IndexOf(row);
                DataGridViewRow dataRow = dataGridView1.Rows[index];
                txtDetails.Text = dataRow.Cells[1].Value.ToString();
                //txtCloID.Text = dataRow.Cells[2].Value.ToString();
                
            }
            else
            {
                MessageBox.Show("Rubric NOT Found!");
                txtID.ReadOnly = false;
                txtDetails.ReadOnly = true;
                btnUpdate.Enabled = false;
                btnUpdate.Enabled = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Details FROM Rubric", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Details"].ToString() == txtDetails.Text && reader["Id"].ToString() != txtID.Text)
                {
                    MessageBox.Show("This Detail already exists!");
                    reader.Close();
                    return;
                }
                if (txtDetails.Text == string.Empty || cbCloID.SelectedIndex == -1)
                {
                    MessageBox.Show("No field shoud be empty!");
                    reader.Close();
                    return;
                }
                else
                {
                    cmd = new SqlCommand("UPDATE Rubric SET Details = @Details, CloId = CloId WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", txtID.Text);
                    cmd.Parameters.AddWithValue("@Details", txtDetails.Text);
                    cmd.Parameters.AddWithValue("@CloId", cbCloID.Text);
                }
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");

            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Rubric]", con1);
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
            SqlCommand cmd = new SqlCommand("Delete from Rubric Where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", txtID.Text);
            cmd.Parameters.AddWithValue("@Details", txtDetails.Text);
            cmd.Parameters.AddWithValue("@CloId", cbCloID.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Deleted");

            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Rubric]", con1);
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
