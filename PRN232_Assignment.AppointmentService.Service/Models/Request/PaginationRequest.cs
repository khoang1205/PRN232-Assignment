using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Assignment.DoctorService.Service.Models.Request
{
    public class PaginationRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
