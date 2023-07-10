using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Models
{
    class MIS_ProjectMaster
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int VerticalId { get; set; }
        public int ProjectManagerId { get; set; }
        public bool IsActive { get; set; }
    }
}
