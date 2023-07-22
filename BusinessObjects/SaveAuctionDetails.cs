using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class SaveAuctionDetails
    {
        public int CurrentAuctionId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int NoOfMembers { get; set; }
        public DateTime AuctionDate { get; set; }
        //public DateTime NextAuctionDate { get; set; }
        public int GroupValue { get; set; }
        public int AuctionAmount { get; set; }
        //public int AmountToBePaid { get; set; }
        public int AuctionMonth { get; set; }
    }
}
