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
        public async Task<List<AppUsers>> GetAppUsers()
        {

            return await this.All<AppUsers>(SqlQueries.GET_APP_USERS);
        }
        public async Task<List<RegisteUsers>> GetUsers(int userId, bool IsActive)
        {
            if(userId > 0)
                return await this.All<RegisteUsers>(SqlQueries.Get_Users_By_Id, new { userId });
            else
            return await this.All<RegisteUsers>(SqlQueries.Get_All_Users, new { isActive=  IsActive });
        }
        public async Task<List<EnrollMents>> GetEnrollMents(int userId, int groupId)
        {
            if (userId > 0 && groupId > 0) //it is used for my chits in user
                return await this.All<EnrollMents>(SqlQueries.Get_EnrollMents_By_UserId_GroupId, new { groupId, userId });
            else if (groupId > 0)
                return await this.All<EnrollMents>(SqlQueries.Get_EnrollMents_By_GroupId, new { groupId });
            else
                return await this.All<EnrollMents>(SqlQueries.Get_All_EnrollMents);
        }
        public async Task<List<UserPayments>>  GetAuctionDetails(int groupId)
        {
            return await this.All<UserPayments>(SqlQueries.Get_AuctionDetails_By_GroupId, new { groupId });
        }
        public async Task<int> AddAppUsers(AppUsers appUsers)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.Add_APP_USER, new
            {
                Name = appUsers.Name,
                Phone = appUsers.Phone,
                State = appUsers.State,
                Date = DateTime.Now,
            });
        }
        public async Task<int> AddChitPlan(ChitPlans chitPlans)
        {
            if (!chitPlans.Existed && !chitPlans.IsDelete)
                return await this.AddOrUpdateDynamic(SqlQueries.Update_ChitPlan, new {GroupId = chitPlans.Id, Existed = true , StartDate = DateTime.Now});
            else if (!chitPlans.GroupClosed && chitPlans.Existed || chitPlans.IsDelete)
                return await this.AddOrUpdateDynamic(SqlQueries.Closed_ChitPlan, new { GroupId = chitPlans.Id, IsDelete = chitPlans.IsDelete, GroupClosed = true, EndDate = DateTime.Now });
            else { 
            return await this.AddOrUpdateDynamic(SqlQueries.Add_ChitPlan, new
            {
                GroupName= chitPlans.GroupName,
                Amount = chitPlans.Amount,
                Duration =chitPlans.Duration,
                InstallmentAmount =chitPlans.InstallmentAmount,
                NoOfMembers = chitPlans.NoOfMembers,
                Existed = false,
                StartDate = DateTime.Now,
                MembersInCircle = chitPlans.MembersInCircle
            });
            }
        }
        public async Task<int> EnrollMent(int userId, int groupId, DateTime enrollmentDate)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.EnrollMent, new
            {
                userId,
                groupId,
                enrollmentDate
            });
        }
        public async Task<int> UserRegistration(RegisteUsers registeUsers)
        {
            if (!registeUsers.IsActive)
                return await this.AddOrUpdateDynamic(SqlQueries.DeleteUsers_ById, new { userId =registeUsers.Id });
            else if (registeUsers.Id > 0)
            {
                return await this.AddOrUpdateDynamic(SqlQueries.UpdateUsers_ById, new
                {
                    registeUsers.Id,
                    registeUsers.Name,
                    registeUsers.Phone,
                    registeUsers.EMail,
                    registeUsers.Password,
                    registeUsers.Address,
                    registeUsers.City,
                    registeUsers.State,
                    registeUsers.Aadhar,
                    Date = DateTime.Now
                });
            }
            else {
                return await this.AddOrUpdateDynamic(SqlQueries.RegisteUsers, new
                {
                    registeUsers.Name,
                    registeUsers.Phone,
                    registeUsers.EMail,
                    registeUsers.Password,
                    registeUsers.Address,
                    registeUsers.City,
                    registeUsers.State,
                    registeUsers.Aadhar,
                    Date = DateTime.Now
                });
            }
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
                
                UserId = userPayments.UserId,
                GroupId = userPayments.GroupId,
                CurrentMonthEmi =userPayments.CurrentMonthEmi,
                Dividend =userPayments.Dividend,
                TotalAmount =userPayments.TotalAmount,
                DueAmount = userPayments.DueAmount,
                //userPayments.DueAmount,
                //AuctionDate =userPayments.AuctionDate,
                AuctionDate = DateTime.Now,
                PaymentDate = DateTime.Now,
                //userPayments.PaymentDate,
                FullyPaid =userPayments.FullyPaid,
                PaymentMonth =userPayments.PaymentMonth,
                Raised =userPayments.Raised
            });
        }
        //we need to chnage the query and table 
        public async Task<List<UserPayments>> UserOutStandings(int groupId)
        {
            if (groupId > 0)
                return await this.All<UserPayments>(SqlQueries.Get_UserOutStandings_By_GroupId, new { groupId });
            else
                return await this.All<UserPayments>(SqlQueries.Get_All_UserOutStandings);
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