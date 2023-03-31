using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Hospital_CMS.Models.ViewModels;
using System.Web.Script.Serialization;
using Hospital_CMS.Models;

namespace Hospital_CMS.Controllers
{
    public class DoctorController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static DoctorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44370/api/");
        }
        // GET: Doctor/List
        public ActionResult List()
        {
            //Objective: communicate with our doctor data api to retrive the list of doctor
            //curl https://localhost:44370/api/DoctorData/ListDoctors
          
            string url = "DoctorData/ListDoctors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The Response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DoctorDto> doctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;
            //Debug.WriteLine("No of doctor recieved");
            //Debug.WriteLine(doctors.Count());

            return View(doctors);
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            //Objective: communicate with our Doctor data api to retrive one Doctor
            //curl  https://localhost:44370/api/DoctorData/FindDoctor/{id}

            
            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The Response code is ");
            Debug.WriteLine(response.StatusCode);

            DoctorDto selecteddoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            Debug.WriteLine("doctor recieved");
            Debug.WriteLine(selecteddoctor.DoctorFirstname);


            return View(selecteddoctor);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Doctor/New
        public ActionResult New()
        {

            //information all doctoe in the system
            //GET: api/DoctorData/ListDoctors

            string url = "SpecilizationData/ListSpecilization";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<SpecilizationDto> SpeciliztionsOptions = response.Content.ReadAsAsync<IEnumerable<SpecilizationDto>>().Result;


            return View(SpeciliztionsOptions);
        }

        // POST: Doctor/Create
        [HttpPost]
        public ActionResult Create(Doctor doctor)
        {
            Debug.WriteLine("the inputted doctor jsonpayload is:");
            Debug.WriteLine(doctor.DoctorFirstname);

            //Objective add a new doctor into our system using API
            //curl -d @doctor.json -H "Content-type:application/json" https://localhost:44370/api/DoctorData/AddDoctor
            string url = "DoctorData/AddDoctor";

           
            string jsonpayload = jss.Serialize(doctor);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
            
        }

        // GET: Doctor/Edit/5
        public ActionResult Edit(int id)
        {

            UpdateDoctor ViewModel = new UpdateDoctor();


            //existing information
            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto SelectedDoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            ViewModel.SelectedDoctor = SelectedDoctor;



            //also like to include all specilization to choose from when updating doctor

            //existing information
            url = "SpecilizationData/ListSpecilization/";
            response = client.GetAsync(url).Result;
            IEnumerable<SpecilizationDto> SpecilizationOption = response.Content.ReadAsAsync<IEnumerable<SpecilizationDto>>().Result;

            ViewModel.SpecilizationOption = SpecilizationOption;

            return View(ViewModel);
        }

        // POST: Doctor/Update/5
        [HttpPost]
        public ActionResult Update(int id, Doctor doctor)
        {
            string url = "DoctorData/UpdateDoctor/" + id;
            string jsonpayload = jss.Serialize(doctor);
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

        // GET: Doctor/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DoctorData/FindDoctor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDto selecteddoctor = response.Content.ReadAsAsync<DoctorDto>().Result;
            return View(selecteddoctor);

        }

        // POST: Doctor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "DoctorData/DeleteDoctor/" + id;
            
            HttpContent content = new StringContent("");
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
    }
}
