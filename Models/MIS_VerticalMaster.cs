﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Models
{
    
    class MIS_VerticalMaster
    {
        [Key]
        public int VerticalId { get; set; }

        public int SiteinchargeId { get; set; }
        public string VerticalName { get; set; }
    }
}
