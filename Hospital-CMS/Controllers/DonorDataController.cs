﻿using System;
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
    public class DonorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DonorData/ListDonors
        [HttpGet]
        public IEnumerable<DonorDto> ListDonors()
        {
            List<Donor> donors = db.Donors.ToList();
            List<DonorDto> DonorDtos = new List<DonorDto>();

            donors.ForEach(d => DonorDtos.Add(new DonorDto()

            {
                DonorID = d.DonorID,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Contact = d.Contact,
                Address = d.Address,
                Value = d.Value

            }));


            return DonorDtos;
        }
        // GET: api/DonorData/ListDonorsForDepartment/1
        [HttpGet]
        public IEnumerable<DonorDto> ListDonorsForDepartment(int id)
        {
            List<Donor> Donors = db.Donors.Where(
                a => a.Departments.Any(
                    d => d.DepartmentID == id)

                ).ToList();
            List<DonorDto> DonorDtos = new List<DonorDto>();

            Donors.ForEach(a => DonorDtos.Add(new DonorDto()
            {
                DonorID = a.DonorID,
                FirstName = a.FirstName,
                LastName = a.LastName

            }));


            return DonorDtos;
        }


        // GET: api/DonorData/ListDonorsNotCaringForDepartment/1
        [HttpGet]
        public IEnumerable<DonorDto> ListDonorsNotCaringForDepartment(int id)
        {
            List<Donor> Donors = db.Donors.Where(
                a => !a.Departments.Any(
                    d => d.DepartmentID == id)

                ).ToList();
            List<DonorDto> DonorDtos = new List<DonorDto>();

            Donors.ForEach(a => DonorDtos.Add(new DonorDto()
            {
                DonorID = a.DonorID,
                FirstName = a.FirstName,
                LastName = a.LastName

            }));


            return DonorDtos;
        }

        // GET: api/DonorData/FindDonor/5
        [ResponseType(typeof(Donor))]
        [HttpGet]
        public IHttpActionResult FindDonor(int id)
        {
            Donor Donor = db.Donors.Find(id);
            DonorDto DonorDto = new DonorDto()
            {
                DonorID = Donor.DonorID,
                FirstName = Donor.FirstName,
                LastName = Donor.LastName,
                Contact = Donor.Contact,
                Address = Donor.Address,
                Value = Donor.Value
            };


            return Ok(DonorDto);
        }

        // POST: api/DonorData/UpdateDonor/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDonor(int id, Donor donor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donor.DonorID)
            {
                return BadRequest();
            }

            db.Entry(donor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonorExists(id))
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

        // POST: api/DonorData/AddDonor
        [ResponseType(typeof(Donor))]
        [HttpPost]
        public IHttpActionResult AddDonor(Donor donor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donors.Add(donor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donor.DonorID }, donor);
        }

        // DELETE: api/DonorData/DeleteDonor/5
        [ResponseType(typeof(Donor))]
        [HttpPost]
        public IHttpActionResult DeleteDonor(int id)
        {
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }

            db.Donors.Remove(donor);
            db.SaveChanges();

            return Ok(donor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DonorExists(int id)
        {
            return db.Donors.Count(e => e.DonorID == id) > 0;
        }
    }
}