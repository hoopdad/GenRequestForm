using System.ComponentModel.DataAnnotations;
using System;

namespace GenReq.Models
{
    public enum RegistrationTypes
    {
        [Display(Name = "None")]
        Unassigned,
        [Display(Name = "Free Trial - 5 Pack")]
        FreeTrial5 = 1
    }
    public class UserRegistration
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Plan Type")]
        public RegistrationTypes RegistrationType { get; set; } = RegistrationTypes.FreeTrial5;

        [DataType(DataType.Date)]
        [Display(Name = "Plan Started")]
        public DateTime RegistrationStarted { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Plan Ended")]
        public DateOnly? RegistrationEnded { get; set; }
    }
}
