using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_CMS.Models.ViewModels
{
    public class UpdateDoctor
    {
        //The viewmodel is class which store data information that we need to present to /Doctor/Update/{}


        //the exisiting information

       public  DoctorDto SelectedDoctor { get; set; }

        //all specilization to choose from when updating this doctor

        public IEnumerable<SpecilizationDto> SpecilizationOption { get;set; }
    }
}