using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hospital_CMS.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        public string DoctorFirstname { get; set; }

        public string DoctorLastname { get; set; }

        public string Gender { get; set; }

        public Int64 LicenceNo { get; set; }

        public Int64 ContactNo { get; set; }

        [ForeignKey("Specilization")]
        public int SpecilizationId { get; set; }

        public virtual Specilization Specilization { get; set; }
    }
    //dto

    public class DoctorDto
    {
        public int DoctorId { get; set; }

        public string DoctorFirstname { get; set; }

        public string DoctorLastname { get; set; }

        public string Gender { get; set; }

        public Int64 LicenceNo { get; set; }

        public Int64 ContactNo { get; set; }

        public int SpecilizationId { get; set; }

       // public string SpecilizationName { get; set; }
    }

}
