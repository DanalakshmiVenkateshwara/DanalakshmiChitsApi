using BusinessManagers.Interfaces;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanalakshmiChitsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IUserManager _userManager { get; set; }
        private IAdminManger _adminManger { get; set; }
        public AdminController(IUserManager userManager, IAdminManger adminManger)
        {
            _userManager = userManager;
            _adminManger = adminManger;
        }
        [HttpGet]
        [Route("GetAdminProfile")]
        public async Task<AdminProfile> GetAdminProfile()
        {
            return await _adminManger.GetAdminProfile();
        }
        [HttpPost]
        [Route("AddChitPlan")]
        public async Task<int> AddChitPlan(ChitPlans chitPlans)
        {
            return await _adminManger.AddChitPlan(chitPlans);
        }
        [HttpPost]
        [Route("EnrollMent")]
        public async Task<int> EnrollMent(int userId, int GroupId, string enrolementDate, bool groupStatus)
        {
            return await _adminManger.EnrollMent(userId, GroupId, enrolementDate, groupStatus);
        }

        [HttpPost]
        [Route("UserRegistration")]
        public async Task<int> UserRegistration(RegisteUsers registeUsers)
        {
            return await _adminManger.UserRegistration(registeUsers);
        }

        [HttpPost]
        [Route("AuctionDetailsByGroup")]
        public async Task<int> AuctionDetailsByGroup(GroupWiseDetails groupWiseDetails)
        {
            return await _adminManger.AuctionDetailsByGroup(groupWiseDetails);
        }
        [HttpPost]
        [Route("UserPayment")]
        public async Task<int> UserPayments(UserPayments userPayments)
        {
            return await _adminManger.UserPayments(userPayments);
        }

        [HttpPost]
        [Route("AddAuctionDetails")]
        public async Task<int> AddAuctionDetails(AuctionDetails auctionDetails)
        {
            return await _adminManger.AddAuctionDetails(auctionDetails);
        }
    }
}
