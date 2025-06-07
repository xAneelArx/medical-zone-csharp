using MongoDB.Driver;
using Hospital.Models;
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
using System.Text.RegularExpressions;

namespace Hospital.Forms
{
    public partial class FormPatient : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        IMongoCollection<Models.Patient> patientsCollection;
        public FormPatient()
        {
            InitializeComponent();
        }

        private void FormPatient_Load(object sender, EventArgs e)
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


                //When the form is loaded - we would like to get the list of all patient
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
        //Function that Refreshes the data on the screen
        {

            FormTreatment formTreatment = new FormTreatment();
            List<Models.Patient> results;

            
            results = patientsCollection.Aggregate().ToList();

            
            //FilterDefinition<Models.Patient> emptyFilter = Builders<Models.Patient>.Filter.Empty;
            //results = patientsCollection.Find(emptyFilter).ToList();

            formTreatment.dataGridView_PatientTable.DataSource = results;
        }



        private void btn_InsertPatient(object sender, EventArgs e)
        //Event click that Check if all the data is valid then Insert tha Patient(obj) to the Patient Table
        {
            Models.Patient patient = GetPatientDetailsFromScreen();

            if (!isID(patient.PatientIdNumber))
            {
                MessageBox.Show("Patient ID have to be real number that contain just 9 numbers",
                               "Wrong input",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);

                return; // exit from the function if ID input is not valid
            }


            if (!isName(patient.PatientFirstName) || !isName(patient.PatientLastName))
            {
                MessageBox.Show("First Name And Last Name have to be real",
                              "Wrong input",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);

                return; // exit from the function if name input is not valid
            }

            if (!Regex.IsMatch(patient.PatientAge, @"^\d+$"))
            {
                MessageBox.Show("Patient age have to be A Number",
                                "Wrong input",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return; // exit from the function if age input is not valid
            }


            if (!isPhoneNumber(patient.PatientPhoneNumber))
            {
                MessageBox.Show("Patient phone number have to be real number that contain 10 numbers",
                               "Wrong input",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);

                return; // exit from the function if phonenumber input is not valid
            }

           

            if (!isDate(patient.PateintDateRecieved) || !isDate(patient.PateintReleaseDate))
            {
                MessageBox.Show("Date have to be real Date MM/DD/YYYY",
                               "Wrong input",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);

                return; // exit from the function if Date input is not valid
            }

            


            try
            {
                patientsCollection.InsertOne(patient);
                MessageBox.Show("The following patient was inserted:\n" + patient.ToString(),
                                "Patient was inserted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                //If the insert succeeded - refresh the screen with the new information
                LoadPatientsUponScreen();

            }
            catch (Exception ex)
            {
                MessageBox.Show("The patient was NOT inserted , and we got the following error :\n" + ex.Message,
                                "Patient was NOT inserted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            textBox_PatientID.Text = "";
            textBox_PatientFirstName.Text = "";
            textBox_PatientLastName.Text = "";
            textBox_PatientAge.Text = "";
            textBox_PatientPhoneNumber.Text = "";
            textBox_PatientDateRecieved.Text = "";
            textBox_PatientReleaseDate.Text = "";

        }


        private Patient GetPatientDetailsFromScreen()
        //Function returns Patient object 
        //1- get all the data from the screen
        //2- put all the data to Patient Object
        {

            string patientID = textBox_PatientID.Text;
            string patientFirstName = textBox_PatientFirstName.Text;
            string patientLastName = textBox_PatientLastName.Text;
            string patientAge = textBox_PatientAge.Text;
            string patientGender = comboBox_PatientAge.Text;
            string patientPhoneNumber = textBox_PatientPhoneNumber.Text;
            string patientCityAddress = comboBox_PatientCityAddress.Text;
            string patientTypeOfDisease = comboBox_PatientTypeOfDisease.Text;
            string patientDateRecievde = textBox_PatientDateRecieved.Text;
            string patientReleaseDate = textBox_PatientReleaseDate.Text;

            //Create patient object
            Patient patient = new Patient(patientID, patientFirstName, patientLastName, patientAge, patientGender
            , patientPhoneNumber, patientCityAddress, patientTypeOfDisease, patientDateRecievde, patientReleaseDate);

            return patient;
        }

        public static bool isPhoneNumber(String phoneNumber)
        //Function to check if the PhoneNumber is valid
        {
            return phoneNumber[0] == '0' && phoneNumber[1] == '5' && IsDigit(phoneNumber) && phoneNumber.Length == 10;
        }


        public static bool IsDigit(String input)
        //Function to checks if the "String input" is only digits
        {
            foreach(char c in input)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }


        public static bool isID(String id)
        //Function to check if the id is valid
        {
            return IsDigit(id) && id.Length == 9;
        }

        public static bool isDate(string date)
        //Function to check if the Date is valid
        {
            DateTime dateTime;
            return DateTime.TryParse(date, out dateTime);
        }

        public static bool isName(String name)
        //Function to check if the Name [Not only for the name(Strings in general))] is valid
        {
            Regex regex = new Regex(@"^[a-zA-Z]+$");

            return regex.IsMatch(name);
        }
    }


}
