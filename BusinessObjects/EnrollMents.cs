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
        public int Duration { get; set; }
        public string GroupName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime NextAuctionDate { get; set; }
        public string PaidUpto { get; set; }
        public string TotalInstallMents { get; set; }
        public bool UserChitSatus { get; set; }
        public int Amount { get; set; }
        public DateTime EnrollMentDate { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public DateTime CloseDate { get; set; }
    }
}
