using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Patient
    {
        

        //PatientId will be the coding name, and it will be mapped to the PK _id of the table

        [BsonId, BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string PatientId { get; set; }


        //PatientIdNumber is the coding name and it will mapped to the patient_id column
        //in the Mongo DB.
        //In the code , PatientIdNumber is defined as string , and also in the database - it will be stored as string 
        [BsonElement("patient_id"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string PatientIdNumber { get; set; }


        //PatientFirstName is the coding name and it will mapped to the patient_first_name column
        //in the Mongo DB.
        //In the code , PatientFirstName is defined as string , and also in the database - it will be stored as string 
        [BsonElement("patient_first_name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string PatientFirstName { get; set; }

        //PatientLastName is the coding name and it will mapped to the patient_last_name column
        //in the Mongo DB.
        //In the code , PatientLastName is defined as string , and also in the database - it will be stored as string 
        [BsonElement("patient_last_name"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string PatientLastName { get; set; }

        //PatientAge is the coding name and it will mapped to the patient_last_name column
        //in the Mongo DB.
        //In the code , PatientLastName is defined as double , and also in the database - it will be stored as double 
        [BsonElement("patient_age"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public String PatientAge { get; set; }


        //PatientGender is the coding name and it will mapped to the patient_gender column
        //in the Mongo DB.
        //In the code , PatientGender is defined as string , and also in the database - it will be stored as string 
        [BsonElement("patient_gender"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public String PatientGender { get; set; }


        //PatientPhoneNumber is the coding name and it will mapped to the patient_phone_number column
        //in the Mongo DB.
        //In the code , PatientPhoneNumber is defined as string , and also in the database - it will be stored as string 
        [BsonElement("patient_phone_number"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public String PatientPhoneNumber { get; set; }


        //PatientCityAdress is the coding name and it will mapped to the patient_city_address column
        //in the Mongo DB.
        //In the code , PatientCityAdress is defined as String , and also in the database - it will be stored as String 
        [BsonElement("patient_city_address"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public String PatientCityAddress { get; set; }


        //PatientTypeOfDisease is the coding name and it will mapped to the patient_type_of_disease column
        //in the Mongo DB.
        //In the code , PatientTypeOfDisease is defined as String , and also in the database - it will be stored as String
        [BsonElement("patient_type_of_disease"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public String PatientTypeOfDisease { get; set; }



        //PateintDateRecieved is the coding name and will mapped to the pateint_date_recieved column
        //in the Mongo DB.
        [BsonElement("pateint_date_recieved"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public String PateintDateRecieved { get; set; }


        //PateintReleaseDate is the coding name and will mapped to the pateint_release_date column
        //in the Mongo DB.
        [BsonElement("pateint_release_date"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public String PateintReleaseDate { get; set; }


        public Patient(string patientIdNumber, string patientFirstName, string patientLastName, string patientAge, string patientGender, string patientPhoneNumber, string patientCityAddress, string patientTypeOfDisease, string patientDateRecievde, string patientReleaseDate)
        //Constructor for Patient
        {
            PatientIdNumber = patientIdNumber;
            PatientFirstName = patientFirstName;
            PatientLastName = patientLastName;
            PatientAge = patientAge;
            PatientGender = patientGender;
            PatientPhoneNumber = patientPhoneNumber;
            PatientCityAddress = patientCityAddress;
            PatientTypeOfDisease = patientTypeOfDisease;
            PateintDateRecieved = patientDateRecievde;
            PateintReleaseDate = patientReleaseDate;
        }

        public override string ToString()
        {
            return "ID Number:" + this.PatientIdNumber + "\nFirst Name: " + this.PatientFirstName + "\nLast Name: " + this.PatientLastName
                + "\nPtient Age: " + this.PatientAge + "\nGender: " + this.PatientGender
                + "\nPhone Number: " + this.PatientPhoneNumber + "\nCity Address: " + this.PatientCityAddress+
                "\nDisease: "+this.PatientTypeOfDisease+"\nDate Recived: "+this.PateintDateRecieved+
                "\nRelease Date:"+this.PateintReleaseDate;
        }
    }
}
