using BusinessManagers.Interfaces;
using BusinessObjects;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessManagers.Managers
{
    public class AdminManger : IAdminManger
    {
        private IAdminRepository _adminRepository;
        public AdminManger(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public async Task<AdminProfile> GetAdminProfile()
        {
            return await _adminRepository.GetAdminProfile();
        }
        public async Task<int> AddChitPlan(ChitPlans chitPlans)
        {
            return await _adminRepository.AddChitPlan(chitPlans);
        }
        public async Task<int> EnrollMent(int userId, int GroupId, string enrolementDate, bool groupStatus)
        {
            return await _adminRepository.EnrollMent(userId, GroupId, enrolementDate, groupStatus);
        }
        public async Task<int> UserRegistration(RegisteUsers registeUsers)
        {
            return await _adminRepository.UserRegistration(registeUsers);
        }
        public async Task<int> AuctionDetailsByGroup(GroupWiseDetails groupWiseDetails)
        {
            return await _adminRepository.AuctionDetailsByGroup(groupWiseDetails);
        }
        public async Task<int> UserPayments(UserPayments userPayments)
        {
            return await _adminRepository.UserPayments(userPayments);
        }
        public async Task<int> AddAuctionDetails(AuctionDetails auctionDetails)
        {
            return await _adminRepository.AddAuctionDetails(auctionDetails);
        }
    }
    
}