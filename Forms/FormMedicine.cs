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
    public partial class FormMedicine : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        IMongoCollection<Models.Medicines> medicinesCollection;

        public FormMedicine()
        {
            InitializeComponent();
        }



        private void FormMedicine_Load(object sender, EventArgs e)
        //Load FormMedicine
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

                //Get the collection that is called 'Medicines'
                medicinesCollection = db.GetCollection<Models.Medicines>("Medicines");


                //When the form is loaded - we would like to get the list of all products
                LoadMedicinesUponScreen();
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

      


        public void LoadMedicinesUponScreen()
        //Function refresh the FormMedicine
        {
            FormTreatment formTreatment = new FormTreatment();
            List<Models.Medicines> results;
            results = medicinesCollection.Aggregate().ToList();
            formTreatment.dataGridView_PatientTable.DataSource = results;
        }


        private void btn_InsertMedicine_Click(object sender, EventArgs e)
        {
            Models.Medicines medicine = GetMedicinesDetailsFromScreen();

            if (!isCode(medicine.MedicineCode))
            {
                MessageBox.Show("Medicine Code have to be Like : nnnn-nnnn (n number between 0 and 9)",
                                "Wrong input",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return; // exit from the function if code input is not valid
            }

            if (!isName(medicine.MedicineName))
            {
                MessageBox.Show("Medicine Name Contains Just a letters",
                                "Wrong input",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return; // exit from the function if name input is not valid
            }

            if (!isDate(medicine.MedicineReleaseDate) || !isDate(medicine.MedicineExpirationDate))
            {
                MessageBox.Show("Date have to be real Date  MM/DD/YYYY",
                               "Wrong input",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);

                return; // exit from the function if Date input is not valid
            }

            try
            {
                medicinesCollection.InsertOne(medicine);
                MessageBox.Show("The following product was inserted:\n" + medicine.ToString(),
                                "Product was inserted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                //If the insert succeeded - refresh the screen with the new information
                LoadMedicinesUponScreen();

            }
            catch (Exception ex)
            {
                MessageBox.Show("The product was NOT inserted , and we got the following error :\n" + ex.Message,
                                "Product was NOT inserted",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            textBox_MedicineCode.Text = "";
            textBox_MedicineName.Text = "";
            textBox_MedicineQuantity.Text = "";
            textBox_MedicineReleasedDate.Text = "";
            textBox_MedicineExpiratinDate.Text = "";
        }

        



        private Medicines GetMedicinesDetailsFromScreen()
        //Function returns Medicine object 
        //1- get all the data from the screen
        //2- put all the data to Medicine Object
        {


                string MedicineCode = textBox_MedicineCode.Text;
                string MedicineName = textBox_MedicineName.Text;
                string MedicineTypeOfDisease = comboBox_MedicineTypeOfDisease.Text;
                string MedicineType = comboBox_MedicinType.Text;
                string MedicineQuantity = textBox_MedicineQuantity.Text;
                string MedicineDateRecievde = textBox_MedicineReleasedDate.Text;
                string MedicineExpirationDate = textBox_MedicineExpiratinDate.Text;

                //create medicine object
                Medicines medicine = new Medicines(MedicineCode, MedicineName, MedicineTypeOfDisease,
                MedicineType, MedicineQuantity, MedicineDateRecievde, MedicineExpirationDate);


                return medicine;
           
        }


        public static bool isCode(String code)
        //Function Checks if the medicine code is valid
        {
            if(code.Length != 10)
            {
                return false;
            }
            return '0' <= code[0] &&  code[0] <= '9' && '0' <= code[1] && code[1] <= '9' 
                && '0' <= code[2] && code[2] <= '9' && '0' <= code[3] && code[3] <= '9'
                && '0' <= code[4] && code[4] <= '9'
                && code[5] == '-' && '0' <= code[6] && code[6] <= '9' 
                && '0' <= code[7] && code[7] <= '9' && '0' <= code[8] && code[8] <= '9'
                && '0' <= code[9] && code[9] <= '9';
        }

        public static bool isName(String name)
        //Function checks if the name include just letters
        {
            Regex regex = new Regex(@"^[a-zA-Z]+$");

            return regex.IsMatch(name);
        }

        public static bool isDate(string date)
        //Function checks if the date is valid
        {
            DateTime dateTime;
            return DateTime.TryParse(date, out dateTime);
        }

     
    }
}
