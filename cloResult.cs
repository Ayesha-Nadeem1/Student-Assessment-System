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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.NetworkInformation;

namespace studentAssessmentProject
{
    public partial class cloResult : Form
    {
        public cloResult()
        {
            InitializeComponent();
        }

        private void cloResult_Load(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Clo", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbCloID.DataSource = dt;
            cbCloID.ValueMember = "Id";
            dataGridView1.AllowUserToAddRows = false;

            btnLoadStudents.Enabled = false;
            //btnCalculateResult.Enabled = false;
            btnReport.Enabled = false;
            
            cbCloID.Enabled = false;
            btnCalculate.Enabled = false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT StudentResult.StudentId as SID,Student.FirstName+' '+Student.LastName +'(' +Student.RegistrationNumber+')'  as [Student Name],StudentResult.RubricMeasurementId as Level,Assessment.Title+'-'+AssessmentComponent.Name as Component,Clo.Id,AssessmentComponent.TotalMarks,CAST(AssessmentComponent.TotalMarks *(RubricMeasurementId/4.0) AS Float) AS [Obtained Marks],CAST(AssessmentComponent.TotalMarks *(RubricMeasurementId/4.0)/AssessmentComponent.TotalMarks*100 AS Float) AS [Obtained Weightage] FROM StudentResult JOIN AssessmentComponent ON StudentResult.AssessmentComponentId=AssessmentComponent.Id JOIN Assessment ON Assessment.Id=AssessmentComponent.AssessmentId JOIN Rubric ON Rubric.Id = AssessmentComponent.RubricId JOIN Clo ON Clo.Id = Rubric.CloId JOIN Student ON Student.Id=StudentResult.StudentId", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new System.Drawing.Font("Century Gothic", 10f);
            cbCloID.Enabled = true;
            btnLoad.Enabled = false;
            btnLoadStudents.Enabled = true;
            
        }

        private void btnLoadStudents_Click(object sender, EventArgs e)
        {
            //cbCloID.Enabled = false;
            btnLoad.Enabled = false;
            int value = int.Parse(cbCloID.Text);
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT StudentResult.StudentId as SID,Student.FirstName+' '+Student.LastName +'(' +Student.RegistrationNumber+')'  as [Student Name],StudentResult.RubricMeasurementId as Level,Assessment.Title+'-'+AssessmentComponent.Name as Component,Clo.Id,AssessmentComponent.TotalMarks,CAST(AssessmentComponent.TotalMarks *(RubricMeasurementId/4.0) AS Float) AS [Obtained Marks],CAST(AssessmentComponent.TotalMarks *(RubricMeasurementId/4.0)/AssessmentComponent.TotalMarks*100 AS Float) AS [Obtained Weightage] FROM StudentResult JOIN AssessmentComponent ON StudentResult.AssessmentComponentId=AssessmentComponent.Id JOIN Assessment ON Assessment.Id=AssessmentComponent.AssessmentId JOIN Rubric ON Rubric.Id = AssessmentComponent.RubricId JOIN Clo ON Clo.Id = Rubric.CloId JOIN Student ON Student.Id=StudentResult.StudentId WHERE Clo.Id = @value", con);
            
            cmd.Parameters.AddWithValue("@value", value);
            //cmd2.Parameters.AddWithValue("@Id", txtID.Text);
            DataTable dataTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new System.Drawing.Font("Century Gothic", 10f);
            btnCalculate.Enabled = true;

            


        }

       

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "Result.pdf";
                bool ErrorMessage = false;
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
                            ErrorMessage = true;
                            MessageBox.Show("Unable to wride data in disk" + ex.Message);
                        }
                    }
                    if (!ErrorMessage)
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

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            
            var con = Configuration.getInstance().getConnection();
            
            SqlCommand cmd = new SqlCommand("SELECT StudentResult.StudentId as SID,Student.FirstName+' '+Student.LastName +'(' +Student.RegistrationNumber+')'  as [Student Name],Clo.Id,SUM(AssessmentComponent.TotalMarks) AS [Total CLO Marks],SUM(CAST(AssessmentComponent.TotalMarks *(RubricMeasurementId/4.0) AS Float)) AS [Obtained CLO Marks] FROM StudentResult JOIN AssessmentComponent ON StudentResult.AssessmentComponentId=AssessmentComponent.Id\r\nJOIN Assessment\r\nON Assessment.Id=AssessmentComponent.AssessmentId JOIN Rubric ON Rubric.Id = AssessmentComponent.RubricId JOIN Clo ON Clo.Id = Rubric.CloId JOIN Student ON Student.Id=StudentResult.StudentId WHERE Clo.Id = @value GROUP BY StudentResult.StudentId,Student.FirstName+' '+Student.LastName +'(' +Student.RegistrationNumber+')',Clo.Id", con);
            cmd.Parameters.AddWithValue("@value", cbCloID.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.ForeColor = Color.Black;
            dataGridView1.Font = new System.Drawing.Font("Century Gothic", 10f);
            btnReport.Enabled = true;
        }
    }
}
