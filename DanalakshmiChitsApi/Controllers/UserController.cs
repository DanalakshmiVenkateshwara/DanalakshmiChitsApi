﻿using BusinessManagers.Interfaces;
using BusinessObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        private readonly WebSocketController _webSocketController;
        public UserController(IUserManager userManager, IAdminManger adminManger)
        {
            _webSocketController = new WebSocketController();
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
        public async Task<List<ChitPlans>> GetChitsDropDown(int userId)
        {
            return await _userManager.GetChitsDropDown(userId);
        }
        [HttpGet]
        [Route("GetUserAcCopy")]
        public async Task<List<AcDetails>> GetUserAcCopy(int userId, int groupId)
        {
            return await _userManager.GetUserAcCopy(userId, groupId);
        }
        [HttpGet]
        [Route("GetUserId")]
        public async Task<int> GetUserId(long mobileNo)
        {
            return await _userManager.GetUserId(mobileNo);
        }
        [HttpPost("GetMyChitsData")]
        [EnableCors]
        public async Task<List<EnrollMents>> GetMyChitsData(int userId)
        {
            return await _userManager.GetMyChitsData(userId);
        }
        [HttpPost("trigger-action")]
        public async Task<IActionResult> TriggerActionToSocket()
        {
            await _webSocketController.TriggerAction();
            return Ok();
        }
    }
}
