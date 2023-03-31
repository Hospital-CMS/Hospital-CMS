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

namespace Hospital_CMS.Controllers
{
    public class DoctorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DoctorData/ListDoctors
        //curl https://localhost:44370/api/DoctorData/ListDoctors
        [HttpGet]
        [ResponseType(typeof(DoctorDto))]
        public IEnumerable<DoctorDto> ListDoctors()
        {
           List<Doctor> Doctors =db.Doctors.ToList();
            List<DoctorDto> DoctorDtos = new List<DoctorDto>();

            Doctors.ForEach(a => DoctorDtos.Add(new DoctorDto() {
                DoctorId = a.DoctorId,
                DoctorFirstname = a.DoctorFirstname,
                DoctorLastname = a.DoctorLastname,
                Gender =a.Gender,
                LicenceNo =a.LicenceNo,
                ContactNo =a.ContactNo,
                SpecilizationId= a.Specilization.SpecilizationId,
                SpecilizationName =a.Specilization.SpecilizationName
                
            }));

            return DoctorDtos;
        }



        /// <summary>
        /// Gather all information about doctor related to particular specilization id
        /// </summary>
        /// <param name="id"> Specilization Id</param>
        /// <returns></returns>

        // GET: api/DoctorData/ListDoctorsForSpecilization/2
        //curl https://localhost:44370/api/DoctorData/ListDoctors
        [HttpGet]
        [ResponseType(typeof(DoctorDto))]
        public IEnumerable<DoctorDto> ListDoctorsForSpecilization(int id)
        {
            List<Doctor> Doctors = db.Doctors.Where(a=>a.SpecilizationId==id).ToList();
            List<DoctorDto> DoctorDtos = new List<DoctorDto>();

            Doctors.ForEach(a => DoctorDtos.Add(new DoctorDto()
            {
                DoctorId = a.DoctorId,
                DoctorFirstname = a.DoctorFirstname,
                DoctorLastname = a.DoctorLastname,
                Gender = a.Gender,
                LicenceNo = a.LicenceNo,
                ContactNo = a.ContactNo,
                SpecilizationId = a.Specilization.SpecilizationId,
                SpecilizationName = a.Specilization.SpecilizationName

            }));

            return DoctorDtos;
        }



        // GET: api/DoctorData/FindDoctor/5
        // curl "https://localhost:44370/api/DoctorData/FindDoctor/1"
        [ResponseType(typeof(Doctor))]
        [HttpGet]
        public IHttpActionResult FindDoctor(int id)
        {
            Doctor Doctor = db.Doctors.Find(id);
            DoctorDto DoctorDto = new DoctorDto()
            {
                DoctorId = Doctor.DoctorId,
                DoctorFirstname= Doctor.DoctorFirstname,
                DoctorLastname =Doctor.DoctorLastname,
                Gender = Doctor.Gender,
                LicenceNo =Doctor.LicenceNo,
                ContactNo =Doctor.ContactNo,
                SpecilizationId = Doctor.Specilization.SpecilizationId,
                SpecilizationName= Doctor.Specilization.SpecilizationName

            };
            if (Doctor == null)
            {
                return NotFound();
            }

            return Ok(DoctorDto);
        }

        // POST: api/DoctorData/UpdateDoctor/5
        //curl -d @doctor.json -H "Content-type:application/json" "https://localhost:44370/api/DoctorData/updateDoctor/3"
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateDoctor(int id, Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctor.DoctorId)
            {
                return BadRequest();
            }

            db.Entry(doctor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DoctorData/AddDoctor
        //curl -d @doctor.json -H "Content-type:application/json" https://localhost:44370/api/DoctorData/AddDoctor
        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult AddDoctor(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Doctors.Add(doctor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctor.DoctorId }, doctor);
        }

        // POST: api/DoctorData/DeleteDoctor/5
        //curl -d "" https://localhost:44370/api/DoctorData/DeleteDoctor/2

        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }

            db.Doctors.Remove(doctor);
            db.SaveChanges();

            return Ok(doctor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorExists(int id)
        {
            return db.Doctors.Count(e => e.DoctorId == id) > 0;
        }
    }
}