using Antlr.Runtime.Tree;
using Hospital_CMS.Models;
using Hospital_CMS.Models.ViewModels;
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
    public class DepartmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DepartmentController()

        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44370/api/");
        }
        // GET: Department/List

        public ActionResult List()
        {

            //Objective: Communicate with our department data api to retrieve a list of department
            //Curl https://localhost:44370/api/treedata/listtrees
            //HttpClient client = new HttpClient();
            string url = "departmentdata/listdepartments";
            //string url = "https://localhost:44307/api/treedata/listtrees";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine("The respose code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            //Debug.WriteLine("Number of department received : ");
            //Debug.WriteLine(departments.Count());


            return View(departments);
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            DetailsDepartments ViewModel = new DetailsDepartments();
            //Objective: Communicate with our tree data api to retrieve one department
            //Curl https://localhost:44370/api/departmentdata/finddepartment/{id}


            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine("The respose code is ");
            //Debug.WriteLine(response.StatusCode);

            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            ViewModel.SelectedDepartment = SelectedDepartment;

           url = "donordata/ListDonorsForDepartment/" + id;
           response = client.GetAsync(url).Result;
           IEnumerable<DonorDto> ResponsibleDonors = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;

            ViewModel.ResponsibleDonors = ResponsibleDonors;


            return View(ViewModel);
        }


        public ActionResult Error()
        {
            return View();
        }
        // GET: Department/New
        public ActionResult New()
        {
           
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        public ActionResult Create(Department department)
        {
            //Debug.WriteLine("The inputted tre name is : ");
            //Debug.WriteLine(tree.TreeName);
            //Objective: Add a new tree into our system using the api
            //Curl -H "Content-type:application.json" -d @tree.json https://localhost:44370/api/treedata/adddepartment

            string url = "departmentdata/adddepartment";

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(department);
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

        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine("The respose code is ");
            //Debug.WriteLine(response.StatusCode);

            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
            //Debug.WriteLine("Tree received : ");
            //Debug.WriteLine(selectedtree.TreeName);
           
            return View(SelectedDepartment);
        }

        // POST: Department/Update/5
        [HttpPost]
        public ActionResult Update(int id, Department department)
        {
            //Debug.WriteLine("The inputted department name is : ");
            //Debug.WriteLine(department.DepartmentName);
            //Objective: Add a new department into our system using the api
            //Curl -H "Content-type:application.json" -d @tree.json https://localhost:44370/api/departmentdata/updatedepartment

            string url = "departmentdata/updatedepartment/" + id;

            //JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(department);
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

        // GET: Department/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto selecteddepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            return View(selecteddepartment);
        }

        // POST: Department/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Department department)
        {
            string url = "departmentdata/deletedepartment/" + id;


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
