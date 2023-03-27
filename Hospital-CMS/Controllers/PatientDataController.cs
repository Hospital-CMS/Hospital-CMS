using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Hospital_CMS.Models;
using System.Diagnostics;

namespace Hospital_CMS.Controllers
{
    public class PatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PatientData/ListPatients
        //curl https://localhost:44370/api/patientdata/listpatients
        [HttpGet]
        public IEnumerable<PatientDto> ListPatients()
        {
            List<Patient> Patients = db.Patients.ToList();
            List<PatientDto> PatientDtos = new List<PatientDto>();

            Patients.ForEach(p => PatientDtos.Add(new PatientDto()
            {
                PatientId = p.PatientId,
                PFName = p.PFName,
                PLName = p.PLName,
                Gender = p.Gender,
                BirthDate = p.BirthDate,
                HealthcardNo = p.HealthcardNo,
                ContactNo = p.ContactNo,
                RoomNo = p.Room.RoomNo,
                RoomType = p.Room.RoomType
            }));

            return PatientDtos;
        }

        // GET: api/PatientData/FindPatient/5
        //curl https://localhost:44370/api/patientdata/findpatient/2
        [ResponseType(typeof(Patient))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            Patient Patient = db.Patients.Find(id);
            PatientDto PatientDto = new PatientDto()
            {
                PatientId = Patient.PatientId,
                PFName = Patient.PFName,
                PLName = Patient.PLName,
                Gender = Patient.Gender,
                BirthDate = Patient.BirthDate,
                HealthcardNo = Patient.HealthcardNo,
                ContactNo = Patient.ContactNo,
                RoomNo = Patient.Room.RoomNo,
                RoomType = Patient.Room.RoomType
            };
            if (Patient == null)
            {
                return NotFound();
            }

            return Ok(PatientDto);
        }

        // POST: api/PatientData/UpdatePatient/5
        //curl -d @patient.json -H "Content-type:application/json" "https://localhost:44370/api/patientdata/updatepatient/6"
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePatient(int id, Patient patient)
        {
            Debug.WriteLine("I have reached the update patient method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != patient.PatientId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + patient.PatientId);
                Debug.WriteLine("POST parameter" + patient.PFName);
                Debug.WriteLine("POST parameter" + patient.PLName);
                Debug.WriteLine("POST parameter" + patient.Gender);
                Debug.WriteLine("POST parameter" + patient.BirthDate);
                Debug.WriteLine("POST parameter" + patient.HealthcardNo);
                Debug.WriteLine("POST parameter" + patient.ContactNo);
                return BadRequest();
            }

            db.Entry(patient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    Debug.WriteLine("Patient not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PatientData/AddPatient
        //curl -d @patient.json -H "Content-type:application/json" https://localhost:44370/api/patientdata/addpatient
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult AddPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PatientId }, patient);
        }

        // POST: api/PatientData/DeletePatient/5
        //curl -d "" https://localhost:44370/api/patientdata/deletepatient/2
        [ResponseType(typeof(Patient))]
        [HttpPost]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientId == id) > 0;
        }
    }
}