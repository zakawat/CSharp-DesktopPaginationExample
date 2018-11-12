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

namespace PaginationAssigment2407
{
    public partial class Form1 : Form
    {
        private String conString = @"Data Source=ZAKIPC;Initial Catalog=systemPrograming;User ID=sa;Password=XXXXXX";

        private int numberOfRecordsToDisplay = 0, displayedRecordStartPosition = 0, displayedRecordEndPosition = 0;

        private DataTable dt = new DataTable();
        private int totalNumberOfRecordsInDataset;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            SqlCommand cmd = new SqlCommand("SELECT * FROM Students;", new SqlConnection(conString));
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            
            sda.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                System.Diagnostics.Debug.WriteLine($"{item[0]}, {item[1]}, {item[2]}, {item[3]}");
            }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;


            totalNumberOfRecordsInDataset = dt.AsEnumerable().Count();
            btnReloadFunctionality();
            



        }


        private void btnReload_Click(object sender, EventArgs e)
        {
            btnReloadFunctionality();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            displayedRecordEndPosition = displayedRecordStartPosition;
            displayedRecordStartPosition -= numberOfRecordsToDisplay;
            loadDataIntoGridView();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            displayedRecordStartPosition = displayedRecordEndPosition;
            displayedRecordEndPosition += numberOfRecordsToDisplay;
            loadDataIntoGridView();
        }

        private void btnReloadFunctionality()
        {
            try
            {
                displayedRecordStartPosition = 0;
                numberOfRecordsToDisplay = int.Parse(txtNoOfRecords.Text);
                displayedRecordEndPosition = int.Parse(txtNoOfRecords.Text);

                if (numberOfRecordsToDisplay <= 0)
                {
                    MessageBox.Show("The record can not be Zero or Negative Number !\n\nSetting back to default value. i.e 20");
                    numberOfRecordsToDisplay = displayedRecordEndPosition = 20;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Setting back to default value. i.e 20");
                numberOfRecordsToDisplay = 20;
                txtNoOfRecords.Text = "20";
            }
            loadDataIntoGridView();
        }



        private void loadDataIntoGridView()
        {

            if (displayedRecordStartPosition == 0)
            {
                //first Page !
                btnPrevious.Enabled = false;
                btnNext.Enabled = true;
                //dataGridView1.DataSource = dt.AsEnumerable().Take(numberOfRecordsToDisplay).CopyToDataTable();
                dataGridView1.DataSource = dt.AsEnumerable().Skip(displayedRecordStartPosition).Take(numberOfRecordsToDisplay).CopyToDataTable();


            }
            else if (displayedRecordEndPosition == totalNumberOfRecordsInDataset)
            {
                //Last Page !

                btnPrevious.Enabled = true;
                btnNext.Enabled = false;
                dataGridView1.DataSource = dt.AsEnumerable().Skip(displayedRecordStartPosition).Take(numberOfRecordsToDisplay).CopyToDataTable();
                
            }
            else
            {
                //Middile Pages

                btnPrevious.Enabled = true;
                btnNext.Enabled = true;
                dataGridView1.DataSource = dt.AsEnumerable().Skip(displayedRecordStartPosition).Take(numberOfRecordsToDisplay).CopyToDataTable();

            }

                updateNavigationalInfoLabel();


        }

        private void updateNavigationalInfoLabel()
        {
            lblRecodsDisplay.Text = $"Showing {displayedRecordStartPosition+1} to {displayedRecordEndPosition} records on screen out of {totalNumberOfRecordsInDataset}";
        }
    }
}
