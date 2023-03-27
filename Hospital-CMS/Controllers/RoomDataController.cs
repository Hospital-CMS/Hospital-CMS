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
    public class RoomDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RoomData/ListRooms
        //curl https://localhost:44370/api/roomdata/listrooms
        [HttpGet]
        public IEnumerable<RoomDto> ListRooms()
        {
            List<Room> Rooms = db.Rooms.ToList();
            List<RoomDto> RoomDtos = new List<RoomDto>();

            Rooms.ForEach(r => RoomDtos.Add(new RoomDto()
            {
                RoomId = r.RoomId,
                RoomNo = r.RoomNo,
                RoomType = r.RoomType
            }));

            return RoomDtos;
        }

        // GET: api/RoomData/FindRoom/5
        //curl https://localhost:44370/api/roomdata/findroom/2
        [ResponseType(typeof(Room))]
        [HttpGet]
        public IHttpActionResult FindRoom(int id)
        {
            Room Room = db.Rooms.Find(id);
            RoomDto RoomDto = new RoomDto()
            {
                RoomId = Room.RoomId,
                RoomNo = Room.RoomNo,
                RoomType = Room.RoomType
            };
            if (Room == null)
            {
                return NotFound();
            }

            return Ok(RoomDto);
        }

        // POST: api/RoomData/UpdateRoom/5
        //curl -d @room.json -H "Content-type:application/json" "https://localhost:44370/api/roomdata/updateroom/3"
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRoom(int id, Room room)
        {
            Debug.WriteLine("I have reached the update room method!");
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model state is invalid");
                return BadRequest(ModelState);
            }

            if (id != room.RoomId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + room.RoomId);
                Debug.WriteLine("POST parameter" + room.RoomId);
                Debug.WriteLine("POST parameter" + room.RoomType);
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
                {
                    Debug.WriteLine("Room not found");
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

        // POST: api/RoomData/AddRoom
        //curl -d @room.json -H "Content-type:application/json" https://localhost:44370/api/roomdata/addroom
        [ResponseType(typeof(Room))]
        [HttpPost]
        public IHttpActionResult AddRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = room.RoomId }, room);
        }

        // POST : api/RoomData/DeleteRoom/5
        //curl -d "" https://localhost:44370/api/roomdata/deleteroom/2
        [ResponseType(typeof(Room))]
        [HttpPost]
        public IHttpActionResult DeleteRoom(int id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            db.Rooms.Remove(room);
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

        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.RoomId == id) > 0;
        }
    }
}