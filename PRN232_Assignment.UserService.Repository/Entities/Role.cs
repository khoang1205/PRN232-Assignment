using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Assignment.UserService.Repository.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; } // 1: Doctor, 2: Patient

        [Required]
        public string Name { get; set; }
    }
}
