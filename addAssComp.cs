using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace studentAssessmentProject
{
    public partial class addAssComp : Form
    {
        DataTable dataTable = new DataTable();
        public addAssComp()
        {
            InitializeComponent();
        }

        private void addAssComp_Load(object sender, EventArgs e)
        {
            
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select * from [AssessmentComponent]", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            int count = 0;

            cmd = new SqlCommand("SELECT COUNT(*) FROM [ProjectB].[dbo].[AssessmentComponent]", con);
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

            cmd = new SqlCommand("SELECT Id FROM  Assessment", con);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            cbAssID.DataSource = dt;
            cbAssID.ValueMember = "Id";
            dataGridView1.AllowUserToAddRows = false;

            txtDateCreated.Text = DateTime.Now.ToString();
            txtDateCreated.ReadOnly = true;

            txtDateUpdated.Text = DateTime.Now.ToString();
            txtDateUpdated.ReadOnly = true;

            btnSave.Enabled = false;
            // btnAdd.Enabled = false; 
            
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("RubricId");
            dataTable.Columns.Add("TotalMarks");
            dataTable.Columns.Add("DateCreated");
            dataTable.Columns.Add("DateUpdated");
            dataTable.Columns.Add("AssessmentId");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbAssID.Enabled = false;
            int value =0;
            //bool allOK = true;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM AssessmentComponent", con);
            //SqlDataReader reader = cmd.ExecuteReader();
            if (txtID.Text == string.Empty || txtName.Text == string.Empty || txtMarks.Text == string.Empty || cbRubricID.SelectedIndex == -1 || cbAssID.SelectedIndex == -1)
            {
                
                MessageBox.Show("No field shoud be empty!");
                //reader.Close();
                return;
            }
            if (!int.TryParse(txtID.Text, out value))
            {
                //allOK = false;
                MessageBox.Show("ID should be an integer!");
                return;
            }
            if (!int.TryParse(txtMarks.Text, out value))
            {
                //allOK = false;
                MessageBox.Show("Marks should be an integer!");
                return;
            }
            if (dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[0].Value != null && r.Cells[0].Value.ToString() == txtID.Text))
            {
                MessageBox.Show("This ID already exists!");
                return;
            }
            if (dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[6].Value != null && r.Cells[6].Value.ToString() == cbAssID.Text) && dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[2].Value != null && r.Cells[2].Value.ToString() == cbRubricID.Text) && dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[1].Value != null && r.Cells[1].Value.ToString() == txtName.Text))
            {
                MessageBox.Show("This Assessment Component already present!");
                
                return;
            }
            if (dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[6].Value != null && r.Cells[6].Value.ToString() == cbAssID.Text) && dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[2].Value != null && r.Cells[2].Value.ToString() == cbRubricID.Text))
            {
                MessageBox.Show("The Assessment Component for selected Rubric is already present in the selected Assessment!");
                return;
            }
            if (dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[6].Value != null && r.Cells[6].Value.ToString() == cbAssID.Text) && dataGridView1.Rows.Cast<DataGridViewRow>().Any(r => r.Cells[1].Value != null && r.Cells[1].Value.ToString() == txtName.Text))
            {
                MessageBox.Show("This Assessment Component already exists for the selected Assessment!");
                return;
            }
            //while (reader.Read())
            //{
            //    if (reader["Id"].ToString() == txtID.Text)
            //    {
            //        MessageBox.Show("This ID already exists!");
            //        reader.Close();
            //        return;
            //    }
            //    if (reader["AssessmentId"].ToString() == cbRubricID.Text && reader["Name"].ToString() == txtName.Text && reader["RubricId"].ToString() == cbRubricID.Text)
            //    {
            //        MessageBox.Show("This Assessment Component already exists coresponding to the selected Rubric!");
            //        reader.Close();
            //        return;
            //    }
            //    if (reader["AssessmentId"].ToString() == cbRubricID.Text && reader["Name"].ToString() == txtName.Text)
            //    {
            //        MessageBox.Show("This Assessment Component already exists for the selected Assessment!");
            //        reader.Close();
            //        return;
            //    }

            //}
            //reader.Close();
            //if (allOK)
            //{                
            DataRow row = dataTable.NewRow();
                row["Id"] = txtID.Text;
                row["Name"] = txtName.Text;
                row["RubricId"] = cbRubricID.Text;
                row["TotalMarks"] = txtMarks.Text;
                row["DateCreated"] = txtDateUpdated.Text;
                row["DateUpdated"] = txtDateUpdated.Text;
                row["AssessmentId"] = cbAssID.Text;
                dataTable.Rows.Add(row);
                dataGridView1.DataSource = dataTable;
            //}
            

            

        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            int sum = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                sum += Convert.ToInt32(row.Cells["TotalMarks"].Value);
            }
            //MessageBox.Show(sum.ToString());

            SqlCommand cmd = new SqlCommand("SELECT TotalMarks FROM Assessment WHERE Id = @Id", con);
            cmd.Parameters.AddWithValue("@Id", cbAssID.Text);
            int totalMarks = (int)cmd.ExecuteScalar();
            if(sum == totalMarks)
            {
                btnSave.Enabled = true;
                MessageBox.Show("equal");
            }
            if (sum != totalMarks)
            {
                MessageBox.Show("not equal");
            }
            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    connection.Open();
            //    string query = "SELECT TotalMarks FROM table_name WHERE id = @id";
            //    SqlCommand command = new SqlCommand(query, connection);
            //    command.Parameters.AddWithValue("@id", id);
            //    int totalMarks = (int)command.ExecuteScalar();
            //    if (sum == totalMarks)
            //    {
            //        //Do something
            //    }
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            string connectionString = @"Data Source=(local);Initial Catalog=ProjectB;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //con.Open();

                // Create a list of SQL parameters
                //List<SqlParameter> sqlParams = new List<SqlParameter>();

                // Insert records one by one
                foreach (DataGridViewRow dgvRow in dataGridView1.Rows)
                {
                    // Create a parameter for each column
                    //sqlParams.Add(new SqlParameter("@Id", dgvRow.Cells["Id"].Value));
                    //sqlParams.Add(new SqlParameter("@Name", dgvRow.Cells["Name"].Value));
                    //sqlParams.Add(new SqlParameter("@RubricId", dgvRow.Cells["RubricId"].Value));
                    //sqlParams.Add(new SqlParameter("@TotalMarks", dgvRow.Cells["TotalMarks"].Value));
                    //sqlParams.Add(new SqlParameter("@DateCreated", dgvRow.Cells["DateCreated"].Value));
                    //sqlParams.Add(new SqlParameter("@DateUpdated", dgvRow.Cells["DateUpdated"].Value));
                    //sqlParams.Add(new SqlParameter("@AssessmentId", dgvRow.Cells["AssessmentId"].Value));

                    // Execute the query
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "SET IDENTITY_INSERT AssessmentComponent ON";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO AssessmentComponent (Id, Name, RubricId, TotalMarks, DateCreated, DateUpdated, AssessmentId) VALUES (@Id, @Name, @RubricId, @TotalMarks, @DateCreated, @DateUpdated, @AssessmentId)";
                        //cmd.Parameters.AddRange(sqlParams.ToArray());
                        cmd.Parameters.AddWithValue("@Id", dgvRow.Cells["Id"].Value);
                        cmd.Parameters.AddWithValue("@Name", dgvRow.Cells["Name"].Value);
                        cmd.Parameters.AddWithValue("@RubricId", dgvRow.Cells["RubricId"].Value);
                        cmd.Parameters.AddWithValue("@TotalMarks", dgvRow.Cells["TotalMarks"].Value);

                        //cmd.Parameters.AddWithValue("@DateCreated", DateTime.ParseExact(dgvRow.Cells["DateCreated"].Value.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        //cmd.Parameters.AddWithValue("@DateUpdated", DateTime.ParseExact(dgvRow.Cells["DateUpdated"].Value.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

                        cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@AssessmentId", dgvRow.Cells["AssessmentId"].Value);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SET IDENTITY_INSERT AssessmentComponent OFF";
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("saved successfully");
                btnSave.Enabled = false;
                //con.Close();
            }
        }
    }
}
