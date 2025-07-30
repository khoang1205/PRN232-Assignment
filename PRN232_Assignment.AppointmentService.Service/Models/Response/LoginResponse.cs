using PRN232_Assignment.DoctorService.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Assignment.DoctorService.Service.Models.Response
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
    }
}
