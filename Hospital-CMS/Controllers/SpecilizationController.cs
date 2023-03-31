using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hospital_CMS.Models.ViewModels;
using Hospital_CMS.Models;

namespace Hospital_CMS.Controllers
{
    public class SpecilizationController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static SpecilizationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44370/api/");
        }

        // GET: Specilization/List
        public ActionResult List()
        {
            //Objective: communicate with our specilization data api to retrive the list of specilization
            //curl https://localhost:44370/api/SpecilizationData/ListSpecilization

          
            string url = "SpecilizationData/ListSpecilization";
            HttpResponseMessage response = client.GetAsync(url).Result;

           //Debug.WriteLine("The Response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<SpecilizationDto> speciliztions = response.Content.ReadAsAsync<IEnumerable<SpecilizationDto>>().Result;
           // Debug.WriteLine("No of specilization recieved");
            //Debug.WriteLine(speciliztions.Count());

            return View(speciliztions);
        }

        // GET: Specilization/Details/5
        public ActionResult Details(int id)
        {
            //Objective: communicate with our specilization data api to retrive one specilization
            //curl https://localhost:44370/api/SpecilizationData/FindSpecilization/{id}


            DetailsSpecilization ViewModel = new DetailsSpecilization();

           
            string url = "SpecilizationData/FindSpecilization/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The Response code is ");
           // Debug.WriteLine(response.StatusCode);

            SpecilizationDto SelectedSpeciliztion = response.Content.ReadAsAsync<SpecilizationDto>().Result;
            //Debug.WriteLine("specilization recieved");
            //Debug.WriteLine(SelectedSpeciliztion.SpecilizationName);

            ViewModel.SelectedSpecilization = SelectedSpeciliztion;
            //showcase information about doctor related to this specilization
            //send a request to gather information about doctors 

            url = "DoctorData/ListDoctorsForSpecilization" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DoctorDto> RelatedDoctors = response.Content.ReadAsAsync<IEnumerable<DoctorDto>>().Result;
            

            ViewModel.RelatedDoctor = RelatedDoctors;

            return View(ViewModel);
        }
        public ActionResult Error()
        {
            return View();
        }

        // GET: Specilization/New
        public ActionResult New()
        {

            return View();
        }

        // POST: Specilization/Create
        [HttpPost]
        public ActionResult Create(Specilization specilization)
        {
            Debug.WriteLine("the json payload is:");
            Debug.WriteLine(specilization.SpecilizationName);

            //Objective add a new Specilization into our system using API
            // curl -d @specilization.json -H "Content-type:application/json" https://localhost:44370/api/SpecilizationData/AddSpecilization
            string url = "SpecilizationData/AddSpecilization";

            
            string jsonpayload = jss.Serialize(specilization);

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

        // GET: Specilization/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "SpecilizationData/FindSpecilization/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SpecilizationDto selectedspeciliztion = response.Content.ReadAsAsync<SpecilizationDto>().Result;
            return View(selectedspeciliztion);
        }

        // POST: Specilization/Update/5
        [HttpPost]
        public ActionResult Update(int id, Specilization specilization)
        {
            string url = "SpecilizationData/UpdateSpecilization/" + id;
            string jsonpayload = jss.Serialize(specilization);
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

        // GET: Specilization/Delete/5
        public ActionResult DeleteConfirm(int id)
        {

            string url = "SpecilizationData/FindSpecilization/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SpecilizationDto selectedspeciliztion = response.Content.ReadAsAsync<SpecilizationDto>().Result;
            return View(selectedspeciliztion);
        }

        // POST: Specilization/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "SpecilizationData/DeleteSpecilization/" + id;
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
