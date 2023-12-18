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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace studentAssessmentProject
{
    public partial class studentAttendance : Form
    {
        DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();

        public studentAttendance()
        {
            InitializeComponent();
        }

        private void studentAttendance_Load(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT AttendanceDate FROM  ClassAttendance WHERE ClassAttendance.Id NOT IN (select AttendanceId from StudentAttendance)", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbAttendanceDate.DataSource = dt;
            cbAttendanceDate.ValueMember = "AttendanceDate";
            dataGridView1.AllowUserToAddRows = false;
            btnSave.Enabled = false;
            btnCheck.Enabled = false;   
        }

        private void cbAttendanceDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cbAttendanceDate.SelectedIndex == -1) //No item has been selected
            //{
            //    btnLoad.Enabled = false;
            //    btnSave.Enabled = false;
            //}
            //else  //An item has been selected
            //{
            //    btnLoad.Enabled = true;
            //    btnSave.Enabled = true;
            //}
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            cbAttendanceDate.Enabled = false;
            btnCheck.Enabled = true;
            //DateTime value = DateTime.Parse(cbAttendanceDate.Text);
            //string query = "SELECT * FROM table WHERE column = @value";
            var con = Configuration.getInstance().getConnection();
            //SqlCommand cmd = new SqlCommand("Select * from Student,ClassAttendance WHERE AttendanceDate=@value", con);
            //SqlCommand cmd = new SqlCommand("Select * from Student,ClassAttendance", con);
            //DateTime date = DateTime.Parse(cbAttendanceDate.Text);
            //cmd.Parameters.AddWithValue("@value", date);
            SqlCommand command = new SqlCommand("Select * from Student,ClassAttendance WHERE AttendanceDate=@DateSeached AND Status = 5", con);

            DateTime dateTimeValue = DateTime.Parse(cbAttendanceDate.SelectedValue.ToString());
            command.Parameters.Add("@DateSeached", SqlDbType.DateTime).Value = dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss.fff");

            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new Font("Century Gothic", 10f);
                                   
            
            if (!(dataGridView1.Columns.Contains(comboBoxColumn)))
            {
                //Set the properties of the ComboBox
                comboBoxColumn.DisplayStyle = (DataGridViewComboBoxDisplayStyle)ComboBoxStyle.DropDownList;
                comboBoxColumn.Items.AddRange(new string[] { "Present", "Absent", "Leave", "Late" });
                comboBoxColumn.ValueType = typeof(string);
                comboBoxColumn.DisplayStyleForCurrentCellOnly = true;

                //Add validation to the column
                DataGridViewCell cell = comboBoxColumn.CellTemplate;
                //cell.Value = "";
                cell.ErrorText = "Value not selected";

                //Add to the DataGridView
                dataGridView1.Columns.Add(comboBoxColumn);
            }
            
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            //MessageBox.Show("All OK :)");
            var con = Configuration.getInstance().getConnection();
            int status = 0;
            //SqlCommand cmd = new SqlCommand("INSERT INTO StudentAttendance VALUES (@value1, @value2, @value3)", con);
            //Open the connection
            //con.Open();

            //Iterate through the DataGridView rows
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                //Create a new SqlCommand object
                SqlCommand cmd = new SqlCommand("INSERT INTO StudentAttendance VALUES (@value1, @value2, @value3)",con);
                //cmd.Connection = con;
                //cmd.CommandType = CommandType.Text;
                //cmd.CommandText = "INSERT INTO dbo.TableName VALUES (@param1, @param2, ...)";

                //Add the parameters
                if(row.Cells[9].Value.ToString() == "Present")
                {
                    MessageBox.Show("1");
                   status = 1;
                }
                if (row.Cells[9].Value.ToString() == "Absent")
                {
                    MessageBox.Show("2");
                    status = 2;
                }
                if (row.Cells[9].Value.ToString() == "Leave")
                {
                    MessageBox.Show("3");
                    status = 3;
                }
                if (row.Cells[9].Value.ToString() == "Late")
                {
                    MessageBox.Show("4");
                    status = 4;
                }
                cmd.Parameters.AddWithValue("@value1", row.Cells[7].Value);
                cmd.Parameters.AddWithValue("@value2", row.Cells[0].Value);
                cmd.Parameters.AddWithValue("@value3", status);
                //string value = 
                //cmd.Parameters.AddWithValue("@value3", row.Cells[1].Value);
                //...

                //Execute the command
                cmd.ExecuteNonQuery();
            }
            //cmd.ExecuteNonQuery();
            //Close the connection
            //con.Close();
            btnSave.Enabled = false;
            btnCheck.Enabled = false;
            SqlCommand cmd1 = new SqlCommand("SELECT AttendanceDate FROM  ClassAttendance WHERE ClassAttendance.Id NOT IN (select AttendanceId from StudentAttendance)", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbAttendanceDate.DataSource = dt;
            cbAttendanceDate.ValueMember = "AttendanceDate";
            dataGridView1.AllowUserToAddRows = false;
            cbAttendanceDate.Enabled = true;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            bool allCellsValid = true;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCell cell = row.Cells[comboBoxColumn.Index];
                if (!String.IsNullOrEmpty(cell.ErrorText)) //if error text is not null then the IF condition is true 
                {
                    allCellsValid = false;
                    MessageBox.Show("Select Attendance Status for every student!");
                    break;
                }
                
            }

            if (allCellsValid)
            {
                btnSave.Enabled = true;
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (e.ColumnIndex == 0) //column index of combobox
            //{ 
            //    if (String.IsNullOrEmpty(e.FormattedValue.ToString())) 
            //    { MessageBox.Show("Please select a value"); 
            //     e.Cancel = true; 
            //    }
            //}
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridView1[e.ColumnIndex, e.RowIndex];
            if (cell.ColumnIndex == comboBoxColumn.Index)
            {
                cell.ErrorText = "";
            }
        }
    }
}
