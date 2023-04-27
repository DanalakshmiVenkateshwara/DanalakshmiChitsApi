using BusinessManagers.Interfaces;
using BusinessObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanalakshmiChitsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("DanalakshmiChitsCors")]
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
        [Route("AddAppUsers")]
        public async Task<int> AddAppUsers(AppUsers appUsers)
        {
            return await _adminManger.AddAppUsers(appUsers);
        }
        //we need to get data based on date
        [HttpGet]
        [Route("GetAppUsers")]
        public async Task<List<AppUsers>> GetAppUsers()
        {
            return await _adminManger.GetAppUsers();
        }
        [HttpPost]
        [Route("AddChitPlan")]
        public async Task<int> AddChitPlan(ChitPlans chitPlans)
        {
            return await _adminManger.AddChitPlan(chitPlans);
        }
        [HttpPost]
        [Route("EnrollMent")]
        public async Task<int> EnrollMent(int userId, int groupId)
        {
            return await _adminManger.EnrollMent(userId, groupId, DateTime.Now);
        }
        [HttpGet]
        [Route("GetEnrollMents")]
        public async Task<List<EnrollMents>> GetEnrollMents(int userId, int groupId)
        {
            return await _adminManger.GetEnrollMents(userId,groupId);
        }
        [HttpPost]
        [Route("UserRegistration")]
        public async Task<int> UserRegistration(RegisteUsers registeUsers)
        {
            return await _adminManger.UserRegistration(registeUsers);
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<List<RegisteUsers>> GetUsers(int userId)
        {
            return await _adminManger.GetUsers(userId);
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
        [HttpGet]
        [Route("UserOutStandings")]
        public async Task<List<UserPayments>> UserOutStandings(int groupId)
        {
            return await _adminManger.UserOutStandings(groupId);
        }

        [HttpPost]
        [Route("AddAuctionDetails")]
        public async Task<int> AddAuctionDetails(AuctionDetails auctionDetails)
        {
            return await _adminManger.AddAuctionDetails(auctionDetails);
        }
    }
}
