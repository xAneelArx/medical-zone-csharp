using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Medicines
    {
     
        //MedicineId will be the coding name, and it will be mapped to the PK _id of the table

        [BsonId, BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string MedicineId { get; set; }

        //MedicineCode is the coding name and it will mapped to the Medicine_code column
        //in the Mongo DB.
        //In the code , MedicineCode is defined as string , and also in the database - it will be stored as string 

        [BsonElement("medicine_code"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string MedicineCode { get; set; }


        //MedicineName is the coding name and it will mapped to the Medicine_Name column
        //in the Mongo DB.
        //In the code , MedicineName is defined as string , and also in the database - it will be stored as string 

        [BsonElement("medicine_Name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string MedicineName { get; set; }

        //MedicineTypeOfDisease is the coding name and it will mapped to the Medicine_name_of_disease column
        //in the Mongo DB.
        [BsonElement("medicine_name_of_disease"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string MedicineTypeOfDisease { get; set; }

        //MedicineType is the coding name and will mapped to the Medicine_type column
        //in the Mongo DB.
        [BsonElement("medicine_type"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string MedicinesType { get; set; }

        //MedicineQuantity is the coding name and will mapped to the Medicine_quantity column
        //in the Mongo DB.
        [BsonElement("medicine_quantity"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string MedicineQuantity { get; set; }

        //MedicineReleaseDate is the coding name and will mapped to the medicine_release_date column
        //in the Mongo DB.
        [BsonElement("medicine_release_date"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string MedicineReleaseDate { get; set; }

        //TreatmentExpirationDate is the coding name and will mapped to the medicine_expiration_date column
        //in the Mongo DB.
        [BsonElement("medicine_expiration_date"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string MedicineExpirationDate { get; set; }

        public Medicines(string medicineCode, string medicineName, string medicineTypeOfDisease, string medicineType, string medicineQuantity, string medicineDateRecievde, string medicineExpirationDate)
        //Constructor for Medicine
        {
            MedicineCode = medicineCode;
            MedicineName = medicineName;
            MedicineTypeOfDisease = medicineTypeOfDisease;
            MedicinesType = medicineType;
            MedicineQuantity = medicineQuantity;
            MedicineReleaseDate = medicineDateRecievde;
            MedicineExpirationDate = medicineExpirationDate;
        }

        public override string ToString()
        {
            return "Medicine Code:" + this.MedicineCode + "\nMedicine Name: " + this.MedicineName + "\nDisease Name: " + this.MedicineTypeOfDisease
                + "\nMedicine type: " + this.MedicinesType + "\nQuantity: " + this.MedicineQuantity
                + "\nRelease Date: " + this.MedicineReleaseDate + "\nExpiration Date: " + this.MedicineExpirationDate;
        }
    }
}
