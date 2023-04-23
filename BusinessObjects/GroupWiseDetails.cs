using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class GroupWiseDetails
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string AuctionDate { get; set; }
        public long AuctionAmount { get; set; }
        public long AuctionToBePaid { get; set; }
        public int NoOfMonthsCompleted { get; set; }
        public string DueDate { get; set; }
        public bool Status { get; set; }
    }
}
