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
    public partial class FormFilterMedicines : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        IMongoCollection<Models.Medicines> medicinesCollection;

        public FormFilterMedicines()
        {
            InitializeComponent();
        }
        private void FormFilterMedicines_Load(object sender, EventArgs e)
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


                //When the form is loaded - we would like to get the list of all medicines
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
        {
            List<Models.Medicines> results;
            results = medicinesCollection.Aggregate().ToList();
            dataGridView_FilterMedicines.DataSource = results;
            dataGridView_FilterMedicines.Columns["MedicineId"].Visible = false;
        }

        private void btn_FilterMedicine_Click(object sender, EventArgs e)
        {
            if (textBox_FilterMedicineCode.TextLength > 0)
            {
                string codeToFilter = textBox_FilterMedicineCode.Text;
                FilterDefinition<Models.Medicines> filter = Builders<Models.Medicines>.Filter.Eq(m => m.MedicineCode, codeToFilter);
                List<Models.Medicines> results = medicinesCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterMedicines.DataSource = results;
            }

            else if (textBox_FilterMedicineName.TextLength > 0)
            {
                string nameToFilter = textBox_FilterMedicineName.Text;
                FilterDefinition<Models.Medicines> filter = Builders<Models.Medicines>.Filter.Eq(m => m.MedicineName, nameToFilter);
                List<Models.Medicines> results = medicinesCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterMedicines.DataSource = results;
            }

            else if (textBox_FilterMedicineTypeOfDisease.TextLength > 0)
            {
                string typeOfDiseaseToFilter = textBox_FilterMedicineTypeOfDisease.Text;
                FilterDefinition<Models.Medicines> filter = Builders<Models.Medicines>.Filter.Eq(m => m.MedicineTypeOfDisease, typeOfDiseaseToFilter);
                List<Models.Medicines> results = medicinesCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterMedicines.DataSource = results;
            }

            else if (textBox_FilterType.TextLength > 0)
            {
                string typeToFilter = textBox_FilterType.Text;
                FilterDefinition<Models.Medicines> filter = Builders<Models.Medicines>.Filter.Eq(m => m.MedicinesType, typeToFilter);
                List<Models.Medicines> results = medicinesCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterMedicines.DataSource = results;
            }

            else if (textBox_FilterMedicineQuantity.TextLength > 0)
            {
                string quantityToFilter = textBox_FilterMedicineQuantity.Text;
                FilterDefinition<Models.Medicines> filter = Builders<Models.Medicines>.Filter.Eq(m=> m.MedicineQuantity, quantityToFilter);
                List<Models.Medicines> results = medicinesCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterMedicines.DataSource = results;
            }

            else if (textBox_FilterMedicineReleaseDate.TextLength > 0)
            {
                string releaseDateToFilter = textBox_FilterMedicineReleaseDate.Text;
                FilterDefinition<Models.Medicines> filter = Builders<Models.Medicines>.Filter.Eq(m => m.MedicineReleaseDate, releaseDateToFilter);
                List<Models.Medicines> results = medicinesCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterMedicines.DataSource = results;
            }

            else if (textBox_FilterMedicineExpiratinDate.TextLength > 0)
            {
                string expirationDateToFilter = textBox_FilterMedicineExpiratinDate.Text;
                FilterDefinition<Models.Medicines> filter = Builders<Models.Medicines>.Filter.Eq(m => m.MedicineExpirationDate, expirationDateToFilter);
                List<Models.Medicines> results = medicinesCollection.Aggregate().Match(filter).ToList();
                dataGridView_FilterMedicines.DataSource = results;
            }

            else
            {
                MessageBox.Show("One of medicine details had to be filled"
                                , "Fill Data"
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Warning);
            }
        }

        private void dataGridView_OpenUpdateOrDeleteMedicine(object sender, DataGridViewCellEventArgs e)
        //Function to fill the data in all the textbox's in FormUpdateOrDeleteMedicine from the row we double clicked 
        {
            FromUpdateOrDeleteMedicines updateOrDeleteMedicineForm = new FromUpdateOrDeleteMedicines(medicinesCollection);

            updateOrDeleteMedicineForm.textBox_MedicineMongoDbId.Text = dataGridView_FilterMedicines.CurrentRow.Cells[0].Value.ToString();
            updateOrDeleteMedicineForm.textBox_UpdateOrDeleteMedicineCode.Text = dataGridView_FilterMedicines.CurrentRow.Cells[1].Value.ToString();
            updateOrDeleteMedicineForm.textBox_UpdateOrDeleteMedicineName.Text = dataGridView_FilterMedicines.CurrentRow.Cells[2].Value.ToString();
            updateOrDeleteMedicineForm.comboBox_UpdateOrDeleteMedicineTypeOfDisease.Text = dataGridView_FilterMedicines.CurrentRow.Cells[3].Value.ToString();
            updateOrDeleteMedicineForm.comboBox_MedicineType.Text = dataGridView_FilterMedicines.CurrentRow.Cells[4].Value.ToString();
            updateOrDeleteMedicineForm.textBox_UpdateOrDeleteMedicineQuantity.Text = dataGridView_FilterMedicines.CurrentRow.Cells[5].Value.ToString();
            updateOrDeleteMedicineForm.textBox_UpdateOrDeleteMedicineReleaseDate.Text = dataGridView_FilterMedicines.CurrentRow.Cells[6].Value.ToString();
            updateOrDeleteMedicineForm.textBox_UpdateOrDeleteMedicineExpiratinDate.Text = dataGridView_FilterMedicines.CurrentRow.Cells[7].Value.ToString();





            updateOrDeleteMedicineForm.ShowDialog();

            LoadMedicinesUponScreen();// Refresh ater we are coming back from the update/delete screen
        }
    }
}
