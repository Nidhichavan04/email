using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Email.Models
{
     class MIS_Users
    {
        [Key]
        public int UserId  { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
}
