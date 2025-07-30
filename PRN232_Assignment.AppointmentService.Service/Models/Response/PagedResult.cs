using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Assignment.DoctorService.Service.Models.Response
{
    public class PagedResult<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public List<T> Items { get; set; } = new();
    }
}
