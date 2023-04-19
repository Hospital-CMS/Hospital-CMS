using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospital_CMS.Controllers
{
    public class AppointmentController : Controller
    {
         private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static AppointmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new url("https://localhost:44370/api/");
        }


        // GET: Appointment/List
        public ActionResult List()
        {
             string url = "appointmentdata/listappointments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AppointmentDto> appointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
            return View(appointments);
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
             string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            AppointmentDto selectedappointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
            Debug.WriteLine("appointment booked : ");
            Debug.WriteLine(selectedappointment.Appointment);

            return View(selectedappointment);
        }

        // GET: Appointment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
             string url = "appointmentdata/addappointment";

            
            string jsonpayload = jss.Serialize(appointment);

            

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
              

        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
           string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto selectedappointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            return View(selectedappointment);
        }

        // POST: Appointment/Update/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
             string url = "appointmentdata/updateappointment/" + id;
            string jsonpayload = jss.Serialize(room);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        }

        // GET: Appointment/Delete/5
        public ActionResult Delete(int id)
        {
           string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AppointmentDto selectedappointment = response.Content.ReadAsAsync<AppointmentDto>().Result;

            return View(selectedappointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
