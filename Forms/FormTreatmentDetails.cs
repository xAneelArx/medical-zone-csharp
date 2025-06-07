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
    public partial class FormTreatmentDetails : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        IMongoCollection<Models.Patient> patientsCollection;
        IMongoCollection<Models.Medicines> medicinesCollection;
        IMongoCollection<Models.PatientTreatment> patientTreatmentCollection;

        public FormTreatmentDetails(IMongoCollection<Models.PatientTreatment> patientTreatmentCollection)
        {
            InitializeComponent();
            this.patientTreatmentCollection = patientTreatmentCollection;
        }

        

        private void FormTreatmentDetails_Load(object sender, EventArgs e)
        {
        //Function that loads the Patient details from the MongoDB and Show it on the screen

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
                medicinesCollection = db.GetCollection<Models.Medicines>("Medicines");
                

                //When the form is loaded - we would like to get the list of all products
                LoadTreatmentDetailesScreen();


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


        public void LoadTreatmentDetailesScreen()
        //Function that Refreshes the data on the screen
        {
            List<Models.Patient> PatientsResults;

            PatientsResults = patientsCollection.Aggregate().ToList();
        }

        private void btn_SelectNewMedicine_Click(object sender, EventArgs e)
        //Event Click to Change the current Medicine to a New One
        {
            FormSelectNewMedicine formSelectNewMedicine = new FormSelectNewMedicine(textBox_TreatmentMongoID.Text);

            formSelectNewMedicine.ShowDialog();//Open Selct New Medicine Form

            this.Close();
        }

        private void btn_SelectNewPatient_Click(object sender, EventArgs e)
        //Event Click to Change the current Patient to a New One

        {
            FormSelectNewPatient formSelectNewPatient = new FormSelectNewPatient(textBox_TreatmentMongoID.Text);

            formSelectNewPatient.ShowDialog();//Open Select New Patient Form
            this.Close();
            
        }

        private void btn_deleteTreatmnet_Click(object sender, EventArgs e)
        //Event Click to Delete Treatment
        {
            //Get the Mongo DB ID from the screen
            string id = textBox_TreatmentMongoID.Text;

            //Using the DeleteOne command by the MonogoID
            try
            {
                var filter = Builders<Models.PatientTreatment>.Filter.Eq(p => p.TreatmentId, id);
                DeleteResult deleteResult = patientTreatmentCollection.DeleteOne(filter);

                //Check if we managed to delete the requeted ID
                if (deleteResult.DeletedCount == 1)
                {
                    MessageBox.Show("Item id #" + id + "succeesuly deleted!",
                                    "Item Deleted",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);


                }
                else
                {
                    MessageBox.Show("Item id #" + id + "failed to be deleted",
                                    "Item Not Deleted",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Item id #" + id + "failed to be deleted, we got the following exceiption : \n\n" + ex.Message,
                                     "Item Not Deleted",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
            }

            this.Close();
        }
    }
}
