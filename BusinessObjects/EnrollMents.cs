using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class EnrollMents
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime NextAuctionDate { get; set; }
        public string PaidUpto { get; set; }
        public int Amount { get; set; }
        public DateTime EnrollMentDate { get; set; }
        public bool IsActive { get; set; }
    }
}
