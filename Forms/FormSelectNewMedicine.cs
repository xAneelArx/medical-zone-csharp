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
    public partial class FormSelectNewMedicine : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        IMongoCollection<Models.Medicines> medicinesCollection;
        IMongoCollection<Models.PatientTreatment> treatmentCollection;

        string mongoId = string.Empty;

        public FormSelectNewMedicine()
        {
            InitializeComponent();
        }

        public FormSelectNewMedicine(string mongoId)
        {
            InitializeComponent();
            this.mongoId = mongoId;
        }
        private void FormSelectNewMedicine_Load(object sender, EventArgs e)
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

                //Get the collection that is called 'Patients' and 'Medicines'
                medicinesCollection = db.GetCollection<Models.Medicines>("Medicines");
                treatmentCollection = db.GetCollection<Models.PatientTreatment>("Patient_Treatment");

                //When the form is loaded - we would like to get the list of all medicines
                LoadSelectNewMedicineScreen();

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

        public void LoadSelectNewMedicineScreen()
        //refresh FormSelectNewMedicine
        {
            List<Models.Medicines> Medicinesresults;
            Medicinesresults = medicinesCollection.Aggregate().ToList();
            dataGridView_selectMedicine.DataSource = Medicinesresults;
            dataGridView_selectMedicine.Columns["MedicineId"].Visible = false;
        }

        private void DataGridView_SelectNewMedicine(object sender, DataGridViewCellEventArgs e)
        {
            FormTreatment formTreatment = new FormTreatment();
            string patientID = dataGridView_selectMedicine.CurrentRow.Cells[1].Value.ToString();
            string treatmentMongoID = this.mongoId;

            FilterDefinition<Models.PatientTreatment> filter
                = Builders<Models.PatientTreatment>.Filter.Eq(p => p.TreatmentId, treatmentMongoID);

            //Define the update rule
            var updateDefinition = Builders<Models.PatientTreatment>.Update.Set(p => p.MedicineTreatmentId, patientID);


          
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

        private void btn_searchMedicineCode_Click(object sender, EventArgs e)
        //function to search specific medicine
        {
            if (textBox_SelectNewMedicineSearch.TextLength > 0)
            {
               
                string codeToSearch = textBox_SelectNewMedicineSearch.Text;
                FilterDefinition<Models.Medicines> filter = Builders<Models.Medicines>.Filter.Eq(m => m.MedicineCode, codeToSearch);
                List<Models.Medicines> results = medicinesCollection.Aggregate().Match(filter).ToList();
                dataGridView_selectMedicine.DataSource = results;
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
