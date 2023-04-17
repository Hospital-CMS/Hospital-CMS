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
using Antlr.Runtime.Tree;
using Hospital_CMS.Migrations;
using Hospital_CMS.Models;

namespace Hospital_CMS.Controllers
{
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DepartmentData/ListDepartments
        [HttpGet]
        public IEnumerable<DepartmentDto> ListDepartments()
        {
            List<Department> departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            departments.ForEach(d => DepartmentDtos.Add(new DepartmentDto()

            {
                DepartmentID = d.DepartmentID,
                DepartmentName = d.DepartmentName,
                Service = d.Service,
                //FirstName = d.Donor.FirstName,
                //LastName = d.Donor.LastName,
                //DoctorFirstname = d.Doctor.DoctorFirstname,
                //DoctorLastname = d.Doctor.DoctorLastname

            }));


            return DepartmentDtos;
        }




        // GET: api/DepartmentData/ListDepartmentForDonor/1
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IEnumerable<DepartmentDto> ListDepartmentForDonor(int id)
        {
            List<Department> Departments = db.Departments.Where(
                d => d.Donors.Any(
                a => a.DonorID == id

                )).ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            Departments.ForEach(d => DepartmentDtos.Add(new DepartmentDto()
            {
                DepartmentID = d.DepartmentID,
                DepartmentName = d.DepartmentName

                


            }));


            return DepartmentDtos;
        }

        // GET: api/DepartmentData/FindDepartment/5
        [ResponseType(typeof(Department))]
        [HttpGet]
        public IHttpActionResult FindDepartment(int id)
        {
            Department Department = db.Departments.Find(id);
            DepartmentDto DepartmentDto = new DepartmentDto()
            {
                DepartmentID = Department.DepartmentID,
                DepartmentName = Department.DepartmentName,
                Service = Department.Service,
                //FirstName = Department.Donor.FirstName,
                //LastName = Department.Donor.LastName,
                //DoctorFirstname = Department.Doctor.DoctorFirstname,
                //DoctorLastname = Department.Doctor.DoctorLastname
            };


            return Ok(DepartmentDto);
        }

        // POST: api/DepartmentData/UpdateDepartment/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentID)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/DepartmentData/AddDepartment
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult AddDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentID }, department);
        }

        // POST: api/DepartmentData/DeleteDepartment/5
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentID == id) > 0;
        }
    }
}