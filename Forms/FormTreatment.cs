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
    public partial class FormTreatment : Form
    {
        public static FormTreatment formTreatment;//object For FormTreatment WinForm to use it in another classes 
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        IMongoCollection<Models.Patient> patientsCollection;
        IMongoCollection<Models.Medicines> medicinesCollection;
        IMongoCollection<Models.PatientTreatment> patientTreatmentCollection;

        public FormTreatment()
        {
            InitializeComponent();
            formTreatment = this;
        }

        private void FormTreatment_Load(object sender, EventArgs e)
        //Function that loads the Treatment details from the MongoDB and Show it on the screen
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

                //Get the collection that is called 'Patients' and Medicines and 'patient_Treatment'
                patientsCollection = db.GetCollection<Models.Patient>("Patients");
                medicinesCollection = db.GetCollection<Models.Medicines>("Medicines");
                patientTreatmentCollection = db.GetCollection<Models.PatientTreatment>("Patient_Treatment");

                //When the form is loaded - we would like to get the list of all products
                LoadPatientsAndMedicinesUponScreen();
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


        public void LoadPatientsAndMedicinesUponScreen()
        //Function that Refreshes the data on the screen
        {
            List<Models.Patient> Patientresults;
            List<Models.Medicines> Medicinesresults;
            List<Models.PatientTreatment> PatientTreatmentResult;


            Patientresults = patientsCollection.Aggregate().ToList();
            Medicinesresults = medicinesCollection.Aggregate().ToList();
            PatientTreatmentResult = patientTreatmentCollection.Aggregate().ToList();
            

            dataGridView_PatientTable.DataSource = Patientresults;
            dataGridView_MedicinesTable.DataSource = Medicinesresults;
            dataGridView_PatientTreatment.DataSource = PatientTreatmentResult;
            dataGridView_PatientTable.Columns["PatientId"].Visible = false;
            dataGridView_MedicinesTable.Columns["MedicineId"].Visible = false;
            dataGridView_PatientTreatment.Columns["TreatmentId"].Visible = false;


        }

        private void btn_AddMedicineForPatient_Click(object sender, EventArgs e)
        //Event Click to add the Connect(Treatment)
        {
            if(dataGridView_MedicinesTable.CurrentRow == null || dataGridView_PatientTable.CurrentRow == null)
            {
                MessageBox.Show("You Have To Mark One Patient Row And One Medicine Row \n"
                               , "Error"
                               , MessageBoxButtons.OK
                               , MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                //Get the selected Patient ID And medicine Code
                string patientMongoId = dataGridView_PatientTable.CurrentRow.Cells[1].Value.ToString();
                string medicineMongoId = dataGridView_MedicinesTable.CurrentRow.Cells[1].Value.ToString();
                DateTime dateTime =  DateTime.Today;

            
                Models.PatientTreatment patientTreatment = new Models.PatientTreatment(patientMongoId , medicineMongoId , dateTime.ToString());

                    patientTreatmentCollection.InsertOne(patientTreatment);
                    MessageBox.Show("The following patient was inserted:\n" + patientTreatment.ToString(),
                                    "Patient was inserted",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    //If the insert succeeded - refresh the screen with the new information
                    LoadPatientsAndMedicinesUponScreen();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("The patient treatment was not inserted , and we got the following error :\n" + ex.Message,
                               "Patient treatment was not inserted",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                }


            }
        }

        private void dataGridVew_OpenTreatmentDetails(object sender, DataGridViewCellEventArgs e)
        //Event double click to open the Treatment details
        {
            FormTreatmentDetails formTreatmentDetails = new FormTreatmentDetails(patientTreatmentCollection);

            string id = dataGridView_PatientTreatment.CurrentRow.Cells[0].Value.ToString();

            PatientTreatment treatment = patientTreatmentCollection.Find(t => t.TreatmentId == id).FirstOrDefault();
            string patientMongoId = treatment.PatientTreatmentId;
            string medicineMongoId = treatment.MedicineTreatmentId;

            FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientId, patientMongoId);
            

            Patient patient = patientsCollection.Find(p => p.PatientIdNumber == patientMongoId).FirstOrDefault();
            Medicines medicines = medicinesCollection.Find(m => m.MedicineCode == medicineMongoId).FirstOrDefault();

            formTreatmentDetails.textBox_TreatmentMongoID.Text = dataGridView_PatientTreatment.CurrentRow.Cells[0].Value.ToString();
            

            //Add to the text boxes patient details data from table of patient
            formTreatmentDetails.textBox_UpdateOrDeletePatientID.Text = patient.PatientIdNumber;
            formTreatmentDetails.textBox_UpdateOrDeletePatientFirstName.Text = patient.PatientFirstName;
            formTreatmentDetails.textBox_UpdateOrDeletePatientLastName.Text = patient.PatientLastName;
            formTreatmentDetails.textBox_UpdateOrDeletePatientAge.Text = patient.PatientAge;
            formTreatmentDetails.textBox_UpdateOrDeletePatientGender.Text = patient.PatientGender;
            formTreatmentDetails.textBox_UpdateOrDeletePatientPhoneNumber.Text = patient.PatientPhoneNumber;
            formTreatmentDetails.textBox_UpdateOrDeletePatientCityAddress.Text = patient.PatientCityAddress;
            formTreatmentDetails.textBox_UpdateOrDeletePatientTypeOfDisease.Text = patient.PatientTypeOfDisease;
            formTreatmentDetails.textBox_UpdateOrDeletePatientDateRecieved.Text = patient.PateintDateRecieved;
            formTreatmentDetails.textBox_UpdateOrDeletePatientReleaseDate.Text = patient.PateintReleaseDate;

           


            //Add to the text boxes medicine details data from table of medicine
            formTreatmentDetails.textBox_UpdateOrDeleteMedicineCode.Text = medicines.MedicineCode;
            formTreatmentDetails.textBox_UpdateOrDeleteMedicineName.Text = medicines.MedicineName;
            formTreatmentDetails.textBox_UpdateOrDeleteMedicineTypeOfDisease.Text = medicines.MedicineTypeOfDisease;
            formTreatmentDetails.textBox_UpdateOrDeleteType.Text = medicines.MedicineTypeOfDisease;
            formTreatmentDetails.textBox_UpdateOrDeleteMedicineQuantity.Text = medicines.MedicinesType;
            formTreatmentDetails.textBox_UpdateOrDeleteMedicineReleaseDate.Text = medicines.MedicineReleaseDate;
            formTreatmentDetails.textBox_UpdateOrDeleteMedicineExpiratinDate.Text = medicines.MedicineExpirationDate;



            formTreatmentDetails.ShowDialog();

            LoadPatientsAndMedicinesUponScreen(); //Refresh Function
        }
    }
    
}
