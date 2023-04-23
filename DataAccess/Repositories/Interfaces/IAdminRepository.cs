using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<AdminProfile> GetAdminProfile();
        Task<int> AddChitPlan(ChitPlans chitPlans);
        Task<int> EnrollMent(int userId, int GroupId, string enrolementDate, bool groupStatus);
        Task<int> UserRegistration(RegisteUsers registeUsers);
        Task<int> AuctionDetailsByGroup(GroupWiseDetails groupWiseDetails);
        Task<int> UserPayments(UserPayments userPayments);
        Task<int> AddAuctionDetails(AuctionDetails auctionDetails);
    }
}
