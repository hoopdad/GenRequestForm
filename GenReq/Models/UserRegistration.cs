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

        [Display(Name = "Registration Type")]
        public RegistrationTypes RegistrationType { get; set; } = RegistrationTypes.Unassigned;

        [DataType(DataType.Date)]
        [Display(Name = "Registration Started")]
        public DateTime RegistrationStarted { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        [Display(Name = "Registration Ended")]
        public DateOnly? RegistrationEnded { get; set; }
    }
}
