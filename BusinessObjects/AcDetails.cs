using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class AcDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMonth { get; set; }
        public int TotalAmount { get; set; }
        public int DueAmount { get; set; }
    }
}
