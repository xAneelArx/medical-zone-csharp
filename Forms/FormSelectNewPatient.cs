using Hospital.Models;
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
    public partial class FormSelectNewPatient : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        IMongoCollection<Models.Patient> patientsCollection;
        IMongoCollection<Models.PatientTreatment> treatmentCollection;

        string mongoId = string.Empty;

        public FormSelectNewPatient()
        {
            InitializeComponent();
        }

        public FormSelectNewPatient(string mongoId)
        {
            InitializeComponent();
            this.mongoId = mongoId;
        }


        private void FormSelectNewPatient_Load(object sender, EventArgs e)
        //Function that loads the Patients details from the MongoDB and Show it on the screen
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

                //Get the collection that is called 'Patients' and Medicines
                patientsCollection = db.GetCollection<Models.Patient>("Patients");
                treatmentCollection = db.GetCollection<Models.PatientTreatment>("Patient_Treatment");


                //When the form is loaded - we would like to get the list of all products
                LoadSelectNewPatientScreen();


                //When the form is first loaded - present the path for bulk activities
                //textBox_FullPathForBulk.Text = externalFile;
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

        public void LoadSelectNewPatientScreen()
        //Fucntion to Refresh the data
        {
            List<Models.Patient> PatientsResults;
            PatientsResults = patientsCollection.Aggregate().ToList();
            dataGridView_SelectPatient.DataSource = PatientsResults;
            dataGridView_SelectPatient.Columns["PatientId"].Visible = false;
        }

        private void DataGridView_SelectNewPatient(object sender, DataGridViewCellEventArgs e)
        {
            FormTreatmentDetails formTreatmentDetails = new FormTreatmentDetails(treatmentCollection);
            FormTreatment formTreatment = new FormTreatment();


            
            string patientID = dataGridView_SelectPatient.CurrentRow.Cells[1].Value.ToString();
           

            //string treatmentMongoID = formTreatmentDetails.textBox_TreatmentMongoID.Text;
            string treatmentMongoID = this.mongoId;




            FilterDefinition<Models.PatientTreatment> filter 
                = Builders<Models.PatientTreatment>.Filter.Eq(p => p.TreatmentId, treatmentMongoID);


            //Define the update rule
            var updateDefinition = Builders<Models.PatientTreatment>.Update.Set(p => p.PatientTreatmentId, patientID);
                                                                          

            
            //Using the UpdateOne command by the MonogoID
            try
            {
                //Perform the update 
                UpdateResult updateResult = treatmentCollection.UpdateOne(filter, updateDefinition);


                if (updateResult != null && updateResult.ModifiedCount == 1)
                {
                    MessageBox.Show("Item id #" + patientID + "succeesuly Updated!\n\nPresss OK to close this window",
                                    "Item Updated",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(" 1- Item id #" + patientID + "failed to be Updated\n\nPresss OK to close this window",
                                    "Item Not Updated",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" 2- Item id #" + patientID + "failed to be Updated, we got the following exceiption : \n\n" + ex.Message + "\n\nPresss OK to close this window",
                                     "Item Not Updated",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
            }
            this.Close();
           
        }

        private void btn_searchPatientID_Click(object sender, EventArgs e)
        //Event click Search Specific patient
        {
            if (textBox_SelectNewPatientSearch.TextLength > 0)
            {
                // Stage1 : Take the data out of the screen
                string idToearch = textBox_SelectNewPatientSearch.Text;
                // stage 2 ; Build the filter
                FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientIdNumber, idToearch);
                // Stage 3: Perform the filter
                List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                // Stage 4 : Present the results upon the grid
                dataGridView_SelectPatient.DataSource = results;
            }

            else
            {
                MessageBox.Show("Medicine Code had to be filled"
                                , "Enter Medicine Code"
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Warning);
            }
        }
    }
}
