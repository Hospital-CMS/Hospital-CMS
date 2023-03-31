using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_CMS.Models.ViewModels
{
    public class DetailsSpecilization
    {
        public SpecilizationDto SelectedSpecilization { get; set; }

        public IEnumerable<DoctorDto> RelatedDoctor { get; set; }
    }
}