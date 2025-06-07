using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital.Forms
{
    public partial class FormFilterPatients : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        IMongoCollection<Models.Patient> patientsCollection;

        public FormFilterPatients()
        {
            InitializeComponent();
        }

        private void FormFilterPatients_Load(object sender, EventArgs e)
        //Function that loads the Patient details from the MongoDB and Show it on the screen
        {
            //Get the db NAME (as a string)
            MongoUrl mongoUrl = MongoUrl.Create(connectionString);
            string dbName = mongoUrl.DatabaseName;


            //Create Mongo Client
            MongoClient mongoClient;

            try
            {
                mongoClient = new MongoClient(connectionString);

                //Get the DB OBJECT
                IMongoDatabase db = mongoClient.GetDatabase(dbName);

                //Get the collection that is called 'Patients'
                patientsCollection = db.GetCollection<Models.Patient>("Patients");


                //When the form is loaded - we would like to get the list of all the patients from Mongo
                LoadPatientsUponScreen();

            }
            catch (Exception ex)
            {
                mongoClient = null;
                MessageBox.Show("The following error occured:\n" + ex.Message,
                                "Balagan",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

            }
        }

        public void LoadPatientsUponScreen()
        //Function refresh FormFilterPatients
        {
            List<Models.Patient> results;

            results = patientsCollection.Aggregate().ToList();
            FilterDefinition<Models.Patient> emptyFilter = Builders<Models.Patient>.Filter.Empty;

            results = patientsCollection.Find(emptyFilter).ToList();
            dataGridView_FilterPatients.DataSource = results;
            dataGridView_FilterPatients.Columns["PatientId"].Visible = false;
        }


        private void btn_Filter_Click(object sender, EventArgs e)
        //function filters by only one texbox that will be filled by the user
        {


            if (textBox_FilterPatientID.TextLength > 0)
            {
                string idToFilter = textBox_FilterPatientID.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientIdNumber, idToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else if (textBox_FilterPatientFirstName.TextLength > 0)
            {
                string firstNameToFilter = textBox_FilterPatientFirstName.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientFirstName, firstNameToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else if (textBox_FilterPatientLastName.TextLength > 0)
            {
                string lastNameToFilter = textBox_FilterPatientLastName.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientLastName, lastNameToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else if (textBox_FilterPatientAge.TextLength > 0)
            {
                string ageToFilter = textBox_FilterPatientAge.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, ageToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else if (textBox_FilterPatientGender.TextLength > 0)
            {
                string genderToFilter = textBox_FilterPatientGender.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, genderToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else if (textBox_FilterPatientPhoneNumber.TextLength > 0)
            {
                string phoneNumberToFilter = textBox_FilterPatientPhoneNumber.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientPhoneNumber, phoneNumberToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else if (textBox_FilterPatientCityAddress.TextLength > 0)
            {
                string cityAddressToFilter = textBox_FilterPatientCityAddress.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientCityAddress, cityAddressToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else if (textBox_FilterPatientTypeOfDisease.TextLength > 0)
            {
                string typeOfDiseaseToFilter = textBox_FilterPatientTypeOfDisease.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, typeOfDiseaseToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else if (textBox_FilterPatientDateRecieved.TextLength > 0)
            {
                string releaseDateToFilter = textBox_FilterPatientReleaseDate.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PateintReleaseDate, releaseDateToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else if (textBox_FilterPatientReleaseDate.TextLength > 0)
            {
                string dateRevievedToFilter = textBox_FilterPatientDateRecieved.Text;
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PateintDateRecieved, dateRevievedToFilter);
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterPatients.DataSource = results;
            }

            else
            {
                MessageBox.Show("One of patient details had to be filled"
                                , "Fill Data"
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Warning);
            }
            

        }

        private void dataGridView_OpenUpdateOrDeletePatients(object sender, DataGridViewCellEventArgs e)
        //Function to fill the data in all the textbox's in FormUpdateOrDeletePatient from the row we double clicked 
        {
           
            FormUpdateOrDeletePatients updateOrDeletePatientForm = new FormUpdateOrDeletePatients(patientsCollection);

            updateOrDeletePatientForm.textBox_MongoDbID.Text = dataGridView_FilterPatients.CurrentRow.Cells[0].Value.ToString();
            updateOrDeletePatientForm.textBox_UpdateOrDeletePatientID.Text = dataGridView_FilterPatients.CurrentRow.Cells[1].Value.ToString();
            updateOrDeletePatientForm.textBox_UpdateOrDeletePatientFirstName.Text = dataGridView_FilterPatients.CurrentRow.Cells[2].Value.ToString();
            updateOrDeletePatientForm.textBox_UpdateOrDeletePatientLastName.Text = dataGridView_FilterPatients.CurrentRow.Cells[3].Value.ToString();
            updateOrDeletePatientForm.textBox_UpdateOrDeletePatientAge.Text = dataGridView_FilterPatients.CurrentRow.Cells[4].Value.ToString();
            updateOrDeletePatientForm.comboBox_UpdateOrDeletePatientGender.Text = dataGridView_FilterPatients.CurrentRow.Cells[5].Value.ToString();
            updateOrDeletePatientForm.textBox_UpdateOrDeletePatientPhoneNumber.Text = dataGridView_FilterPatients.CurrentRow.Cells[6].Value.ToString();
            updateOrDeletePatientForm.comboBox_UpdateOrDeletePatientCityAddress.Text = dataGridView_FilterPatients.CurrentRow.Cells[7].Value.ToString();
            updateOrDeletePatientForm.comboBox_UpdateOrDeletePatientTypeOfDisease.Text = dataGridView_FilterPatients.CurrentRow.Cells[8].Value.ToString();
            updateOrDeletePatientForm.textBox_UpdateOrDeletePatientDateRecieved.Text = dataGridView_FilterPatients.CurrentRow.Cells[9].Value.ToString();
            updateOrDeletePatientForm.textBox_UpdateOrDeletePatientReleaseDate.Text = dataGridView_FilterPatients.CurrentRow.Cells[10].Value.ToString();



            

            updateOrDeletePatientForm.ShowDialog();

            LoadPatientsUponScreen();// Refresh ater we are coming back from the update/delete screen
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormSpecialFilterPatients formSpecialFilterPatients = new FormSpecialFilterPatients();

            formSpecialFilterPatients.ShowDialog();

        }
    }
}
