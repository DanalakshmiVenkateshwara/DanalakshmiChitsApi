using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UserPayments
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int CurrentMonthEmi { get; set; }
        public int Divident { get; set; }
        public int TotalAmount { get; set; }
        public int DueAmount { get; set; }
        public string AuctionDate { get; set; }
        public bool FullyPiad { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentMonth { get; set; }
        public string Raised { get; set; }
    }
}
