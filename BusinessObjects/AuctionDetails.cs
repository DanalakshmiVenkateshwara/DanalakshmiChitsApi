using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class AuctionDetails
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string AuctionDate { get; set; }
        public long AuctionAmount { get; set; }
        public long Dividend { get; set; }
    }
}
