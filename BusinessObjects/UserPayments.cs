using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UserPayments
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int CurrentMonthEmi { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public DateTime NextAuctionDate { get; set; }
        public int PaidUpto { get; set; }
        public int Dividend { get; set; }
        public int TotalAmount { get; set; }
        public int DueAmount { get; set; }
        public DateTime AuctionDate { get; set; }
        public bool FullyPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMonth { get; set; }
        public bool Raised { get; set; }
    }
}
