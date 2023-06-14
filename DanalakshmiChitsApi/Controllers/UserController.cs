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
    public class UserController : ControllerBase
    {
        private IUserManager _userManager { get; set; }
        private IAdminManger _adminManger { get; set; }
        public UserController(IUserManager userManager, IAdminManger adminManger)
        {
            _userManager = userManager;
            _adminManger = adminManger;
        }
        [HttpGet]
        [Route("GetUserprofile")]
        public async Task<UserProfile> GetUserProfile(int userId)
        {
            return await _userManager.GetUserProfile(userId);
        }
        [HttpGet]
        [Route("GetAllChitPlans/{groupClosed}")]
        public async Task<List<ChitPlans>> GetAllChitPlans(bool groupClosed, int userId = 0)
        {
            return await _userManager.GetAllChitPlans(groupClosed, userId);
        }
        [HttpGet]
        [Route("GetChitsDropDown")]
        public async Task<List<ChitPlans>> GetChitsDropDown()
        {
            return await _userManager.GetChitsDropDown();
        }
        [HttpGet]
        [Route("GetUserAcCopy")]
        public async Task<List<AcDetails>> GetUserAcCopy(int userId, int groupId)
        {
            return await _userManager.GetUserAcCopy(userId, groupId);
        }
    }
}
