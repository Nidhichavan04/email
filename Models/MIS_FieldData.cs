using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Email.Models
{
    class MIS_FieldData
    {
        [Key]
        public int DataId { get; set; }
        public int  ReportId { get; set; }
        public int FieldId { get; set; }
        public string FieldValue { get; set; }
        public string Remarks { get; set; }
        public int EntryBy { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
    }
}
