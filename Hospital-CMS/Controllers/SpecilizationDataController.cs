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
    public class SpecilizationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Returns all specilization in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Specilization in the database, including their doctor.
        /// </returns>
        /// <example>
        /// GET: api/SpecilizationData/ListSpecilization
        /// </example>

        // GET: api/SpecilizationData/ListSpecilization
        //curl https://localhost:44370/api/SpecilizationData/ListSpecilization
        [HttpGet]
        public IEnumerable<SpecilizationDto> ListSpecilization()
        {
            List<Specilization> Specilizations= db.Specilization.ToList();
            List<SpecilizationDto> SpecilizationDtos = new List<SpecilizationDto>();

            Specilizations.ForEach(a => SpecilizationDtos.Add(new SpecilizationDto()
            {
                SpecilizationId =a.SpecilizationId,
                SpecilizationName=a.SpecilizationName
            }));

            return SpecilizationDtos;
        }

        /// <summary>
        /// Gather information about all Specilization realated to particular doctor id
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Specilization in the database, including their doctor matched with a particular specilization ID </returns>
        ///  <param name="id">Specilization ID.</param>
        /// <example>
        /// GET: api/SpecilizationData/ListSpecilization
        /// </example>


        // GET: api/SpecilizationData/ListSpecilization
        //curl https://localhost:44370/api/SpecilizationData/ListSpecilization
        [HttpGet]
        [ResponseType(typeof(SpecilizationDto))]
        public IEnumerable<SpecilizationDto> ListSpecilizationForDoctor()
        {
            List<Specilization> Specilizations = db.Specilization.ToList();
            List<SpecilizationDto> SpecilizationDtos = new List<SpecilizationDto>();

            Specilizations.ForEach(a => SpecilizationDtos.Add(new SpecilizationDto()
            {
                SpecilizationId = a.SpecilizationId,
                SpecilizationName = a.SpecilizationName
            }));

            return SpecilizationDtos;
        }

        /// <summary>
        /// Returns all specilization in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An specilization in the system matching up to the doctor ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The specilization key of the doctor</param>
        /// <example>
        /// GET: api/SpecilizationData/FindSpecilization/5
        /// </example>
        


        // GET: api/SpecilizationData/FindSpecilization/5
        //curl "https://localhost:44370/api/SpecilizationData/FindSpecilization/1"
        [ResponseType(typeof(Specilization))]
        [HttpGet]

        public IHttpActionResult FindSpecilization(int id)
        {
            Specilization Specilization = db.Specilization.Find(id);
            SpecilizationDto SpecilizationDto = new SpecilizationDto()
            {
                SpecilizationId= Specilization.SpecilizationId,
                SpecilizationName =Specilization.SpecilizationName
            };
            if (Specilization == null)
            {
                return NotFound();
            }

            return Ok(SpecilizationDto);
        }


        /// <summary>
        /// Updates a particular specilization in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the specilization ID primary key</param>
        /// <param name="specilization">JSON FORM DATA of an specilization</sparam>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/SpecilizationData/UpdateSpecilization/5
        /// FORM DATA: Specilization JSON Object
        /// </example>

        // POST: api/SpecilizationData/UpdateSpecilization/5
        //curl -d @specilization.json -H "Content-type:application/json" "https://localhost:44370/api/SpecilizationData/UpdateSpecilization/3"
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSpecilization(int id, Specilization specilization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != specilization.SpecilizationId)
            {
                return BadRequest();
            }

            db.Entry(specilization).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecilizationExists(id))
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

        /// <summary>
        /// Adds an specilization to the system
        /// </summary>
        /// <param name="specilization">JSON FORM DATA of an specilization</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: specilization ID, specilization Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/SpecilizationData/AddSpecilization
        /// FORM DATA: specilization JSON Object
        /// </example>

        // POST: api/SpecilizationData/AddSpecilization
        //curl -d @specilization.json -H "Content-type:application/json" https://localhost:44370/api/SpecilizationData/AddSpecilization
        [ResponseType(typeof(Specilization))]
        [HttpPost]
        public IHttpActionResult AddSpecilization(Specilization specilization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Specilization.Add(specilization);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = specilization.SpecilizationId }, specilization);
        }

        /// <summary>
        /// Deletes an specilization from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the specilization</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/SpecilizationData/DeleteSpecilization/5
        /// FORM DATA: (empty)
        /// </example>

        // POST: api/SpecilizationData/DeleteSpecilization/5
        //curl -d "" https://localhost:44370/api/SpecilizationData/DeleteSpecilization/3

        [ResponseType(typeof(Specilization))]
        [HttpPost]
        public IHttpActionResult DeleteSpecilization(int id)
        {
            Specilization specilization = db.Specilization.Find(id);
            if (specilization == null)
            {
                return NotFound();
            }

            db.Specilization.Remove(specilization);
            db.SaveChanges();

            return Ok(specilization);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpecilizationExists(int id)
        {
            return db.Specilization.Count(e => e.SpecilizationId == id) > 0;
        }
    }
}