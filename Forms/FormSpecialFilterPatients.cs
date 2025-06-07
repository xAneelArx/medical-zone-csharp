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
    public partial class FormSpecialFilterPatients : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MyMongo"].ConnectionString;
        IMongoCollection<Models.Patient> patientsCollection;
        public FormSpecialFilterPatients()
        {
            InitializeComponent();
        }

        private void FormSpecialFilterPatients_Load(object sender, EventArgs e)
        {
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
        }

            public void LoadPatientsUponScreen()
            //Function refresh FormFilterPatients
            {
                List<Models.Patient> results;

                results = patientsCollection.Aggregate().ToList();
                FilterDefinition<Models.Patient> emptyFilter = Builders<Models.Patient>.Filter.Empty;

                results = patientsCollection.Find(emptyFilter).ToList();
                dataGridView_SpecialFilterPatient.DataSource = results;
                dataGridView_SpecialFilterPatient.Columns["PatientId"].Visible = false;
            }

        private void btn_FilterSpecialFilterPatient_Click(object sender, EventArgs e)
        //function to filter By using "AND" or "OR"
        {

            try
            {
                if (comboBox_AndOrAgeGender.Text == "AND" && comboBox_AndOrGenderTypeOfDisese.Text == "AND")
                {
                    string AgeToFilter = textBox_SpecialFilterPatientAge.Text;
                    string GenderToFilter = textBox_SpecialFilterPatientGender.Text;
                    string TypeOfDisease = textBox_SpecialFilterPatientTypeOfDisease.Text;


                    if (AgeToFilter.Length > 0 && GenderToFilter.Length > 0 && TypeOfDisease.Length > 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) &
                            Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter) &
                            Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else
                    {
                        MessageBox.Show("Patient Not Found",
                                       "Not Found",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        return;
                    }


                }

                else if (comboBox_AndOrAgeGender.Text == "AND" && comboBox_AndOrGenderTypeOfDisese.Text == "OR")
                {

                    string AgeToFilter = textBox_SpecialFilterPatientAge.Text;
                    string GenderToFilter = textBox_SpecialFilterPatientGender.Text;
                    string TypeOfDisease = textBox_SpecialFilterPatientTypeOfDisease.Text;



                    if (AgeToFilter.Length > 0 && GenderToFilter.Length > 0 && TypeOfDisease.Length == 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) &
                            Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else if (AgeToFilter.Length > 0 && GenderToFilter.Length > 0 && TypeOfDisease.Length > 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) &
                           (Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter) |
                            Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease));

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }


                    else if (AgeToFilter.Length > 0 && GenderToFilter.Length == 0 && TypeOfDisease.Length > 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) &
                            Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else
                    {
                        MessageBox.Show("Patient Not Found",
                                        "Not Found",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }


                }


                else if (comboBox_AndOrAgeGender.Text == "OR" && comboBox_AndOrGenderTypeOfDisese.Text == "OR")
                {
                    string AgeToFilter = textBox_SpecialFilterPatientAge.Text;
                    string GenderToFilter = textBox_SpecialFilterPatientGender.Text;
                    string TypeOfDisease = textBox_SpecialFilterPatientTypeOfDisease.Text;


                    if (AgeToFilter.Length == 0 && GenderToFilter.Length == 0 && TypeOfDisease.Length == 0)
                    {
                        MessageBox.Show("Patient Not Found",
                                      "Not Found",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                        return;
                    }

                    else
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) |
                       Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter) |
                        Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }
                }



                else if (comboBox_AndOrAgeGender.Text == "OR" && comboBox_AndOrGenderTypeOfDisese.Text == "AND")
                {
                    string AgeToFilter = textBox_SpecialFilterPatientAge.Text;
                    string GenderToFilter = textBox_SpecialFilterPatientGender.Text;
                    string TypeOfDisease = textBox_SpecialFilterPatientTypeOfDisease.Text;

                    if (AgeToFilter.Length > 0 && GenderToFilter.Length == 0 && TypeOfDisease.Length == 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter);
                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else if (AgeToFilter.Length > 0 && GenderToFilter.Length > 0 && TypeOfDisease.Length > 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) |
                           (Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter) &
                            Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease));
                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else
                    {
                        MessageBox.Show("Patient Not Found",
                                       "Not Found",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        return;
                    }
                }
                

                else if (comboBox_AndOrAgeGender.Text == "AND" && comboBox_AndOrGenderTypeOfDisese.Text == "")
                {
                    string AgeToFilter = textBox_SpecialFilterPatientAge.Text;
                    string GenderToFilter = textBox_SpecialFilterPatientGender.Text;
                    string TypeOfDisease = textBox_SpecialFilterPatientTypeOfDisease.Text;


                    if (AgeToFilter.Length > 0 && GenderToFilter.Length > 0 && TypeOfDisease.Length == 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) &
                        Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else if (AgeToFilter.Length > 0 && GenderToFilter.Length == 0 && TypeOfDisease.Length > 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) &
                        Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else
                    {
                        MessageBox.Show("Patient Not Found",
                                       "Not Found",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        return;
                    }


                }


                else if (comboBox_AndOrAgeGender.Text == "OR" && comboBox_AndOrGenderTypeOfDisese.Text == "")
                {
                    string AgeToFilter = textBox_SpecialFilterPatientAge.Text;
                    string GenderToFilter = textBox_SpecialFilterPatientGender.Text;
                    string TypeOfDisease = textBox_SpecialFilterPatientTypeOfDisease.Text;

                    if (AgeToFilter.Length == 0 && GenderToFilter.Length == 0 && TypeOfDisease.Length == 0)
                    {
                        MessageBox.Show("Patient Not Found",
                                      "Not Found",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                        return;
                    }

                    else
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) |
                       Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter) |
                        Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                }


                else if(comboBox_AndOrAgeGender.Text == "" && comboBox_AndOrGenderTypeOfDisese.Text == "AND")
                {
                    string AgeToFilter = textBox_SpecialFilterPatientAge.Text;
                    string GenderToFilter = textBox_SpecialFilterPatientGender.Text;
                    string TypeOfDisease = textBox_SpecialFilterPatientTypeOfDisease.Text;


                    if (AgeToFilter.Length > 0 && GenderToFilter.Length > 0 && TypeOfDisease.Length == 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) &
                        Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else if (AgeToFilter.Length > 0 && GenderToFilter.Length == 0 && TypeOfDisease.Length > 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) &
                        Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else if (AgeToFilter.Length == 0 && GenderToFilter.Length > 0 && TypeOfDisease.Length > 0)
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter) &
                        Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }

                    else
                    {
                        MessageBox.Show("Patient Not Found",
                                       "Not Found",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Warning);
                        return;
                    }
                }


                else if(comboBox_AndOrAgeGender.Text == "" && comboBox_AndOrGenderTypeOfDisese.Text == "OR")
                {
                    string AgeToFilter = textBox_SpecialFilterPatientAge.Text;
                    string GenderToFilter = textBox_SpecialFilterPatientGender.Text;
                    string TypeOfDisease = textBox_SpecialFilterPatientTypeOfDisease.Text;

                    
                    if (AgeToFilter.Length == 0 && GenderToFilter.Length == 0 && TypeOfDisease.Length == 0)
                    {
                        MessageBox.Show("Patient Not Found",
                                      "Not Found",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Warning);
                        return;
                    }

                    else
                    {
                        FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientAge, AgeToFilter) |
                       Builders<Models.Patient>.Filter.Eq(p => p.PatientGender, GenderToFilter) |
                        Builders<Models.Patient>.Filter.Eq(p => p.PatientTypeOfDisease, TypeOfDisease);

                        List<Models.Patient> results = patientsCollection.Aggregate().Match(filter).ToList();
                        dataGridView_SpecialFilterPatient.DataSource = results;
                    }
                }



            }

            catch (Exception ex)
            {
                MessageBox.Show("There Was An Error",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}

