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
    public partial class updateDeleteClo : Form
    {
        public updateDeleteClo()
        {
            InitializeComponent();
        }

        private void updateDeleteClo_Load(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Clo]", con1);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            cmd1.ExecuteNonQuery();

            txtName.ReadOnly = true;
            txtDatecreated.ReadOnly = true;
            txtDateupdated.ReadOnly = true;
            //txtWeightage.ReadOnly = true;
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
                MessageBox.Show("Clo Found!");
                txtID.ReadOnly = true;
                txtName.ReadOnly = false;
                //txtDatecreated.ReadOnly = false;
                //txtDateupdated.ReadOnly = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;


                DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value != null && r.Cells[0].Value.ToString() == value).First();
                //string name = row.Cells[1].Value.ToString();
                //string address = row.Cells[2].Value.ToString();
                int index = dataGridView1.Rows.IndexOf(row);
                DataGridViewRow dataRow = dataGridView1.Rows[index];
                txtName.Text = dataRow.Cells[1].Value.ToString();
                txtDatecreated.Text = dataRow.Cells[2].Value.ToString();
                txtDateupdated.Text = dataRow.Cells[3].Value.ToString();
                //txtWeightage.Text = dataRow.Cells[4].Value.ToString();


            }
            else
            {
                MessageBox.Show("Clo NOT Found!");
                txtID.ReadOnly = false;
                txtName.ReadOnly = true;
                txtDatecreated.ReadOnly = true;
                txtDateupdated.ReadOnly = true;
                btnUpdate.Enabled = false;
                btnUpdate.Enabled = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id,Name FROM Clo", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader["Name"].ToString() == txtName.Text && reader["Id"].ToString() != txtID.Text)
                {
                    MessageBox.Show("This Name already exists!");
                    reader.Close();
                    return;
                }
                if (txtName.Text == string.Empty)
                {
                    MessageBox.Show("No field shoud be empty!");
                    reader.Close();
                    return;
                }
                else
                {
                    cmd = new SqlCommand("UPDATE Clo SET Name = @Name, DateCreated = @DateCreated, DateUpdated = @DateUpdated WHERE Id = @Id", con);
                    cmd.Parameters.AddWithValue("@Id", txtID.Text);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@DateCreated", DateTime.Parse(txtDatecreated.Text));
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                   // cmd.Parameters.AddWithValue("@TotalWeightage", Int32.Parse(txtWeightage.Text));
                }
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");

            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Clo]", con1);
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
            SqlCommand cmd = new SqlCommand("Delete from Clo Where Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", txtID.Text);
            cmd.Parameters.AddWithValue("@Name", txtName.Text);
            cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
            cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully Deleted");

            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Clo]", con1);
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
