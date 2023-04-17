using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_CMS.Models.ViewModels
{
    public class DetailsDonors
    {
        public DonorDto SelectedDonors { get; set; }

       public IEnumerable<DepartmentDto> KeptDepartments { get; set; }
    }
}