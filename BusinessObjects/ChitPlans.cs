﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
     public class ChitPlans
     {
        public string GroupName { get; set; }
        public int Amount { get; set; }
        public int Duration { get; set; }
        public int InstallmentAmount { get; set; }
        public int NoOfMembers { get; set; }
        public bool Existed { get; set; }
        public DateTime StartDate { get; set; }
        public int MembersInCircle { get; set; }
    }
}
