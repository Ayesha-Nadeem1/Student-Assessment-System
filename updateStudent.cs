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
    public partial class updateStudent : Form
    {
        public updateStudent()
        {
            InitializeComponent();
        }

        private void updateStudent_Load(object sender, EventArgs e)
        {
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Student]", con1);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            cmd1.ExecuteNonQuery();

            txtfname.ReadOnly = true;
            txtlname.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtContact.ReadOnly = true;
            txtRegNo.ReadOnly = true;
            cbStatus.Enabled = false;
            btnAdd.Enabled = false;
        }

        private void checkButton_Click(object sender, EventArgs e)
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
                MessageBox.Show("Student Found!");
                txtID.ReadOnly = true;
                txtfname.ReadOnly = false;
                txtlname.ReadOnly = false;
                txtEmail.ReadOnly = false;
                txtContact.ReadOnly = false;
                txtRegNo.ReadOnly = false;
                cbStatus.Enabled = true;
                btnAdd.Enabled = true;

                DataGridViewRow row = dataGridView1.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value != null && r.Cells[0].Value.ToString() == value).First();
                //string name = row.Cells[1].Value.ToString();
                //string address = row.Cells[2].Value.ToString();
                int index = dataGridView1.Rows.IndexOf(row);
                DataGridViewRow dataRow = dataGridView1.Rows[index];
                txtfname.Text = dataRow.Cells[1].Value.ToString();
                txtlname.Text = dataRow.Cells[2].Value.ToString();
                txtContact.Text = dataRow.Cells[3].Value.ToString(); 
                txtEmail.Text = dataRow.Cells[4].Value.ToString(); 
                txtRegNo.Text = dataRow.Cells[5].Value.ToString();

                int status = Int32.Parse(dataRow.Cells[6].Value.ToString());
                if (status ==5)
                {
                    cbStatus.Text = "Active";
                }
                else
                {
                    cbStatus.Text = "Inactive";
                }

            }
            else 
            {
                MessageBox.Show("Student NOT Found!");
                txtID.ReadOnly= false;
                txtfname.ReadOnly=true;
                txtlname.ReadOnly=true;
                txtEmail.ReadOnly=true;
                txtContact.ReadOnly=true;
                txtRegNo.ReadOnly=true;
                cbStatus.Enabled=false;
                btnAdd.Enabled=false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int status = 0;
            var con = Configuration.getInstance().getConnection();
            int value = Int32.Parse(txtID.Text);
            SqlCommand cmd = new SqlCommand("SELECT Id,Contact, Email, RegistrationNumber FROM Student", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if ((reader["Contact"].ToString() == txtContact.Text || reader["Email"].ToString() == txtEmail.Text || reader["RegistrationNumber"].ToString() == txtRegNo.Text) && reader["Id"].ToString() != txtID.Text)
                {
                    MessageBox.Show("This contact, email or registration number already exists!");
                    reader.Close();
                    return;
                }
                else if (txtContact.Text == string.Empty || txtEmail.Text == string.Empty || txtRegNo.Text == string.Empty || txtfname.Text == string.Empty || txtlname.Text == string.Empty || cbStatus.Text == string.Empty)
                {
                    MessageBox.Show("No field shoud be empty!");
                    reader.Close();
                    return;
                }
                else
                {
                    cmd = new SqlCommand("UPDATE Student SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, Email = @Email, RegistrationNumber = @RegistrationNumber, Status = @Status WHERE Id = @Id", con);
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
                    cmd.Parameters.AddWithValue("@Id", txtID.Text);
                }
            }
            reader.Close();

            cmd.ExecuteNonQuery();
            MessageBox.Show("Successfully updated");

            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("Select * from [Student]", con1);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
            cmd1.ExecuteNonQuery();

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
