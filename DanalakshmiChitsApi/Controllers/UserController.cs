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
    public class UserController : ControllerBase
    {
        private IUserManager _userManager { get; set; }
        private IAdminManger _adminManger { get; set; }
        public UserController(IUserManager userManager, IAdminManger adminManger)
        {
            _userManager = userManager;
            _adminManger = adminManger;
        }
        //[HttpPost]
        //[Route("MemberRegistration")]
        //public async Task<int> MemberRegistration(Register register)
        //{
        //    return await _userManager.MemberRegistration(register);
        //}
        [HttpGet]
        [Route("GetUserprofile")]
        public async Task<UserProfile> GetUserProfile(int userId)
        {
            return await _userManager.GetUserProfile(userId);
        }
        [HttpGet]
        [Route("GetAllChitPlans")]
        public async Task<List<ChitPlans>> GetAllChitPlans()
        {
            return await _userManager.GetAllChitPlans();
        }
        [HttpGet]
        [Route("GetUserAcCopy")]
        public async Task<List<UserPayments>> GetUserAcCopy(int userId, int groupId)
        {
            return await _userManager.GetUserAcCopy(userId, groupId);
        }


    }
}
