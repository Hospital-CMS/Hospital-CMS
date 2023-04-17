using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_CMS.Models.ViewModels
{
    public class DetailsDepartments
    {
        public DepartmentDto SelectedDepartment { get; set; }

        public IEnumerable<DonorDto> ResponsibleDonors { get; set; }

        public IEnumerable<DonorDto> AvailableDonors { get; set; }
    }
}