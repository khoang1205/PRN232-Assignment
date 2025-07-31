using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232_Assignment.AppointmentService.Grpc.DTO;

namespace PRN232_Assignment.UserService.Service.Models
{
    public class DoctorResponseWrapper
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public List<DoctorDto> Items { get; set; }
    }

}
