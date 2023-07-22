using BusinessObjects;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public async Task<List<EnrollMents>> GetEnrollMents(int userId, int groupId, bool isActive)
        {
            if (userId > 0) //it is used for my chits in user
            {
                List<EnrollMents> myMhits = new List<EnrollMents>();
                  myMhits =  await this.All<EnrollMents>(SqlQueries.Get_EnrollMents_By_UserId, new { userId });
                if (myMhits.Count > 0)
                {
                    foreach (var item in myMhits)
                    {
                        item.PaidUpto = await this.FindBy<string>(SqlQueries.Get_RunningMonth_By_GroupId, new {groupId = item.GroupId });
                        
                        var userChitStatus = await this.FindBy<int>(SqlQueries.Get_User_Chit_Status, new { groupId = item.GroupId, userId });
                        if (userChitStatus > 0)
                            item.UserChitSatus = true;
                        else item.UserChitSatus = false;

                        var nextAuctionDate = await this.FindBy<string>(SqlQueries.Get_NextAuctionDate, new { groupId = item.GroupId });
                        if (nextAuctionDate == null)
                            item.NextAuctionDate = item.StartDate.AddDays(30);
                        else
                            item.NextAuctionDate =  DateTime.Parse(nextAuctionDate);
                    }
                }
                return myMhits;
            }
            else if (groupId > 0)
                return await this.All<EnrollMents>(SqlQueries.Get_EnrollMents_By_GroupId, new { groupId });
            else if (groupId == -1)
                return new List<EnrollMents>();
            else
                return await this.All<EnrollMents>(SqlQueries.Get_All_EnrollMents, new { isActive });
        }
        public async Task<int> GetEnrollMentsCount(int groupId)
        {
            return await this.Find<int>(SqlQueries.Get_EnrollMents_Count, new { groupId });
        }
        public async Task<List<UserPayments>>  GetAuctionDetails(int groupId, int userId)
        {
            if (groupId > 0) {
                var patmentDetails = await this.All<UserPayments>(SqlQueries.Get_AuctionDetails_By_GroupId, new { groupId });
                if (patmentDetails.Count > 0)
                {
                    patmentDetails[0].TotalDue = await this.FindBy<int>(SqlQueries.Get_Pending_Payments, new { userId = userId, groupId = groupId });
                    var userDues = await this.All<UserDues>(SqlQueries.Get_UserDues, new { groupId = groupId });
                    if (userDues.Any())
                    {
                        foreach ( var userDue in userDues)
                        {
                            var due = await this.FindBy<string>(SqlQueries.Get_User_MonthWise_Due, new { userId = userId, groupId = groupId, paymentMonth = userDue.Installmentmonth });
                            if (due !=null)
                                userDue.dueAmount = Convert.ToInt32(due);
                        }
                        patmentDetails[0].userDues = userDues.Where(x => x.dueAmount > 0).ToList();
                    }
                }
                return patmentDetails;
            }
            else
                return await this.All<UserPayments>(SqlQueries.Get_AuctionDetails);
        }
        public async Task<int> GetPendingPayments(int userId, int groupId)
        {
            return await this.FindBy<int>(SqlQueries.Get_Pending_Payments, new { userId= userId, groupId= groupId });
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
                StartDate = chitPlans.StartDate,//DateTime.Now,
                MembersInCircle = chitPlans.MembersInCircle
            });
            }
        }
        public async Task<int> EnrollMent(int userId, int groupId, DateTime enrollmentDate, bool isActive)
        {
            if(!isActive)
                return await this.AddOrUpdateDynamic(SqlQueries.Delete_EnrollMent, new
                {
                    groupId = groupId,
                    userId = userId,
                    isActive = isActive
                });
            return await this.AddOrUpdateDynamic(SqlQueries.EnrollMent, new
            {
                userId,
                groupId,
                enrollmentDate,
                isActive = isActive
            });
        }
        public async Task<int> CheckUserExist(string phone)
        {
            return await this.Find<int>(SqlQueries.GET_Check_User_Exist, new { phone = phone });
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
                    name = registeUsers.Name +"-"+ registeUsers.Phone,
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
            var emiAmount = await this.Find<int>(SqlQueries.Get_Emi_Amount, new { UserId = userPayments.UserId, GroupId = userPayments.GroupId, PaymentMonth = userPayments.PaymentMonth });

            if (emiAmount >0) //userPayments.TotalDue > userPayments.DueAmount &&
            {
                return await this.AddOrUpdateDynamic(SqlQueries.UpdateUserPayments, new
                {

                    UserId = userPayments.UserId,
                    GroupId = userPayments.GroupId,
                    DueAmount = userPayments.DueAmount - userPayments.CurrentMonthEmi,
                    CurrentMonthEmi = userPayments.CurrentMonthEmi + Convert.ToInt32(emiAmount),
                    PaymentDate = DateTime.Now,
                    PaymentMonth = userPayments.PaymentMonth
                });
            }
            else
            {
                return await this.AddOrUpdateDynamic(SqlQueries.UserPayments, new
                {

                    UserId = userPayments.UserId,
                    GroupId = userPayments.GroupId,
                    CurrentMonthEmi = userPayments.CurrentMonthEmi,
                    Dividend = userPayments.Dividend,
                    TotalAmount = userPayments.TotalAmount,
                    DueAmount = userPayments.TotalAmount - (userPayments.CurrentMonthEmi + userPayments.Dividend),
                    //userPayments.DueAmount,
                    //AuctionDate =userPayments.AuctionDate,
                    AuctionDate = DateTime.Now,
                    PaymentDate = DateTime.Now,
                    //userPayments.PaymentDate,
                    FullyPaid = userPayments.FullyPaid,
                    PaymentMonth = userPayments.PaymentMonth,
                    Raised = userPayments.Raised
                });
            }
        }
        //we need to chnage the query and table 
        public async Task<List<UserPayments>> UserOutStandings(int groupId, int userId)
        {
            if (groupId > 0)
                return await this.All<UserPayments>(SqlQueries.Get_UserOutStandings_By_GroupId, new { groupId });
            else if (userId > 0)
                return await this.All<UserPayments>(SqlQueries.Get_UserOutStandings_By_UserId, new { userId });
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
        public async Task<UserProfile> ValidateUser (string userName, string password)
        {
            return await this.Find<UserProfile>(SqlQueries.ValidateUser, new { userName = userName, password = password });
        }
        public async Task<int> ValidateGroup(string groupName)
        {
            return await this.Find<int>(SqlQueries.ValidateGroup, new { groupName = groupName});
        }
        public async Task<int> CreateAuction(AuctionCreation auctionCreation)
        {
            return await this.AddOrUpdateDynamic(SqlQueries.Add_Create_Auction_Details, new
            {
                auctionCreation.GroupId,
                auctionCreation.Amount,
                auctionCreation.BaseAmount,
                auctionCreation.StartDate,
                auctionCreation.StartTime,
                auctionCreation.EndTime,
                auctionCreation.AuctionMonth
            });
        }
        public async Task<List<AuctionCreation>> GetCreateAuction(int userId)
        {
            if(userId > 0)
                return await this.All<AuctionCreation>(SqlQueries.Get_Create_Auction_Details_By_User , new { userId });
            else
                return await this.All<AuctionCreation>(SqlQueries.Get_Create_Auction_Details);
        }
        public async Task<int> SaveAuctionDetails(SaveAuctionDetails saveAuctionDetails)
        {
            if(saveAuctionDetails.CurrentAuctionId > 0)
            {
                var currentAuctionStatus = await this.AddOrUpdateDynamic(SqlQueries.Update_Auction_Status, new
                {
                    CurrAuctionId = saveAuctionDetails.CurrentAuctionId
                });
            }
            return await this.AddOrUpdateDynamic(SqlQueries.Save_Auction_Details, new
            {
               groupId = saveAuctionDetails.GroupId,
               userId = saveAuctionDetails.UserId,
               AuctionDate = saveAuctionDetails.AuctionDate.ToString(),
               NextAuctionDate = saveAuctionDetails.AuctionDate.ToString() + 30,
               AuctionAmount = saveAuctionDetails.AuctionAmount,
               AmountToBePaid = saveAuctionDetails.GroupValue- saveAuctionDetails.AuctionAmount,
               NoOFMonthsCompleted = saveAuctionDetails.AuctionMonth,
               Status = true,
               Dividend = saveAuctionDetails.AuctionAmount,
               inStallMentAmount = (saveAuctionDetails.AuctionAmount - ((saveAuctionDetails.GroupValue * 4)/100))/ saveAuctionDetails.NoOfMembers,
            });
        }

    }
}