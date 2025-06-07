using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hospital.Forms
{
    public partial class FormUpdateOrDeletePatients : Form
    {
        //define a mongo collection of Patients
        MongoDB.Driver.IMongoCollection<Models.Patient> patients;
        public FormUpdateOrDeletePatients(MongoDB.Driver.IMongoCollection<Models.Patient> patients)
        {
            InitializeComponent();
            this.patients = patients;
        }

        private void btn_DeletePatient_Click(object sender, EventArgs e)
        {
            //Get the Mongo DB ID from the screen
            string id = textBox_MongoDbID.Text;

            //Using the DeleteOne command by the MonogoID
            try
            {
                var filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientId, id);
                DeleteResult deleteResult = patients.DeleteOne(filter);

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

        private void btn_UpdatePatient_Click(object sender, EventArgs e)
        {
            //Get the ID from the screen
            string id = textBox_MongoDbID.Text;

            //Where criteria
            FilterDefinition<Models.Patient> filter = Builders<Models.Patient>.Filter.Eq(p => p.PatientId, id);

            if (!Regex.IsMatch(textBox_UpdateOrDeletePatientAge.Text, @"^\d+$"))
            {
                MessageBox.Show("Patient age have to be A Number",
                                "Wrong input",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return; // exit from the function if age input is not valid and shows message
            }

            if (!isPhoneNumber(textBox_UpdateOrDeletePatientPhoneNumber.Text))
            {
                MessageBox.Show("Patient phone number have to be real number that contain 10 numbers",
                               "Wrong input",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);

                return; // exit from the function if phonenumber input is not valid and shows message
            }

            if (!isID(textBox_UpdateOrDeletePatientID.Text))
            {
                MessageBox.Show("Patient ID have to be real number that contain just 9 numbers",
                               "Wrong input",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);

                return; // exit from the function if ID input is not valid and shows a message
            }

            if (!isDate(textBox_UpdateOrDeletePatientDateRecieved.Text) || !isDate(textBox_UpdateOrDeletePatientReleaseDate.Text))
            {
                MessageBox.Show("Date have to be real Date MM/DD/YYYY",
                               "Wrong input",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);

                return; // exit from the function if Date input is not valid and shows a message
            }

            if (!isName(textBox_UpdateOrDeletePatientFirstName.Text) || !isName(textBox_UpdateOrDeletePatientLastName.Text))
            {
                MessageBox.Show("First Name And Last Name have to be real",
                              "Wrong input",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);

                return; // exit from the function if name input is not valid (last name and first name) , and shows message 
            }




            //Define the update rule
            var updateDefinition = Builders<Models.Patient>.Update.Set(p => p.PatientIdNumber, textBox_UpdateOrDeletePatientID.Text)
                                                .Set(p => p.PatientFirstName, textBox_UpdateOrDeletePatientFirstName.Text)
                                                .Set(p => p.PatientLastName, textBox_UpdateOrDeletePatientLastName.Text)
                                                .Set(p => p.PatientAge,textBox_UpdateOrDeletePatientAge.Text)
                                                .Set(p => p.PatientGender, comboBox_UpdateOrDeletePatientGender.Text)
                                                .Set(p => p.PatientPhoneNumber, textBox_UpdateOrDeletePatientPhoneNumber.Text)
                                                .Set(p => p.PatientCityAddress, comboBox_UpdateOrDeletePatientCityAddress.Text)
                                                .Set(p => p.PatientTypeOfDisease, comboBox_UpdateOrDeletePatientTypeOfDisease.Text)
                                                .Set(p => p.PateintDateRecieved, textBox_UpdateOrDeletePatientDateRecieved.Text)
                                                .Set(p => p.PateintReleaseDate, textBox_UpdateOrDeletePatientReleaseDate.Text);


            //Using the UpdateOne command by the MonogoID
            try
            {
                //Perform the update 
                UpdateResult updateResult = patients.UpdateOne(filter, updateDefinition);


                if (updateResult != null && updateResult.ModifiedCount == 1)
                {
                    MessageBox.Show("Item id #" + id + "succeesuly Updated!\n\nPresss OK to close this window",
                                    "Item Updated",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Item id #" + id + "failed to be Updated\n\nPresss OK to close this window",
                                    "Item Not Updated",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Item id #" + id + "failed to be Updated, we got the following exceiption : \n\n" + ex.Message + "\n\nPresss OK to close this window",
                                     "Item Not Updated",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
            }
            this.Close();

        }

        public static bool isPhoneNumber(String phoneNumber)
        //Function Checks if the phonenumber is valid
        {
            return phoneNumber[0] == '0' && phoneNumber[1] == '5' && IsDigit(phoneNumber) && phoneNumber.Length == 10;
        }


        public static bool IsDigit(String input)
        //Function checks  if the input string include just numbers
        {
            foreach (char c in input)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }


        public static bool isID(String id)
        //Function checks if the Patient ID is valid
        {
            return IsDigit(id) && id.Length == 9;
        }

        public static bool isDate(string date)
        //Function checks if the date is valid
        {
            DateTime dateTime;
            return DateTime.TryParse(date, out dateTime);
        }

        public static bool isName(String name)
        //Function checks if the name include just letters
        {
            Regex regex = new Regex(@"^[a-zA-Z]+$");

            return regex.IsMatch(name);
        }
    }
    }



