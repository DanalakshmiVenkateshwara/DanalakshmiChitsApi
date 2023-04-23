using BusinessObjects;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AdminRepository : RepositoryBase, IAdminRepository
    {
        public AdminRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<AdminProfile> GetAdminProfile()
        {
            return await this.Find<AdminProfile>(SqlQueries.GET_ADMIN_PROFILE);
        }
        public async Task<int> AddChitPlan(ChitPlans chitPlans)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.Add_ChitPlan, new
            {
                chitPlans.GroupId,
                chitPlans.GroupName,
                chitPlans.ChitAmount,
                chitPlans.Duration,
                chitPlans.InstallmentAmount,
                chitPlans.NoOfMembers,
                chitPlans.Existed,
                chitPlans.StartDate,
                chitPlans.MembersInCircle
            });
        }
        public async Task<int> EnrollMent(int userId, int groupId, string enrolementDate, bool groupStatus)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.EnrollMent, new
            {
                userId,
                groupId,
                enrolementDate,
                groupStatus,
            });
        }
        public async Task<int> UserRegistration(RegisteUsers registeUsers)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.RegisteUsers, new
            {
                registeUsers.Name,
                registeUsers.Phone,
                registeUsers.EMail,
                registeUsers.Password,
                registeUsers.Address,
                registeUsers.City,
                registeUsers.State
            });
        }
        public async Task<int> AuctionDetailsByGroup(GroupWiseDetails groupWiseDetails)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.AuctionDetailsByGroup, new
            {
                groupWiseDetails.UserId,
                groupWiseDetails.GroupId,
                groupWiseDetails.AuctionDate,
                groupWiseDetails.AuctionAmount,
                groupWiseDetails.AuctionToBePaid,
                groupWiseDetails.NoOfMonthsCompleted,
                groupWiseDetails.DueDate,
                groupWiseDetails.Status
            });
        }
        public async Task<int> UserPayments(UserPayments userPayments)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.UserPayments, new
            {
                userPayments.UserId,
                userPayments.GroupId,
                userPayments.CurrentMonthEmi,
                userPayments.Divident,
                userPayments.TotalAmount,
                userPayments.DueAmount,
                userPayments.AuctionDate,
                userPayments.PaymentDate,
                userPayments.FullyPiad,
                userPayments.PaymentMonth,
                userPayments.Raised
            });
        }
        public async Task<int> AddAuctionDetails(AuctionDetails auctionDetails)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.Add_Auction_Details, new
            {
                auctionDetails.UserId,
                auctionDetails.GroupId,
                auctionDetails.AuctionAmount,
                auctionDetails.AuctionDate,
                auctionDetails.Dividend
            });
        }
    }
}