using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Hospital_CMS.Models;
using System.Web.Script.Serialization;

namespace Hospital_CMS.Controllers
{
    public class DischargeController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DischargeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Url("https://localhost:44370/api/");
        }

        // GET: Discharge/List
                public ActionResult List()
        {
            string url = "dischargedata/list";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DischargeDto> discharges = response.Content.ReadAsAsync<IEnumerable<DischargeDto>>().Result;

            return View(discharges);
        }

        // GET: Discharge/Details/5
        public ActionResult Details(int id)
        {
            string url = "dischargedata/finddischarge/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            DischargeDto selecteddischarge = response.Content.ReadAsAsync<DischargeDto>().Result;
            Debug.WriteLine("discharge received : ");
            Debug.WriteLine(selecteddischarge.discharg);

            return View(selecteddischarge);
        }

        // GET: Discharge/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discharge/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            Debug.WriteLine("the json payload is");
           
            string url = "dischargedata/dischargelist";

            
            string jsonpayload = jss.Serialize(discharge);

            Debug.WriteLine(jsonpayload);

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
              
        }

        // GET: Discharge/Edit/5
        public ActionResult Edit(int id)
        {
             string url = "dischargedata/list/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DischargeDto selecteddischarge = response.Content.ReadAsAsync<DischargeDto>().Result;

            return View(selecteddischarge);
        }

        // POST: Discharge/Update/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
             string url = "dischargedata/updatedischarge/" + id;
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

        

        // POST: Discharge/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
          string url = "dischargedata/deletedischarge/" + id;
            HttpContent content = new StringContent("");
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
}
