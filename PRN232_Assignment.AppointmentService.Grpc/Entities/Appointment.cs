using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Assignment.AppointmentService.Entities
{
    public class Appointment
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PatientId { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public DateTime TimeSlot { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

       
    }
}


