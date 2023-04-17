using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital_CMS.Models
{
    public class Department
    {
        [Key] 
        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }
        public string Service { get; set; }

       public ICollection<Donor> Donors { get; set; }

    }

    public class DepartmentDto
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Service { get; set; }

        public int DonorID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DoctorId { get; set; }

        public string DoctorFirstname { get; set; }
        public string DoctorLastname { get; set; }
    }
}