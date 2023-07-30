//using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccess.Repositories
{
    public class UserRepository : RepositoryBase  , IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        //public async Task<int> MemberRegistration(Register register)
        //{
        //    return await this.AddOrUpdateDynamic(SqlQueries.Member_Registration, new { Name = register.Name, Phone = register.Phone, State = register.State});
        //}
        public async Task<UserProfile> GetUserProfile(int userId)
        {
            return await this.Find<UserProfile>(SqlQueries.GET_USER_PROFILE, new { userId = userId });
          
        }
        public async Task<List<ChitPlans>> GetChitsDropDown(int userId)
        {
            if(userId >0)
            {
                return await this.All<ChitPlans>(SqlQueries.Get_Chits_DropDown_for_AcCopy, new { userId = userId });
            }
            else
             return await this.All<ChitPlans>(SqlQueries.Get_Chits_DropDown);
            
        }
        
        public async Task<List<ChitPlans>> GetAllChitPlans(bool groupClosed, int userId)
        {
            if(userId>0)
                return await this.All<ChitPlans>(SqlQueries.Get_Upcoming_ChitPlans);
            else
            {
                var chitPlans =  await this.All<ChitPlans>(SqlQueries.Get_All_ChitPlans_By_Group, new { groupClosed = groupClosed });
                if(chitPlans.Count > 0)
                {
                    foreach(var chitPlan in chitPlans)
                    {
                       var currentMonth = await this.FindBy<int>(SqlQueries.Get_Current_Month, new { groupId = chitPlan.Id });
                        chitPlan.CurrentMonth = currentMonth + 1;
                    }
                }
                return chitPlans;
            }
                
            //else
            //return await this.All<ChitPlans>(SqlQueries.Get_All_ChitPlans);
        }
        public async Task<List<AcDetails>> GetUserAcCopy(int userId, int groupId)
        {
            List<AcDetails> pendingpayments = await this.All<AcDetails>(SqlQueries.Get_User_Pending_Payments, new { groupId, userId });
            List<AcDetails> compltedpayments = await this.All<AcDetails>(SqlQueries.Get_User_Ac_Copy, new { groupId, userId });
            var totalpayments = pendingpayments.Union(compltedpayments).ToList();
            return totalpayments;
        }
        public async Task<int> GetUserId(long mobileNo)
        {
            return await this.FindBy<int>(SqlQueries.Get_User_Id_With_MobileNo, new { mobileNo = mobileNo });
        }
        public async Task<List<EnrollMents>> GetMyChitsData(int userId)
        {
            if (userId > 0) //it is used for my chits in user
            {
                List<EnrollMents> myMhits = new List<EnrollMents>();
                myMhits = await this.All<EnrollMents>(SqlQueries.Get_EnrollMents_By_UserId, new { userId });
                if (myMhits.Count > 0)
                {
                    foreach (var item in myMhits)
                    {
                        item.PaidUpto = await this.FindBy<string>(SqlQueries.Get_RunningMonth_By_GroupId, new { groupId = item.GroupId });

                        var userChitStatus = await this.FindBy<int>(SqlQueries.Get_User_Chit_Status, new { groupId = item.GroupId, userId });
                        if (userChitStatus > 0)
                            item.UserChitSatus = true;
                        else item.UserChitSatus = false;

                        //item.NextAuctionDate = await this.FindBy<DateTime>(SqlQueries.Get_NextAuctionDate, new { groupId = item.GroupId });
                        //if (nextAuctionDate == null)
                        //    item.NextAuctionDate = item.StartDate.AddDays(30);
                        //else
                            //= nextAuctionDate == null? item.StartDate.AddDays(30) : DateTime.Parse(nextAuctionDate);
                    }
                }
                return myMhits;
            }
            return new List<EnrollMents>();
        }

    }
}
