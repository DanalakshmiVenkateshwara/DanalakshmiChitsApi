﻿//using DataAccess.Repositories.Interfaces;
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
        public async Task<List<ChitPlans>> GetAllChitPlans(bool groupClosed)
        {
            //if(groupClosed)
                return await this.All<ChitPlans>(SqlQueries.Get_All_ChitPlans_By_Group, new { groupClosed = groupClosed });
            //else
            //return await this.All<ChitPlans>(SqlQueries.Get_All_ChitPlans);
        }
        public async Task<List<UserPayments>> GetUserAcCopy(int userId, int groupId)
        {
            List<UserPayments> pendingpayments = await this.All<UserPayments>(SqlQueries.Get_User_Pending_Payments, new { groupId, userId });
            List<UserPayments> compltedpayments = await this.All<UserPayments>(SqlQueries.Get_User_Ac_Copy, new { groupId, userId });


            //pendingpayments = (List<UserPayments>)pendingpayments.Concat(compltedpayments);

            return pendingpayments;
        }
           
    }
}
