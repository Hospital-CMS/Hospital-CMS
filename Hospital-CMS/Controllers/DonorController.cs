using Hospital_CMS.Migrations;
using Hospital_CMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Hospital_CMS.Controllers
{
    public class DonorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonorController()

        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44370/api/");
        }
        // GET: Donor/List
        public ActionResult List()
        {
            //Objective: Communicate with our department data api to retrieve a list of donor
            //Curl https://localhost:44370/api/treedata/listtrees
            //HttpClient client = new HttpClient();
            string url = "donordata/listdonors";
            //string url = "https://localhost:44307/api/treedata/listtrees";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine("The respose code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DonorDto> donors = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;
            //Debug.WriteLine("Number of donor received : ");
            //Debug.WriteLine(departments.Count());


            return View(donors);
        }

        // GET: Donor/Details/5
        public ActionResult Details(int id)
        {
            //Objective: Communicate with our tree data api to retrieve one department
            //Curl https://localhost:44370/api/departmentdata/finddepartment/{id}


            string url = "donordata/finddonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine("The respose code is ");
            //Debug.WriteLine(response.StatusCode);

            DonorDto SelectedDonor = response.Content.ReadAsAsync<DonorDto>().Result;



            return View(SelectedDonor);
        }

        public ActionResult Error()
        {
            return View();
        }


        // GET: Donor/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Donor/Create
        [HttpPost]
        public ActionResult Create(Donor donor)
        {
            //Debug.WriteLine("The inputted tre name is : ");
            //Debug.WriteLine(tree.TreeName);
            //Objective: Add a new tree into our system using the api
            //Curl -H "Content-type:application.json" -d @tree.json https://localhost:44370/api/donordata/adddonor

            string url = "donordata/adddonor";

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(donor);
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
                return RedirectToAction("Errors");
            }
        }

        // GET: Donor/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "donordata/finddonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine("The respose code is ");
            //Debug.WriteLine(response.StatusCode);

            DonorDto SelectedDonor = response.Content.ReadAsAsync<DonorDto>().Result;
            //Debug.WriteLine("Tree received : ");
            //Debug.WriteLine(selectedtree.TreeName);

            return View(SelectedDonor);
        }

        // POST: Donor/Update/5
        [HttpPost]
        public ActionResult Update(int id, Donor donor)
        {
            //Debug.WriteLine("The inputted tre name is : ");
            //Debug.WriteLine(tree.TreeName);
            //Objective: Add a new tree into our system using the api
            //Curl -H "Content-type:application.json" -d @tree.json https://localhost:44370/api/donordata/updatedonor

            string url = "donordata/updatedonor/" + id;

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(donor);
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
                return RedirectToAction("Errors");
            }
        }

        // GET: Donor/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "donordata/finddonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonorDto selecteddonor = response.Content.ReadAsAsync<DonorDto>().Result;

            return View(selecteddonor);
        }

        // POST: Donor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Donor donor)
        {
            string url = "donordata/deletedonor/" + id;


            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Errors");
            }
        }
    }
}
