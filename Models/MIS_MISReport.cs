using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Models
{
  
    class MIS_MISReport
    {
        [Key]
        public int ReportId { get; set; }
        //public DateOnly ReportDate { get; set; }
        public int VerticalId { get; set; }
        public int ProjectId { get; set; }
        public int StatusId { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
    }


}

