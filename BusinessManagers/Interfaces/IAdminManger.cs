using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManagers.Interfaces
{
    public interface IAdminManger
    {
        Task<AdminProfile> GetAdminProfile();
        Task<List<RegisteUsers>> GetUsers(int userId, bool isActive);
        Task<List<EnrollMents>> GetEnrollMents(int userId, int groupId, bool isActive);
        Task<List<UserPayments>> GetAuctionDetails(int groupId);
        Task<List<AppUsers>> GetAppUsers();
        Task<int> AddAppUsers(AppUsers appUsers);
        Task<int> AddChitPlan(ChitPlans chitPlans);
        Task<int> EnrollMent(int userId, int groupId, DateTime enrollmentDate, bool isActive);
        Task<int> UserRegistration(RegisteUsers registeUsers);
        Task<int> AuctionDetailsByGroup(GroupWiseDetails groupWiseDetails);
        Task<int> UserPayments(UserPayments userPayments);
        Task<List<UserPayments>> UserOutStandings(int groupId);
        Task<int> AddAuctionDetails(AuctionDetails auctionDetails);
    }
}
