using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace test.ModelViews
{
    public class CreateVaccinationViewModel
    {
        public int MedicalRecordId { get; set; }

        [Display(Name = "Does the animal need vaccines?")]
        public bool NeedsVaccines { get; set; }

        public List<string> VaccineNames { get; set; } = new List<string>();
    }
}
