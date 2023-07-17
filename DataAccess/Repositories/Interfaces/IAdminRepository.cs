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
        Task<List<AppUsers>> GetAppUsers();
        Task<List<EnrollMents>> GetEnrollMents(int userId, int groupId, bool isActive);
        Task<int> GetEnrollMentsCount(int groupId);
        Task<List<UserPayments>> GetAuctionDetails(int groupId, int userId);
        Task<int> AddAppUsers(AppUsers appUsers);
        Task<List<RegisteUsers>> GetUsers(int userId, bool isActive);
        Task<int> AddChitPlan(ChitPlans chitPlans);
        Task<int> EnrollMent(int userId, int groupId, DateTime enrollmentDate, bool isActive);
        Task<int> UserRegistration(RegisteUsers registeUsers);
        Task<int> CheckUserExist(string phone);
        Task<int> AuctionDetailsByGroup(GroupWiseDetails groupWiseDetails);
        Task<int> UserPayments(UserPayments userPayments);
        Task<List<UserPayments>> UserOutStandings(int groupId , int userId);
        Task<int> AddAuctionDetails(AuctionDetails auctionDetails);
        Task<UserProfile> ValidateUser(string userName, string password);
        Task<int> ValidateGroup(string groupName);
        Task<int> GetPendingPayments(int userId, int groupId);
        Task<List<AuctionCreation>> GetCreateAuction(int groupId);
        Task<int> CreateAuction(AuctionCreation auctionCreation);
        Task<int> SaveAuctionDetails(SaveAuctionDetails saveAuctionDetails);
    }
}
