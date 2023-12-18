using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace studentAssessmentProject
{
    public partial class assessmentResult : Form
    {
        DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
        public assessmentResult()
        {
            InitializeComponent();
        }

        private void assessmentResult_Load(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Title FROM Assessment", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbAssID.DataSource = dt;
            cbAssID.ValueMember = "Title";
            dataGridView1.AllowUserToAddRows = false;

            btnLoadStudents.Enabled = false;
            btnCalculate.Enabled = false;
            btnReport.Enabled = false;
            btnSaveResult.Enabled = false;
            cbAssID.Enabled = false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            
            SqlCommand cmd = new SqlCommand("SELECT Student.Id as [Student Id],Student.FirstName+' '+Student.LastName as [Student Name],Student.RegistrationNumber,Student.Status,AssessmentComponent.Id as [AC Id],AssessmentComponent.Name,AssessmentComponent.TotalMarks,AssessmentId,Assessment.Title,Rubric.Id as [Rubric Id],Rubric.Details,Rubric.CloId FROM Student,AssessmentComponent JOIN Assessment ON Assessment.Id=AssessmentComponent.AssessmentId JOIN Rubric ON Rubric.Id=AssessmentComponent.RubricId JOIN Clo ON Rubric.CloId=Clo.Id WHERE NOT EXISTS(SELECT * FROM StudentResult WHERE Student.Id=StudentResult.StudentId AND AssessmentComponent.Id=StudentResult.AssessmentComponentId) AND Student.Status = 5 ORDER BY Student.Id ASC", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new System.Drawing.Font("Century Gothic", 10f);
            cbAssID.Enabled = true;
            btnLoad.Enabled = false;
            btnLoadStudents.Enabled = true;
        }

        private void btnLoadStudents_Click(object sender, EventArgs e)
        {
            //cbAssID.Enabled = false;
            string value = cbAssID.Text;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Student.Id as [Student Id],Student.FirstName+' '+Student.LastName as [Student Name],Student.RegistrationNumber,Student.Status,AssessmentComponent.Id as [AC Id],AssessmentComponent.Name,AssessmentComponent.TotalMarks,AssessmentId,Assessment.Title,Rubric.Id as [Rubric Id],Rubric.Details,Rubric.CloId FROM Student,AssessmentComponent JOIN Assessment ON Assessment.Id=AssessmentComponent.AssessmentId JOIN Rubric ON Rubric.Id=AssessmentComponent.RubricId JOIN Clo ON Rubric.CloId=Clo.Id WHERE NOT EXISTS(SELECT * FROM StudentResult WHERE Student.Id=StudentResult.StudentId AND AssessmentComponent.Id=StudentResult.AssessmentComponentId) AND Student.Status = 5 AND Assessment.Title = @value ORDER BY Student.Id ASC", con);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //SqlCommand cmd = new SqlCommand("SELECT Student.Id AS [Student Id], Student.FirstName + ' ' + Student.LastName AS [Student Name], Student.RegistrationNumber, Student.Status, AssessmentComponent.Id AS [AC Id], AssessmentComponent.Name, AssessmentComponent.TotalMarks, AssessmentId, Assessment.Title, Rubric.Id AS [Rubric Id], Rubric.Details, Rubric.CloId FROM Student, AssessmentComponent JOIN Rubric ON Rubric.Id = AssessmentComponent.RubricId JOIN Assessment ON AssessmentComponent.AssessmentId = Assessment.Id WHERE Student.Status = '5' AND AssessmentComponent.AssessmentId = @value ORDER BY Student.Id, AssessmentComponent.Id", con);
            cmd.Parameters.AddWithValue("@value", value);
            //cmd2.Parameters.AddWithValue("@Id", txtID.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new System.Drawing.Font("Century Gothic", 10f);

            if (!(dataGridView1.Columns.Contains(comboBoxColumn)))
            {
                //Set the properties of the ComboBox
                //DataGridViewColumn marksColumn = new DataGridViewColumn();
                //marksColumn.CellTemplate = new DataGridViewTextBoxCell();
                //dataGridView1.Columns.Add(marksColumn);
                //marksColumn.Name = "Obtained Marks";
                comboBoxColumn.Name = "Measurement Level";
                comboBoxColumn.DisplayStyle = (DataGridViewComboBoxDisplayStyle)ComboBoxStyle.DropDownList;
                comboBoxColumn.Items.AddRange(new string[] { "1", "2", "3", "4" });
                comboBoxColumn.ValueType = typeof(string);
                comboBoxColumn.DisplayStyleForCurrentCellOnly = true;

                //Add validation to the column
                DataGridViewCell cell = comboBoxColumn.CellTemplate;
                //cell.Value = "";
                cell.ErrorText = "Value not selected";

                //Add to the DataGridView
                dataGridView1.Columns.Add(comboBoxColumn);
                //dataGridView1.Columns.Add(marksColumn);
            }
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
                    MessageBox.Show("Select Measurement Level for every student!");
                    break;
                }

            }

            if (allCellsValid)
            {
                btnSaveResult.Enabled = true;
                btnCalculate.Enabled = true;
            }
        }

        private void btnSaveResult_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO StudentResult VALUES (@StudentId, @AssessmentComponentId, @RubricMeasurementId,@EvaluationDate)", con);
                cmd.Parameters.AddWithValue("@StudentId", row.Cells[0].Value);
                cmd.Parameters.AddWithValue("@AssessmentComponentId", row.Cells[4].Value);
                cmd.Parameters.AddWithValue("@RubricMeasurementId", row.Cells[12].Value);
                cmd.Parameters.AddWithValue("@EvaluationDate", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
            btnSaveResult.Enabled = false;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridView1[e.ColumnIndex, e.RowIndex];
            if (cell.ColumnIndex == comboBoxColumn.Index)
            {
                cell.ErrorText = "";
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount != 0)
            {
                dataGridView1.Rows.Clear();
            }
            if (dataGridView1.ColumnCount != 0)
            {
                dataGridView1.Columns.Clear();
            }

            
            btnSaveResult.Enabled = false;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT StudentResult.StudentId as SID,Student.FirstName+' '+Student.LastName+'('+Student.RegistrationNumber+')' as [Student Name],StudentResult.RubricMeasurementId as Level,Assessment.Title+'-'+AssessmentComponent.Name as Component,Assessment.TotalWeightage,AssessmentComponent.TotalMarks,CAST(AssessmentComponent.TotalMarks*(RubricMeasurementId/4.0) AS Float) AS [Obtained Marks],CAST(AssessmentComponent.TotalMarks*(RubricMeasurementId/4.0)/AssessmentComponent.TotalMarks*100 AS Float) AS [Obtained Weightage] FROM StudentResult JOIN AssessmentComponent ON StudentResult.AssessmentComponentId=AssessmentComponent.Id JOIN Assessment ON Assessment.Id=AssessmentComponent.AssessmentId JOIN Rubric ON Rubric.Id=AssessmentComponent.RubricId JOIN Clo ON Clo.Id=Rubric.CloId JOIN Student ON Student.Id=StudentResult.StudentId WHERE Assessment.Title = @Title", con);
            cmd.Parameters.AddWithValue("@Title", cbAssID.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new System.Drawing.Font("Century Gothic", 10f);
            btnReport.Enabled = true;
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "Result.pdf";
                bool Error = false;
                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            Error = true;
                            MessageBox.Show("Unable to wride data in disk" + ex.Message);
                        }
                    }
                    if (!Error)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(dataGridView1.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            foreach (DataGridViewColumn col in dataGridView1.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pTable.AddCell(pCell);
                            }
                            foreach (DataGridViewRow viewRow in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell dcell in viewRow.Cells)
                                {
                                    if (dcell.Value != null)
                                    {
                                        pTable.AddCell(dcell.Value.ToString());
                                    }

                                    //else
                                    //    pTable.AddCell(dcell.Value.ToString());
                                }
                            }
                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            MessageBox.Show("Data Export Successfully", "info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error while exporting Data" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record Found", "Info");
            }

        }
    }
}
