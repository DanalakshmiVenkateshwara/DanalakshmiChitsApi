﻿using BusinessObjects;
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
        Task<List<EnrollMents>> GetEnrollMents(int groupId);
        Task<int> AddAppUsers(AppUsers appUsers);
        Task<List<RegisteUsers>> GetUsers(int userId);
        Task<int> AddChitPlan(ChitPlans chitPlans);
        Task<int> EnrollMent(int userId, int GroupId, DateTime enrollmentDate);
        Task<int> UserRegistration(RegisteUsers registeUsers);
        Task<int> AuctionDetailsByGroup(GroupWiseDetails groupWiseDetails);
        Task<int> UserPayments(UserPayments userPayments);
        Task<List<UserPayments>> UserOutStandings(int groupId);
        Task<int> AddAuctionDetails(AuctionDetails auctionDetails);
    }
}
