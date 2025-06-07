using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
   public class PatientTreatment
    {
        private IMongoCollection<PatientTreatment> patientTreatmentCollection;


        //TreatmentId will be the coding name, and it will be mapped to the PK _id of the table
        [BsonId, BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string TreatmentId { get; set; }

        //PatientTreatmentId will be the coding name, and it will be mapped to the PK _id of the table
        [BsonElement("patient_treatment_id"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string PatientTreatmentId { get; set; }
     

        //MedicineTreatmentId will be the coding name, and it will be mapped to the PK _id of the table
        [BsonElement("medicine_treatment_id"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string MedicineTreatmentId { get; set; }

        //TreatmentReleaseDate is the coding name and will mapped to the treatment_release_date column
        //in the Mongo DB.
        [BsonElement("treatment_release_date"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public String TreatmentReleaseDate { get; set; }



        public PatientTreatment(string patientTreatmentId , string medicineTreatmentId , string treatmentReleaseDate)
        //Constructor for Treatment
        {
            PatientTreatmentId = patientTreatmentId;
            MedicineTreatmentId = medicineTreatmentId;
            TreatmentReleaseDate = treatmentReleaseDate;
        }

        public PatientTreatment(IMongoCollection<PatientTreatment> patientTreatmentCollection)
        {
            this.patientTreatmentCollection = patientTreatmentCollection;
        }

        public override string ToString()
        {
            return "Patient ID : " + this.PatientTreatmentId + "\nMedicine ID : " + this.MedicineTreatmentId
                + "\nTreatment Release Date : " + this.TreatmentReleaseDate;
        }



    }
}
