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
    public partial class FromUpdateOrDeleteMedicines : Form
    {
        MongoDB.Driver.IMongoCollection<Models.Medicines> medicines;
        public FromUpdateOrDeleteMedicines(MongoDB.Driver.IMongoCollection<Models.Medicines> medicines)
        {
            InitializeComponent();
            this.medicines = medicines; 
        }

        private void btn_DeleteMedicine_Click(object sender, EventArgs e)
        {
            //Get the Mongo DB ID from the screen
            string id = textBox_MedicineMongoDbId.Text;

            //Using the DeleOne command by the MonogoID
            try
            {
                var filter = Builders<Models.Medicines>.Filter.Eq(p => p.MedicineId, id);
                DeleteResult deleteResult = medicines.DeleteOne(filter);

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

        private void btn_UpdateMedicine_Click(object sender, EventArgs e)
        {
            //Get the MongoID that belongs to specific medicne from the screen
            string id = textBox_MedicineMongoDbId.Text;

            //Where the MedicineId equals to id filter == medicine with Mongoid equals id
            var filter = Builders<Models.Medicines>.Filter.Eq(m => m.MedicineId, id);



            if (!isCode(textBox_UpdateOrDeleteMedicineCode.Text))
            {
                MessageBox.Show("Medicine Code have to be Like : nnnn-nnnn (n number between 0 and 9)",
                                "Wrong input",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                return; // exit from the function if code is not valid and  show message for the user
            }

            if (!isName(textBox_UpdateOrDeleteMedicineName.Text))
            {
                MessageBox.Show("Medicine Name Contains Just a letters",
                                "Wrong input",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                 
                return; // exit from the function if medicine name is not valid and show a message for the user
            }

            if (!isDate(textBox_UpdateOrDeleteMedicineReleaseDate.Text) || !isDate(textBox_UpdateOrDeleteMedicineExpiratinDate.Text))
            {
                MessageBox.Show("Date have to be real Date MM/DD/YYYY",
                               "Wrong input",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);

                return; // exit from the function if Date is not valid and show a message for the user
            }






            //Define the update rule
            var updateDefinition = Builders<Models.Medicines>.Update.Set(m => m.MedicineCode,textBox_UpdateOrDeleteMedicineCode.Text)
                                                .Set(m => m.MedicineName, textBox_UpdateOrDeleteMedicineName.Text)
                                                .Set(m => m.MedicineTypeOfDisease, comboBox_UpdateOrDeleteMedicineTypeOfDisease.Text)
                                                .Set(m => m.MedicinesType , comboBox_MedicineType.Text)
                                                .Set(m => m.MedicineQuantity, textBox_UpdateOrDeleteMedicineQuantity.Text)
                                                .Set(m => m.MedicineReleaseDate, textBox_UpdateOrDeleteMedicineReleaseDate.Text)
                                                .Set(m => m.MedicineExpirationDate, textBox_UpdateOrDeleteMedicineExpiratinDate.Text);


            //Using the UpdateOne command by the MonogoID
            //if there was error the user will see a MessageBox
            try
            {
                //Perform the update 
                UpdateResult updateResult = medicines.UpdateOne(filter, updateDefinition);


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




        public static bool isCode(String code)
        //Function checks if the medicine code valid  
        {
            if (code.Length != 10)
            {
                return false;
            }
            return '0' <= code[0] && code[0] <= '9' && '0' <= code[1] && code[1] <= '9'
                && '0' <= code[2] && code[2] <= '9' && '0' <= code[3] && code[3] <= '9'
                && '0' <= code[4] && code[4] <= '9'
                && code[5] == '-' && '0' <= code[6] && code[6] <= '9'
                && '0' <= code[7] && code[7] <= '9' && '0' <= code[8] && code[8] <= '9'
                && '0' <= code[9] && code[9] <= '9';
        }

        public static bool isName(String name)
        //Function checks if phoneNumber is valid 
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
